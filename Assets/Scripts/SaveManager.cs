using System.IO;
using UnityEngine;

[DisallowMultipleComponent]
public class SaveManager : MonoBehaviour
{
    private const string DefaultPlayerName = "Player";
    private const string FileName = "savefile.json";

    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SaveManager>();
            return instance;
        }
    }

    public int bestScore { get; protected set; }
    public string championName { get; protected set; } = DefaultPlayerName;
    public string playerName { get; set; } = DefaultPlayerName;
    
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, FileName);
    private bool isInitialized;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (!isInitialized)
        {
            Load();
            isInitialized = true;
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public int bestScore;
        public string playerName;
        public string championName;
    }

    public void Save()
    {
        var data = new SaveData
        {
            bestScore = bestScore,
            playerName = playerName,
            championName = championName
        };

        string json = JsonUtility.ToJson(data);
        try
        {
            File.WriteAllText(SaveFilePath, json);
            Debug.Log($"Saved to {SaveFilePath}");
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to save: {e.Message}");
        }
    }

    public void Load()
    {
        if (!File.Exists(SaveFilePath))
        {
            SetDefaultValues();
            Debug.Log("No save file found. Using defaults.");
            return;
        }

        try
        {
            string json = File.ReadAllText(SaveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
            playerName = string.IsNullOrEmpty(data.playerName) ? DefaultPlayerName : data.playerName;
            championName = string.IsNullOrEmpty(data.championName) ? DefaultPlayerName : data.championName;

            Debug.Log($"Loaded: {playerName} | {bestScore}");
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to load save: {e.Message}");
            SetDefaultValues();
        }
    }

    public void DeleteSave()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("Save file deleted.");
            }
            SetDefaultValues();
        }
        catch (IOException e)
        {
            Debug.LogError($"Error deleting save: {e.Message}");
        }
    }

    public void UpdateBestScore(int currentScore)
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            championName = playerName;
            Save();
        }
    }

    private void SetDefaultValues()
    {
        bestScore = 0;
        playerName = DefaultPlayerName;
        championName = DefaultPlayerName;
    }

    private void OnApplicationQuit() => Save();
    private void OnApplicationPause(bool pauseStatus) { if (pauseStatus) Save(); }
}