
public class ProfileDataStorage<T> : IProfileDataStorage<T> where T : IProfileData, new()
{
    public T CommonData { get; set; } = new T();
    public T PlayerData { get; set; } = new T();
}