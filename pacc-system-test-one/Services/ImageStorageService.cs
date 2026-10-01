using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace pacc_system_test_one.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(
            Stream stream,
            string fileName,
            string contentType,
            string? folder = null);

        Task DeleteFileAsync(
            string blobName);

        Task DeleteFileByUrlAsync(
            string blobUrl);
    }

    public class ImageStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public ImageStorageService(
            IConfiguration configuration)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=cldv7112st10375898;AccountKey=RgdD0ORSJr34RIC8v1HD4Eyfz5fJklUmrylknVSajv74jznnQYl1IKoLXbg+VPM7th9sswp3Ii9g+ASt1AYV3A==;EndpointSuffix=core.windows.net";

            var containerName = "paccgalleryimages";

            if (string.IsNullOrWhiteSpace(
                connectionString))
            {
                throw new InvalidOperationException(
                    "Azure Blob Storage connection string is not configured.");
            }

            if (string.IsNullOrWhiteSpace(
                containerName))
            {
                throw new InvalidOperationException(
                    "Azure Blob Storage container name is not configured.");
            }

            var blobServiceClient =
                new BlobServiceClient(
                    connectionString);

            _containerClient =
                blobServiceClient.GetBlobContainerClient(
                    containerName);

            _containerClient.CreateIfNotExists(
                Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        }


        public async Task<string> UploadFileAsync(
            Stream stream,
            string fileName,
            string contentType,
            string? folder = null)
        {
            if (stream == null)
                throw new ArgumentNullException(
                    nameof(stream));

            if (string.IsNullOrWhiteSpace(
                fileName))
            {
                throw new ArgumentException(
                    "File name is required.",
                    nameof(fileName));
            }


            var blobName =
                string.IsNullOrWhiteSpace(folder)
                    ? fileName
                    : $"{folder.TrimEnd('/')}/{fileName}";


            var blobClient =
                _containerClient.GetBlobClient(
                    blobName);


            var headers =
                new BlobHttpHeaders
                {
                    ContentType = contentType
                };


            var options =
                new BlobUploadOptions
                {
                    HttpHeaders = headers
                };


            await blobClient.UploadAsync(
                stream,
                options);


            return blobClient.Uri.ToString();
        }


        public async Task DeleteFileAsync(
            string blobName)
        {
            if (string.IsNullOrWhiteSpace(
                blobName))
            {
                return;
            }


            var blobClient =
                _containerClient.GetBlobClient(
                    blobName);


            await blobClient.DeleteIfExistsAsync();
        }


        public async Task DeleteFileByUrlAsync(
            string blobUrl)
        {
            if (string.IsNullOrWhiteSpace(
                blobUrl))
            {
                return;
            }


            if (!Uri.TryCreate(
                blobUrl,
                UriKind.Absolute,
                out var uri))
            {
                return;
            }


            var path =
                uri.AbsolutePath.TrimStart('/');


            var containerPrefix =
                _containerClient.Name + "/";


            if (path.StartsWith(
                containerPrefix,
                StringComparison.OrdinalIgnoreCase))
            {
                path =
                    path.Substring(
                        containerPrefix.Length);
            }


            path =
                Uri.UnescapeDataString(path);


            await DeleteFileAsync(path);
        }
    }
}