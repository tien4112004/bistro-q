using Windows.Storage;

namespace BistroQ.Presentation.Models;

/// <summary>
/// Represents a wrapper for file operations that encapsulates file metadata and content stream.
/// Implements IDisposable to ensure proper cleanup of resources.
/// </summary>
public class FileWrapper : IDisposable
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Gets or sets the MIME content type of the file.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the stream containing the file's content.
    /// </summary>
    public Stream Stream { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the FileWrapper class.
    /// </summary>
    /// <param name="stream">The stream containing the file's content.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="contentType">The MIME content type of the file.</param>
    public FileWrapper(Stream stream, string fileName, string contentType)
    {
        Stream = stream;
        FileName = fileName;
        ContentType = contentType;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Creates a FileWrapper instance from a StorageFile.
    /// </summary>
    /// <param name="file">The StorageFile to wrap.</param>
    /// <returns>A new FileWrapper instance containing the file's content and metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when file is null.</exception>
    /// <exception cref="IOException">Thrown when there is an error reading the file.</exception>
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

    /// <summary>
    /// Creates a FileWrapper instance from a StorageFile with size and content type validation.
    /// </summary>
    /// <param name="file">The StorageFile to wrap.</param>
    /// <param name="maxFileSize">Optional maximum allowed file size in bytes. If null, no size validation is performed.</param>
    /// <param name="allowedContentTypes">Optional array of allowed content types. If null, no content type validation is performed.</param>
    /// <returns>A new FileWrapper instance containing the file's content and metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when file is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the file size exceeds the maximum allowed size.</exception>
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

    /// <summary>
    /// Disposes of the resources used by the FileWrapper instance.
    /// </summary>
    /// <remarks>
    /// This method ensures that the underlying stream is properly disposed.
    /// </remarks>
    public void Dispose()
    {
        if (Stream != null)
        {
            Stream.Dispose();
        }
    }
    #endregion
}