using UnityEngine;

public class Storage
{
    enum StorageKey
    {
        SoundOff,
        Username,
        LastLevel,
        ActiveLevel,
        MaxScore,
        SavedMax,
        CombineScore,
        SavedCombine,
        TrialScore,
        SavedTrial
    }

    static void SetInt(StorageKey key, int value)
    {
        PlayerPrefs.SetInt(key.ToString(), value);
    }

    static int GetInt(StorageKey key)
    {
        return PlayerPrefs.GetInt(key.ToString());
    }

    static void SetFloat(StorageKey key, float value)
    {
        PlayerPrefs.SetFloat(key.ToString(), value);
    }

    static float GetFloat(StorageKey key)
    {
        return PlayerPrefs.GetFloat(key.ToString());
    }

    static void SetString(StorageKey key, string value)
    {
        PlayerPrefs.SetString(key.ToString(), value);
    }

    static string GetString(StorageKey key)
    {
        return PlayerPrefs.GetString(key.ToString());
    }

    public static bool SoundOff
    {
        get { return GetInt(StorageKey.SoundOff) == 1; }
        set { SetInt(StorageKey.SoundOff, value ? 1 : 0); }
    }

    public static string Username
    {
        get { return GetString(StorageKey.Username); }
        set { SetString(StorageKey.Username, value); }
    }

    public static int LastLevel
    {
        get { return GetInt(StorageKey.LastLevel); }
        set { SetInt(StorageKey.LastLevel, value); }
    }

    public static int ActiveLevel
    {
        get { return GetInt(StorageKey.ActiveLevel); }
        set { SetInt(StorageKey.ActiveLevel, value); }
    }

    public static int MaxScore
    {
        get { return GetInt(StorageKey.MaxScore); }
        set { SetInt(StorageKey.MaxScore, value); }
    }

    public static int SavedMax
    {
        get { return GetInt(StorageKey.SavedMax); }
        set { SetInt(StorageKey.SavedMax, value); }
    }

    public static int CombineScore
    {
        get { return GetInt(StorageKey.CombineScore); }
        set { SetInt(StorageKey.CombineScore, value); }
    }

    public static int SavedCombine
    {
        get { return GetInt(StorageKey.SavedCombine); }
        set { SetInt(StorageKey.SavedCombine, value); }
    }

    public static float TrialScore
    {
        get { return GetFloat(StorageKey.TrialScore); }
        set { SetFloat(StorageKey.TrialScore, value); }
    }

    public static float SavedTrial
    {
        get { return GetFloat(StorageKey.SavedTrial); }
        set { SetFloat(StorageKey.SavedTrial, value); }
    }
}
