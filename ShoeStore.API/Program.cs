using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoeStore.API.Data;
using ShoeStore.API.Middlewares;
using ShoeStore.API.Models.Validations;
using ShoeStore.API.Repositories.IShoeRepository;
using ShoeStore.API.Repositories.ShoeRepository;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.API.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// ================= DB =================
builder.Services.AddDbContext<ShoeStoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddControllers();

// ================= CORS =================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ================= FluentValidation =================
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ShoeCreateDtoValidator>();

// ================= Swagger =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================= DI =================
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IShoeRepository, ShoeRepository>();
builder.Services.AddScoped<IShoeServices, ShoeServices>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// ================= Exception Middleware ================
app.UseMiddleware<ExceptionMiddleware>();

// ================= Swagger =================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ================= Pipeline =================
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
