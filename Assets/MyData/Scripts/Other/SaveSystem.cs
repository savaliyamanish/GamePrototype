using System;
using UnityEngine;

public static class SaveSystem
{
    //For Save Game
    private const string GameSaveKey = "MatchGame_LastSave";
    public static void SaveGameData(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(GameSaveKey, json);
        PlayerPrefs.Save();
    }
    public static GameSaveData LoadGameData()
    {    
        string json = PlayerPrefs.GetString(GameSaveKey);
        return JsonUtility.FromJson<GameSaveData>(json);
    }
    public static bool HasSaveGameData()
    {
        return PlayerPrefs.HasKey(GameSaveKey);
    }
    public static void ClearSaveGameData()
    {
        if (PlayerPrefs.HasKey(GameSaveKey))
            PlayerPrefs.DeleteKey(GameSaveKey);
    }
    
    public static int LastSelectedLevelDropdown
    {
        get
        {
            return PlayerPrefs.GetInt("LastSelectedLevelDropdown",0);           
        }
        set
        {
            PlayerPrefs.SetInt("LastSelectedLevelDropdown",value);            
        }
    }
    public static bool SoundSetting
    {
        get
        {
            return PlayerPrefs.GetInt("SoundSetting",0)==1;           
        }
        set
        {
            PlayerPrefs.SetInt("SoundSetting",value?1:0);            
        }
    }
}
    
[Serializable]
public class GameSaveData
{
    public string levelId;

    public int currentScore;
    public int matchedCount;
    public int comboCount;

    public int[] itemIds;
    public bool[] matched;
    public bool[] open;
}
