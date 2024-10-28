namespace Web_253505_Tarhonski.Sevices.FileService
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile formFile);
        Task DeleteFileAsync(string fileUrl);
    }
}
