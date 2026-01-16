namespace ShoeStore.API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string username, string verificationToken);
    }
}
