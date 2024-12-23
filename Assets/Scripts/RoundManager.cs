using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public DynamicDisplayText roundText;
    public DynamicDisplayText gameCompleteText;
    private GameTimer gameTimer;

    private EnemySpawner enemySpawner;

    public SupplyDropManager supplyDropManager;

    public bool gameEnded = false;

    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        UpdateRoundText();
    }

    void Update()
    {
        if (gameEnded) return;

        if (enemySpawner != null)
        {
            UpdateRoundText();
        }

        // Help from ChatGPT
        if (enemySpawner.currentRound >= enemySpawner.rounds.Length && !enemySpawner.roundInProgress)
        {
            EndGame();
        }
    }

    void UpdateRoundText()
    {
        if (roundText != null)
        {
            if (enemySpawner.roundInProgress) {
                roundText.SetDisplayKey("round-text");
                roundText.UpdateDynamicText((enemySpawner.currentRound + 1));
                supplyDropManager.StartRound();
            }
            else
            {
                roundText.SetDisplayKey("round-wait");
                roundText.UpdateText();
            }
        }
    }

    void EndGame()
    {
        gameEnded = true;

        if (gameCompleteText != null)
        {
            gameCompleteText.gameObject.SetActive(true);
            gameCompleteText.SetDisplayKey("game-success");
            gameCompleteText.UpdateText();

            if (gameTimer != null)
            {
                gameTimer.StopTimer();

                float finalTime = gameTimer.GetElapsedTime();
                PlayerPrefs.SetFloat("FinalTime", finalTime);
                PlayerPrefs.Save();
            }
    
        }

        Invoke(nameof(RedirectToScoreScene), 5f);
    }

    void RedirectToScoreScene()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Save Scores");
    }
}
