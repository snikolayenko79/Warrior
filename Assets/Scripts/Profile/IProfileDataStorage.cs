
public interface IProfileDataStorage<T> where T : IProfileData, new()
{
    T CommonData { get; set; }
    T PlayerData { get; set; }
}