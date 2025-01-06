using BistroQ.Domain.Contracts.Services;
using Newtonsoft.Json;
using System.Text;

namespace BistroQ.Service.Common;

/// <summary>
/// Implementation of IFileService that handles file operations using JSON serialization.
/// </summary>
public class FileService : IFileService
{
    /// <summary>
    /// Reads and deserializes a JSON file into the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the JSON content into</typeparam>
    /// <param name="folderPath">The path to the folder containing the file</param>
    /// <param name="fileName">The name of the file to read</param>
    /// <returns>The deserialized object of type T, or default value if file doesn't exist</returns>
    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    /// <summary>
    /// Serializes and saves an object to a JSON file.
    /// Creates the directory if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize</typeparam>
    /// <param name="folderPath">The path to the folder where the file will be saved</param>
    /// <param name="fileName">The name of the file to save</param>
    /// <param name="content">The object to serialize and save</param>
    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    /// <summary>
    /// Deletes a file from the specified folder if it exists.
    /// </summary>
    /// <param name="folderPath">The path to the folder containing the file</param>
    /// <param name="fileName">The name of the file to delete</param>
    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }
}