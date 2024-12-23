using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f; 
    public int damage = 25;    

    public GameObject bloodEffectPrefab;
    private TrailRenderer trail;

    void Start()
    {
        Destroy(gameObject, lifeTime); 

        trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.emitting = false;
            StartCoroutine(EnableTrail());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Help from ChatGPT
            if (bloodEffectPrefab != null)
            {
                // Instantiate at the collision point with the collision normal as the rotation
                Instantiate(bloodEffectPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            }
        }

        // Destroy the bullet upon impact
        Destroy(gameObject);
    }

    // Help from ChatGPT
    private System.Collections.IEnumerator EnableTrail()
    {
        // Wait for a frame to allow the bullet to move
        yield return null;
        if (trail != null)
        {
            trail.emitting = true;
        }
    }
}
