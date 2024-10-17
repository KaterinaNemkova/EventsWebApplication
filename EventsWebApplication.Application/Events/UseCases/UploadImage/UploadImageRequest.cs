using Microsoft.AspNetCore.Http;


namespace EventsWebApplication.Application.Events.UseCases.UploadImage
{
    public class UploadImageRequest
    {
        public required Guid Id { get; init; }
        public IFormFile? File { get; set; }
    }

}
