namespace Web_253505_Tarhonski.Extentions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
