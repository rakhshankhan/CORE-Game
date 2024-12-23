using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Round
    {
        public GameObject[] enemyPrefabs; 
        public int enemyCount;            
        public float spawnRate;           
    }

    public Round[] rounds;                
    public Transform[] spawnPoints;       
    public int currentRound = 0;          

    public float timeBetweenRounds = 5f;
    public bool roundInProgress = false;

    void Start()
    {
        StartCoroutine(StartNextRound());
    }

    IEnumerator StartNextRound()
    {
        while (currentRound < rounds.Length)
        {
            // Wait for a delay between rounds
            if (currentRound > 0)
            {
                Debug.Log("Waiting for next round...");
                yield return new WaitForSeconds(timeBetweenRounds);
            }

            Debug.Log("Starting Round " + (currentRound + 1));
            roundInProgress = true;
            yield return StartCoroutine(SpawnEnemiesForRound(rounds[currentRound]));

            // Help for ChatGPT
            // Wait until all enemies in the round are defeated
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return null;
            }

            Debug.Log("Round " + (currentRound + 1) + " completed!");
            roundInProgress = false;

            // Increment to the next round
            currentRound++;
        }

        Debug.Log("All rounds completed!");
    }

    // Help for ChatGPT
    IEnumerator SpawnEnemiesForRound(Round round)
    {
        for (int i = 0; i < round.enemyCount; i++)
        {
            SpawnEnemy(round.enemyPrefabs[Random.Range(0, round.enemyPrefabs.Length)]);
            yield return new WaitForSeconds(round.spawnRate);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Pick a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the enemy at the spawn point
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Notify the EnemyManager
        EnemyManager.Instance.AddEnemy();
    }

}
