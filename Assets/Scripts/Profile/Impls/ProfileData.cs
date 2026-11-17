using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProfileData : IProfileData, ISerializationCallbackReceiver
{
    [SerializeField] private List<IntEntry> ints = new List<IntEntry>();
    [SerializeField] private List<FloatEntry> floats = new List<FloatEntry>();
    [SerializeField] private List<BoolEntry> bools = new List<BoolEntry>();
    [SerializeField] private List<StringEntry> strings = new List<StringEntry>();

    private readonly Dictionary<string, int> _intDict = new Dictionary<string, int>();
    private readonly Dictionary<string, float> _floatDict = new Dictionary<string, float>();
    private readonly Dictionary<string, bool> _boolDict = new Dictionary<string, bool>();
    private readonly Dictionary<string, string> _stringDict = new Dictionary<string, string>();

    [Serializable] private struct IntEntry { public string key; public int val; }
    [Serializable] private struct FloatEntry { public string key; public float val; }
    [Serializable] private struct BoolEntry { public string key; public bool val; }
    [Serializable] private struct StringEntry { public string key; public string val; }

    public void Set<T>(string key, T value)
    {
        switch (value)
        {
            case int i: _intDict[key] = i; break;
            case float f: _floatDict[key] = f; break;
            case bool b: _boolDict[key] = b; break;
            case string s: _stringDict[key] = s; break;
            default: Debug.LogError($"[GameData] Тип {typeof(T)} не поддерживается."); break;
        }
    }

    public T Get<T>(string key, T defaultValue = default)
    {
        Type type = typeof(T);

        if (type == typeof(int) && _intDict.TryGetValue(key, out int i))
            return (T)(object)i;
        if (type == typeof(float) && _floatDict.TryGetValue(key, out float f))
            return (T)(object)f;
        if (type == typeof(bool) && _boolDict.TryGetValue(key, out bool b))
            return (T)(object)b;
        if (type == typeof(string) && _stringDict.TryGetValue(key, out string s))
            return (T)(object)s;

        return defaultValue;
    }

    public bool HasKey<T>(string key)
    {
        Type type = typeof(T);

        if (type == typeof(int))
            return _intDict.ContainsKey(key);
        if (type == typeof(float))
            return _floatDict.ContainsKey(key);
        if (type == typeof(bool))
            return _boolDict.ContainsKey(key);
        if (type == typeof(string))
            return _stringDict.ContainsKey(key);

        return false;
    }

    public void Remove<T>(string key)
    {
        Type type = typeof(T);

        if (type == typeof(int))
            _intDict.Remove(key);
        else if (type == typeof(float))
            _floatDict.Remove(key);
        else if (type == typeof(bool))
            _boolDict.Remove(key);
        else if (type == typeof(string)) _stringDict.Remove(key);
    }

    public void Clear()
    {
        _intDict.Clear();
        _floatDict.Clear();
        _boolDict.Clear();
        _stringDict.Clear();
    }

    public void OnBeforeSerialize()
    {
        ints.Clear();
        foreach (var kvp in _intDict)
            ints.Add(new IntEntry { key = kvp.Key, val = kvp.Value });

        floats.Clear();
        foreach (var kvp in _floatDict) 
            floats.Add(new FloatEntry { key = kvp.Key, val = kvp.Value });

        bools.Clear();
        foreach (var kvp in _boolDict)
            bools.Add(new BoolEntry { key = kvp.Key, val = kvp.Value });

        strings.Clear();
        foreach (var kvp in _stringDict)
            strings.Add(new StringEntry { key = kvp.Key, val = kvp.Value });
    }

    public void OnAfterDeserialize()
    {
        _intDict.Clear();
        foreach (var item in ints)
            _intDict[item.key] = item.val;

        _floatDict.Clear();
        foreach (var item in floats)
            _floatDict[item.key] = item.val;

        _boolDict.Clear();
        foreach (var item in bools)
            _boolDict[item.key] = item.val;

        _stringDict.Clear();
        foreach (var item in strings)
            _stringDict[item.key] = item.val;
    }
}