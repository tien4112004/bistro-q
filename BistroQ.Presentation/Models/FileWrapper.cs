using Windows.Storage;

namespace BistroQ.Presentation.Models;

public class FileWrapper : IDisposable
{
    public string FileName { get; set; }

    public string ContentType { get; set; }

    public Stream Stream { get; set; }

    public FileWrapper(Stream stream, string fileName, string contentType)
    {
        Stream = stream;
        FileName = fileName;
        ContentType = contentType;
    }

    public static async Task<FileWrapper> FromStorageFileAsync(StorageFile file)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        try
        {
            var stream = await file.OpenStreamForReadAsync();
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var contentType = file.ContentType;
            var fileName = file.Name;

            return new FileWrapper(memoryStream, fileName, contentType);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to read file: {file.Name}", ex);
        }
    }

    public static async Task<FileWrapper> FromStorageFileWithValidationAsync(
        StorageFile file,
        long? maxFileSize = null,
        string[]? allowedContentTypes = null)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        // Validate file size
        if (maxFileSize.HasValue)
        {
            var properties = await file.GetBasicPropertiesAsync();
            if ((long)properties.Size > maxFileSize.Value)
            {
                throw new InvalidOperationException(
                    $"File size ({properties.Size} bytes) exceeds maximum allowed size ({maxFileSize.Value} bytes)");
            }
        }

        return await FromStorageFileAsync(file);
    }

    public void Dispose()
    {
        if (Stream != null)
        {
            Stream.Dispose();
        }
    }
}
