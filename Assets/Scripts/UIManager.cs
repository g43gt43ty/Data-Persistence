using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    
    private void Start()
{
    Debug.Log("UIManager started");
    
    if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(InitializeAfterSaveManager());
        }
}

private IEnumerator InitializeAfterSaveManager()
{
    // Ждем инициализации SaveManager
    while (SaveManager.Instance == null)
    {
        yield return null;
    }
    
    // Дополнительная задержка для полной загрузки
    yield return new WaitForEndOfFrame();

    if (nameInputField == null)
    {
        Debug.LogError("InputField reference is null!");
        yield break;
    }

    // Устанавливаем начальное значение
    UpdateBestScoreText();
    
    // Подписываемся на события после инициализации
    nameInputField.onValueChanged.AddListener(OnNameChanged);
}

public void OnNameChanged(string newName)
{
    Debug.Log($"Input changed: '{newName}' (length: {newName.Length})");
    
    if (SaveManager.Instance != null)
    {
        SaveManager.Instance.playerName = newName;
        SaveManager.Instance.SaveSave();
    }
    else
    {
        Debug.LogWarning("SaveManager not ready yet");
    }
}

    private void UpdateBestScoreText()
    {
        if (bestScoreText == null) return;
        
        string currentName = SaveManager.Instance?.championName ?? "Unknown";
        int score = SaveManager.Instance?.bestScore ?? 0;
        bestScoreText.text = $"Best Score: {currentName}: {score}";
    }
}
