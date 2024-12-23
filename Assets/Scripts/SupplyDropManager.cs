using System.Collections;
using UnityEngine;
using TMPro;

public class SupplyDropManager : MonoBehaviour
{
    public GameObject supplyDropPrefab;    
    public Transform[] spawnPoints;       
    public int supplyDropsPerRound = 2;   

    public float spawnInterval = 10f;     
    private bool spawning = false;

    public DynamicDisplayText supplyDropNotificationText; 
    public float notificationDuration = 3f;    


    void Start()
    {
        HideSupplyDropNotification();
        StartRound();
    }

    public void StartRound()
    {
        if (!spawning)
        {
            spawning = true;
            StartCoroutine(SpawnSupplyDrops());
        }
    }

    // Help from ChatGPT
    IEnumerator SpawnSupplyDrops()
    {
        for (int i = 0; i < supplyDropsPerRound; i++)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Spawn a supply drop at a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(supplyDropPrefab, spawnPoint.position, Quaternion.identity);

            ShowSupplyDropNotification();

            Debug.Log("Supply drop spawned at: " + spawnPoint.position);
        }

        spawning = false; // Mark spawning complete
    }

    void ShowSupplyDropNotification()
    {
        if (supplyDropNotificationText != null)
        {
            supplyDropNotificationText.UpdateText();
            supplyDropNotificationText.gameObject.SetActive(true);

            Invoke(nameof(HideSupplyDropNotification), notificationDuration);
        }
    }

    void HideSupplyDropNotification()
    {
        if (supplyDropNotificationText != null)
        {
            supplyDropNotificationText.gameObject.SetActive(false);
        }
    }
}
