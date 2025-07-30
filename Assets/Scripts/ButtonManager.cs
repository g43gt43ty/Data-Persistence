using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartNew()
    {
        SaveManager.Instance.LoadSave();
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        SaveManager.Instance.SaveSave();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
    public void GoMenu()
    {
        SaveManager.Instance.SaveSave();
        SceneManager.LoadScene(0);
        SaveManager.Instance.SaveSave();
    }
    public void Erase()
    {
        SaveManager.Instance.DeleteSave();
    }
}
