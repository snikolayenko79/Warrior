using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class JsonProfileSaver : IProfileSaver
{
    private readonly string _basePath;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public JsonProfileSaver()
    {
        _basePath = Path.Combine(Application.persistentDataPath, "Profiles");

        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    private string GetPath(string fileName) => Path.Combine(_basePath, $"{fileName}.json");

    public async Task SaveAsync<T>(string fileName, T data)
    {
        // 1. Мгновенно делаем текстовый снимок данных в главном потоке до очереди
        string json = JsonUtility.ToJson(data, true);

        // 2. В очередь на запись на диск ставим уже готовую строку json
        await _semaphore.WaitAsync();
        try
        {
            string targetPath = GetPath(fileName);
            string tempPath = targetPath + ".tmp";

            await File.WriteAllTextAsync(tempPath, json);

            if (File.Exists(targetPath))
                File.Replace(tempPath, targetPath, null);
            else
                File.Move(tempPath, targetPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonProfileSaver] Ошибка сохранения файла {fileName}: {e.Message}");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<T> LoadAsync<T>(string fileName) where T : new()
    {
        await _semaphore.WaitAsync();
        try
        {
            string targetPath = GetPath(fileName);

            if (!File.Exists(targetPath))
            {
                return new T();
            }

            string json = await File.ReadAllTextAsync(targetPath);
            return JsonUtility.FromJson<T>(json) ?? new T();
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonProfileSaver] Ошибка загрузки файла {fileName}: {e.Message}");
            return new T();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}