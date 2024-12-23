using UnityEngine;
using TMPro;
using System.Collections;

public class WeaponAim : MonoBehaviour
{
    public Camera mainCamera;
    public Transform weaponBarrel;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;

    public int maxMagazineSize = 30;        
    public int currentAmmo = 30;            
    public int reserveAmmo = 0;             
    public float reloadTime = 2f;

    public float bulletSpeed = 20f;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    public DynamicDisplayText ammoText;
    public DynamicDisplayText reloadText;

    public AudioSource audioSource;
    public AudioClip gunshotSound;

    private bool isReloading = false;

    void Start()
    {
        reloadText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isReloading) return;

        // Update ammo display
        UpdateAmmoUI();

        // Check for reload input
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        // Help from ChatGPT
        // Raycast to aim the weapon
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, 1500f))
        {
            // Rotate the weapon to point at the raycast hit point
            Vector3 targetDirection = hit.point - weaponBarrel.position;
            weaponBarrel.rotation = Quaternion.LookRotation(targetDirection);
        }
        else
        {
            // Default aim direction if no hit point is detected
            weaponBarrel.forward = mainCamera.transform.forward;
        }

        // Fire weapon when aiming and left mouse button is clicked
        if (Input.GetMouseButton(1))
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0)
                {
                    nextFireTime = Time.time + fireRate;
                    Fire();
                }
                else if (currentAmmo == 0)
                {
                    Debug.Log("Out of ammo!");
                    reloadText.gameObject.SetActive(true);
                    reloadText.SetDisplayKey("reload-text");
                    reloadText.UpdateText(); // Press R to Reload.
                }
            }
        }
    }

    void Fire()
    {
        // Instantiate the bullet prefab at the weapon barrel's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, weaponBarrel.position + weaponBarrel.forward, weaponBarrel.rotation);

        currentAmmo--;

        // Apply forward velocity to the bullet
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = weaponBarrel.forward * bulletSpeed;
        }

        // Play the muzzle flash particle system
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (audioSource != null && gunshotSound != null)
        {
            audioSource.PlayOneShot(gunshotSound);
        }

        Debug.Log("Weapon fired!");
    }

    // Help from ChatGPT
    IEnumerator Reload()
    {
        if (currentAmmo == maxMagazineSize || reserveAmmo == 0)
        {
            Debug.Log("Cannot reload!");
            yield break;
        }

        isReloading = true;
        Debug.Log("Reloading...");
        reloadText.gameObject.SetActive(true);
        reloadText.SetDisplayKey("reloading-text");
        reloadText.UpdateText(); // Reloading...
        yield return new WaitForSeconds(reloadTime);

        // Calculate ammo to reload
        int ammoToReload = Mathf.Min(maxMagazineSize - currentAmmo, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;
        Debug.Log("Reload complete! Ammo: " + currentAmmo + "/" + reserveAmmo);
        reloadText.SetDisplayKey("reload-complete");
        reloadText.UpdateText(); // Reload Complete.
        yield return new WaitForSeconds(3);
        reloadText.gameObject.SetActive(false);
    }

    void UpdateAmmoUI()
    {
        ammoText.UpdateDynamicText(currentAmmo, reserveAmmo);
    }
}

