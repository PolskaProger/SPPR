namespace Web_253505_Tarhonski.Sevices.AuthService
{
    public interface IAuthService
    {
        Task<(bool Result, string ErrorMessage)> RegisterUserAsync(string email, string password, IFormFile? avatar);
    }
}
