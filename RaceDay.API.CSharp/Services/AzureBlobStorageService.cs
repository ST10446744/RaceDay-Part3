using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDay.API.CSharp.Services;

public class AzureBlobStorageService
{
    private const long MaxFileSize = 5 * 1024 * 1024;

    private readonly BlobContainerClient _container;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        var connectionString =
            configuration["AzureBlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException(
                "Azure Blob Storage connection string is missing.");

        var containerName =
            configuration["AzureBlobStorage:ContainerName"]
            ?? "raceday-images";

        var serviceClient = new BlobServiceClient(connectionString);
        _container = serviceClient.GetBlobContainerClient(containerName);
    }

    public async Task<string> UploadAsync(
        IFormFile file,
        string folderName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Choose an image file.");

        if (file.Length > MaxFileSize)
            throw new ArgumentException("Images must be 5 MB or smaller.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var allowedTypes = new Dictionary<string, string>
        {
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png",
            [".webp"] = "image/webp"
        };

        if (!allowedTypes.TryGetValue(extension, out var expectedContentType) ||
            !string.Equals(file.ContentType, expectedContentType,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Choose a JPG, PNG, or WebP image.");
        }

        var blobName = $"{folderName}/{Guid.NewGuid():N}{extension}";
        var blob = _container.GetBlobClient(blobName);

        await using var stream = file.OpenReadStream();

        await blob.UploadAsync(stream, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = expectedContentType
            }
        });

        return blobName;
    }

    public async Task<(Stream Stream, string ContentType)> DownloadAsync(
        string blobName)
    {
        var blob = _container.GetBlobClient(blobName);
        var download = await blob.DownloadStreamingAsync();

        return (
            download.Value.Content,
            download.Value.Details.ContentType ?? "application/octet-stream");
    }
}