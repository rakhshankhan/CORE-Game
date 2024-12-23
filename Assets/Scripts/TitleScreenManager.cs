using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("Load Scores");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }
}