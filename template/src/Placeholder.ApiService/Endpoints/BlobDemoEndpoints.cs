using Placeholder.Core.BlobDemo;

namespace Placeholder.ApiService.Endpoints
{
    public static class BlobDemoEndpoints
    {
        public static void MapBlobDemoEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var blobDemoGroup = endpoints.MapGroup("/blob-demo");

            blobDemoGroup
                .MapPost(
                    "/upload",
                    async (IFormFile file, IBlobDemoService service) =>
                    {
                        return await service.UploadImage(file);
                    }
                )
                .DisableAntiforgery()
                .Accepts<IFormFile>("multipart/form-data");
        }
    }
}
