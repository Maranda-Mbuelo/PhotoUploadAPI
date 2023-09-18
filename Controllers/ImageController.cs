using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Threading.Tasks;
using PhotoUploadAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly Cloudinary _cloudinary;

    public ImageController(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel model)
    {
        if (model.Image == null || model.Image.Length == 0)
        {
            return BadRequest("No image selected.");
        }

        using (var stream = model.Image.OpenReadStream())
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(model.Image.FileName, stream),
                // You can add optional Cloudinary transformations here
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                return BadRequest(uploadResult.Error.Message);
            }

            return Ok(new { imageUrl = uploadResult.SecureUri });
        }
    }

    [HttpGet("getall")]
    public IActionResult GetAllImages()
    {
        // Assuming you have a Cloudinary account instance stored as _cloudinary
        var listParams = new ListResourcesParams();
        var result = _cloudinary.ListResources(listParams);

        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }

        // Extract the URLs of all images and return them in the response
        var imageUrls = result.Resources.Select(r => r.SecureUrl.AbsoluteUri).ToList();
        return Ok(imageUrls);
    }

    [HttpDelete("delete/{imageId}")]
    public IActionResult DeleteImageById(string imageId)
    {
        // Use the image ID to delete the image from Cloudinary
        var deleteParams = new DeletionParams(imageId);

        var result = _cloudinary.Destroy(deleteParams);

        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }

        return Ok("Image deleted successfully.");
    }


}
