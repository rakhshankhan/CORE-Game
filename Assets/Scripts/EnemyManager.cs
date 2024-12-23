using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public DynamicDisplayText enemyCountText;
    private int enemiesRemaining = 0;

    // Help from ChatGPT
    void Awake()
    {
        // Ensure this is the only instance of the EnemyManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEnemy()
    {
        enemiesRemaining++;
        UpdateEnemyCountUI();
    }

    public void RemoveEnemy()
    {
        enemiesRemaining--;
        enemiesRemaining = Mathf.Max(0, enemiesRemaining); // Ensure count doesn't go below zero
        UpdateEnemyCountUI();
    }

    private void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.UpdateDynamicText(enemiesRemaining);
        }
    }

    public int GetEnemyCount()
    {
        return enemiesRemaining;
    }
}
