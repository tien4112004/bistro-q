namespace BistroQ.Domain.Contracts.Services;

/// <summary>
/// Interface for handling file operations with JSON serialization.
/// Provides methods for reading, saving, and deleting files.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Reads and deserializes a JSON file into the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the JSON content into</typeparam>
    /// <param name="folderPath">The path to the folder containing the file</param>
    /// <param name="fileName">The name of the file to read</param>
    /// <returns>The deserialized object of type T, or default value if file doesn't exist</returns>
    T Read<T>(string folderPath, string fileName);

    /// <summary>
    /// Serializes and saves an object to a JSON file.
    /// Creates the directory if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize</typeparam>
    /// <param name="folderPath">The path to the folder where the file will be saved</param>
    /// <param name="fileName">The name of the file to save</param>
    /// <param name="content">The object to serialize and save</param>
    void Save<T>(string folderPath, string fileName, T content);

    /// <summary>
    /// Deletes a file from the specified folder if it exists.
    /// </summary>
    /// <param name="folderPath">The path to the folder containing the file</param>
    /// <param name="fileName">The name of the file to delete</param>
    void Delete(string folderPath, string fileName);
}