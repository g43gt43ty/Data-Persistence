using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartNew()
    {
        SaveManager.Instance.Load();
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        SaveManager.Instance.Save();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
    public void GoMenu()
    {
        SaveManager.Instance.Save();
        SceneManager.LoadScene(0);
        SaveManager.Instance.Save();
    }
    public void Erase()
    {
        SaveManager.Instance.DeleteSave();
    }
}
