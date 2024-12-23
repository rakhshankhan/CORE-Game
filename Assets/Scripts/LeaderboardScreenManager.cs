using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardScreenManager : MonoBehaviour
{
    public void BackToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}