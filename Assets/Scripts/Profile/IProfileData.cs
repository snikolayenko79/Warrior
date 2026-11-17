
public interface IProfileData
{
    void Set<T>(string key, T value);
    T Get<T>(string key, T defaultValue = default);
    bool HasKey<T>(string key);
    void Remove<T>(string key);
    void Clear();
}