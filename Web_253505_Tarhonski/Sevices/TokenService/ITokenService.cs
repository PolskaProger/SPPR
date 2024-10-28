namespace Web_253505_Tarhonski.Sevices.TokenService
{
    public interface ITokenAccessor
    {
        Task<string> GetAccessTokenAsync();
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);
    }
}
