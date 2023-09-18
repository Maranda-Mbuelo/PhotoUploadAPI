using Microsoft.AspNetCore.Http;

namespace PhotoUploadAPI.Models
{
    public class ImageUploadModel
    {
        public IFormFile Image { get; set; }
    }
}
