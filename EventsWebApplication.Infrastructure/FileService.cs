using EventsWebApplication.Core;
using Microsoft.AspNetCore.Http;


namespace EventsWebApplication.Infrastructure
{
    public class FileService : IFileService
    {
        public async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }

}
