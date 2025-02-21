namespace Placeholder.Core.BlobDemo;

public interface IBlobDemoService
{
    Task<IResult> UploadImage(IFormFile file);
}
