using UnityEngine;

public class SupplyDrop : MonoBehaviour
{

    public int ammoAmount = 30;
    public AudioClip pickupSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponAim weapon = other.GetComponentInChildren<WeaponAim>();
            if (weapon != null)
            {
                weapon.reserveAmmo += ammoAmount;
                Debug.Log("Picked up supply drop! Reserve ammo: " + weapon.reserveAmmo);

                PlayPickupSound();

                Destroy(gameObject);
            }
        }
    }

    // Help from ChatGPT
    void PlayPickupSound()
    {
        if (pickupSound != null)
        {
            GameObject soundObject = new GameObject("PickupSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = pickupSound;
            audioSource.playOnAwake = false;

            audioSource.spatialBlend = 0;
            audioSource.volume = 1f;

            audioSource.Play();

            Destroy(soundObject, pickupSound.length);
        }
    }
}
