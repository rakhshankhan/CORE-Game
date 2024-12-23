using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int attackDamage = 10;

    public NavMeshAgent agent;
    public Animator animator;

    public float healthBarPosition = 3f;
    public GameObject healthBarPrefab;

    public float despawnTime = 2f;

    private Image healthBarForeground;
    private Transform core;
    private GameObject healthBarInstance;
    private bool isAttacking = false;
    private Core coreScript;
    private Transform cameraTransform;

    public AudioSource audioSource;         
    public AudioClip zombieGrowl;           

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        cameraTransform = Camera.main.transform;

        healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * healthBarPosition, Quaternion.identity);

        // Set the health bar canvas to follow the enemy
        healthBarInstance.transform.SetParent(transform);

        // Find the Foreground Image in the instantiated health bar
        healthBarForeground = healthBarInstance.transform.Find("Background/Foreground").GetComponent<Image>();

        UpdateHealthBar();

        if (Core.Instance != null)
        {
            core = Core.Instance.transform;
            coreScript = Core.Instance;

            agent.SetDestination(core.position);
        }
        else
        {
            Debug.LogError("Core instance not found in the scene!");
        }

        // Play zombie growl sound periodically
        InvokeRepeating(nameof(PlayZombieGrowl), Random.Range(3f, 8f), Random.Range(5f, 10f));
    }


    void Update()
    {
        if (healthBarInstance != null)
        {
            healthBarInstance.transform.position = transform.position + Vector3.up * healthBarPosition;

            // Make the health bar face the camera
            healthBarInstance.transform.LookAt(cameraTransform);
            healthBarInstance.transform.Rotate(0, 180, 0); // Flip to face the player properly
        }

        if (core != null && !isAttacking)
        {
            // Keep setting the destination in case the core moves
            agent.SetDestination(core.position);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarForeground != null)
        {
            // Update the fill amount based on the current health percentage
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
            Die();
        }
    }

    // Help from ChatGPT
    void Die()
    {
        Collider enemyCollider = GetComponent<Collider>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // Play death animation
        animator.SetTrigger("Die");
        agent.isStopped = true;

        // Notify the EnemyManager
        EnemyManager.Instance.RemoveEnemy();

        Destroy(healthBarInstance);
        Destroy(gameObject, despawnTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Core") && !isAttacking)
        {
            isAttacking = true;
            agent.isStopped = true;
            animator.SetTrigger("Attack");
        }
    }

    void PlayZombieGrowl()
    {
        if (audioSource != null && zombieGrowl != null)
        {
            audioSource.pitch = Random.Range(0.5f, 1.5f); // Slightly randomize pitch
            audioSource.PlayOneShot(zombieGrowl);
        }
    }

    public void DealDamage()
    {
        if (coreScript != null)
        {
            coreScript.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogError("Core script is null!");
        }
    }
}
