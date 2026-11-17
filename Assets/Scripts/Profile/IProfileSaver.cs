using System.Threading.Tasks;

public interface IProfileSaver
{
    Task SaveAsync<T>(string fileName, T data);
    Task<T> LoadAsync<T>(string fileName) where T : new();
    T LoadSync<T>(string fileName) where T : new();
    string ProfileFolder { get; }
}