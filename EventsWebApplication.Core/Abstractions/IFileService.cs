using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Core
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderPath);
        void DeleteFile(string filePath);
    }
}