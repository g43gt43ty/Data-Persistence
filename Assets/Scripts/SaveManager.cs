using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public int bestScore;
    public string playerName;
    public string championName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Гарантируем загрузку перед первым использованием
        if (!isInitialized)
        {
            LoadSave();
            isInitialized = true;
        }
    }

private bool isInitialized = false;

    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string playerName;
        public string championName;
    }

    public void SaveSave()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.playerName = playerName; // Всегда сохраняем имя
        data.championName = championName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    private void OnApplicationQuit()
    {
        SaveSave();
    }

    public void LoadSave()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log("Loading from: " + path);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
            playerName = string.IsNullOrEmpty(data.playerName) ? "Player" : data.playerName;
            championName=string.IsNullOrEmpty(data.championName) ? "Player" : data.championName;

            Debug.Log($"Loaded: {playerName} | {bestScore}");
        }
        else
        {
            bestScore = 0;
            playerName = "Player"; // Значение по умолчанию
            Debug.Log("No save file found. Using default values.");
        }
    }
    public void DeleteSave()
{
    string path = Application.persistentDataPath + "/savefile.json";
    
    try
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Сохранение успешно удалено");
            
            // Сбрасываем значения на значения по умолчанию
            bestScore = 0;
            playerName = "";
            
            // Сохраняем "пустые" данные (опционально)
            SaveSave();
        }
        else
        {
            Debug.Log("Файл сохранения не найден");
        }
    }
    catch (System.Exception e)
    {
        Debug.LogError($"Ошибка при удалении сохранения: {e.Message}");
    }
}

    // Обновляет рекорд, если текущий счет больше
    public void UpdateBestScore(int currentScore)
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            championName = playerName;
            SaveSave();
        }
    }
}
