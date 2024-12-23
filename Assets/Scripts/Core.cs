using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour
{
    public static Core Instance;

    // Help from ChatGPT
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int maxHealth = 200;
    private int currentHealth;

    public float healthBarPosition = 3f;
    public GameObject healthBarPrefab;
    private Image healthBarForeground;

    private GameObject healthBarInstance;
    private Transform cameraTransform;

    public DynamicDisplayText gameOverText;
    public float gameOverDelay = 4f;

    void Start()
    {
        currentHealth = maxHealth;

        cameraTransform = Camera.main.transform;

        // Help from ChatGPT
        healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * healthBarPosition, Quaternion.identity);
        healthBarForeground = healthBarInstance.transform.Find("Background/Foreground").GetComponent<Image>();

        UpdateHealthBar();

        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (healthBarInstance != null)
        {
            healthBarInstance.transform.position = transform.position + Vector3.up * healthBarPosition;

            // Make the health bar face the camera
            healthBarInstance.transform.LookAt(cameraTransform);
            healthBarInstance.transform.Rotate(0, 180, 0);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarForeground != null)
        {
            healthBarForeground.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            OnCoreDestroyed();
        }
    }

    void OnCoreDestroyed()
    {
        if (gameOverText != null)
        {
            gameOverText.SetDisplayKey("game-over");
            gameOverText.UpdateText();
            gameOverText.gameObject.SetActive(true);
        }

        Invoke(nameof(EndGame), gameOverDelay);
    }

    void EndGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
