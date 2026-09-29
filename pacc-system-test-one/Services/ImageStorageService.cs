using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace pacc_system_test_one.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string? folder = null);

        Task DeleteFileAsync(string blobUrl);
    }

    public class ImageStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public ImageStorageService(IConfiguration configuration)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=cldv7112st10375898;AccountKey=RgdD0ORSJr34RIC8v1HD4Eyfz5fJklUmrylknVSajv74jznnQYl1IKoLXbg+VPM7th9sswp3Ii9g+ASt1AYV3A==;EndpointSuffix=core.windows.net";

            var containerName ="paccgalleryimages";

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Azure Storage connection string is not configured.");
            }

            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new InvalidOperationException(
                    "Azure Storage container name is not configured.");
            }

            var blobServiceClient =
                new BlobServiceClient(connectionString);

            _containerClient =
                blobServiceClient.GetBlobContainerClient(containerName);

            _containerClient.CreateIfNotExists(
                PublicAccessType.Blob);
        }

        public async Task<string> UploadFileAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string? folder = null)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            if (!fileStream.CanRead)
            {
                throw new ArgumentException(
                    "The supplied file stream cannot be read.");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(
                    "File name is required.",
                    nameof(fileName));
            }

            var extension =
                Path.GetExtension(fileName)
                    .ToLowerInvariant();

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".webp"
            };

            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException(
                    "Only JPG, JPEG, PNG, GIF and WEBP images are allowed.");
            }

            var generatedFileName =
                $"{Guid.NewGuid():N}{extension}";

            var blobName =
                string.IsNullOrWhiteSpace(folder)
                    ? generatedFileName
                    : $"{folder.TrimEnd('/')}/{generatedFileName}";

            var blobClient =
                _containerClient.GetBlobClient(blobName);

            var uploadOptions =
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType =
                            string.IsNullOrWhiteSpace(contentType)
                                ? GetContentType(extension)
                                : contentType
                    }
                };

            await blobClient.UploadAsync(
                fileStream,
                uploadOptions);

            return blobClient.Uri.ToString();
        }

        public async Task DeleteFileAsync(string blobUrl)
        {
            if (string.IsNullOrWhiteSpace(blobUrl))
            {
                return;
            }

            try
            {
                var uri = new Uri(blobUrl);

                var blobName =
                    Uri.UnescapeDataString(
                        uri.AbsolutePath.TrimStart('/'));

                var containerPrefix =
                    _containerClient.Name + "/";

                if (blobName.StartsWith(
                    containerPrefix,
                    StringComparison.OrdinalIgnoreCase))
                {
                    blobName =
                        blobName.Substring(
                            containerPrefix.Length);
                }

                var blobClient =
                    _containerClient.GetBlobClient(blobName);

                await blobClient.DeleteIfExistsAsync();
            }
            catch (UriFormatException)
            {
                throw new ArgumentException(
                    "The supplied blob URL is invalid.");
            }
        }

        private static string GetContentType(
            string extension)
        {
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}