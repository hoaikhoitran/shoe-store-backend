using AutoMapper;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using ShoeStore.API.Core;
using ShoeStore.API.Models;
using ShoeStore.API.Models.DTOs;
using ShoeStore.API.Models.Entities;
using ShoeStore.API.Repositories.IShoeRepository;
using ShoeStore.API.Services.Interfaces;
using System.Security.Cryptography;

namespace ShoeStore.API.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepo, 
            ITokenService tokenService,
            IEmailService emailService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Check if username already exists
            var existingUserByUsername = await _userRepo.GetUserByUsernameAsync(dto.Username);
            if (existingUserByUsername != null)
                throw new BadRequestException("Username already exists");

            // Check if email already exists
            var existingUserByEmail = await _userRepo.GetUserByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
                throw new BadRequestException("Email already exists");

            // Create new user using AutoMapper
            var user = _mapper.Map<User>(dto);
            
            // Set values that require business logic
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.EmailVerificationToken = GenerateVerificationToken();
            user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);

            await _userRepo.AddAsync(user);

            // Send verification email
            try
            {
                await _emailService.SendVerificationEmailAsync(user.Email, user.Username, user.EmailVerificationToken);
            }
            catch (Exception)
            {
                // Log error but don't fail registration
                // User can request resend verification email later
            }

            return new AuthResponseDto
            {
                AccessToken = null,
                RefreshToken = null,
                Message = "Registration successful. Please check your email to verify your account."
            };
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            var user = await _userRepo.GetUserByVerificationTokenAsync(token);

            if (user == null)
                throw new BadRequestException("Invalid or expired verification token");

            if (user.EmailVerified)
                throw new BadRequestException("Email already verified");

            user.EmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepo.UpdateAsync(user);

            return true;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetUserByUsernameAsync(dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new BadRequestException("Invalid Username or password");

            if (!user.EmailVerified)
                throw new BadRequestException("Please verify your email before logging in");

            if (!user.IsActive)
                throw new BadRequestException("Account is inactive");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepo.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDto> GoogleLoginAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["GoogleOAuth:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                if (payload == null)
                    throw new BadRequestException("Invalid Google token");

                // Check if user exists by Google ID
                var user = await _userRepo.GetUserByGoogleIdAsync(payload.Subject);

                if (user == null)
                {
                    // Check if email already exists (user registered with email/password)
                    user = await _userRepo.GetUserByEmailAsync(payload.Email);

                    if (user != null)
                    {
                        // Link Google account to existing user
                        user.GoogleId = payload.Subject;
                        user.EmailVerified = true; // Google emails are verified
                    }
                    else
                    {
                        // Create new user from Google
                        user = new User
                        {
                            Username = payload.Email.Split('@')[0] + "_" + payload.Subject.Substring(0, 8),
                            Email = payload.Email,
                            PasswordHash = string.Empty, // No password for Google users
                            Role = Role.User,
                            IsActive = true,
                            EmailVerified = true,
                            GoogleId = payload.Subject
                        };

                        await _userRepo.AddAsync(user);
                    }
                }

                if (!user.IsActive)
                    throw new BadRequestException("Account is inactive");

                var accessToken = _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5);
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepo.UpdateAsync(user);

                return new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (InvalidJwtException)
            {
                throw new BadRequestException("Invalid Google token");
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Google authentication failed: {ex.Message}");
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);

            if (user == null)
                throw new BadRequestException("Invalid token");
            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new BadRequestException("Refresh token expired");

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userRepo.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        private string GenerateVerificationToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}
