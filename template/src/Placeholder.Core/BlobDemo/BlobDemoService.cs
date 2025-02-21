using Azure.Storage.Blobs;

namespace Placeholder.Core.BlobDemo;

public class BlobDemoService(BlobServiceClient blobServiceClient, ILogger<BlobDemoService> logger)
    : IBlobDemoService
{
    private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
    private readonly ILogger<BlobDemoService> _logger = logger;
    private const string ContainerName = "images";

    private const int MaxFileSizeInMb = 10;

    private readonly string[] AllowedFileTypes = [".jpg", ".jpeg", ".png"];

    public async Task<IResult> UploadImage(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("No file was uploaded");
            }

            var fileSizeInMb = file.Length / (1024 * 1024);
            if (fileSizeInMb > MaxFileSizeInMb)
            {
                return Results.BadRequest(
                    $"File size exceeds the maximum limit of {MaxFileSizeInMb} MB"
                );
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedFileTypes.Contains(fileExtension))
            {
                return Results.BadRequest(
                    $"Only the following file types are allowed: {string.Join(", ", AllowedFileTypes)}"
                );
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await containerClient.CreateIfNotExistsAsync(
                Azure.Storage.Blobs.Models.PublicAccessType.Blob
            );

            string blobName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return Results.Ok(
                new { FileName = file.FileName, BlobUrl = blobClient.Uri.ToString() }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to blob storage");
            return Results.Problem("An error occurred while uploading the file");
        }
    }
}
