using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private ParticleSystem asteroidDestroyPS;
    [SerializeField] private AudioClip asteroidDestroySound;
    [SerializeField] private AudioClip powerupPickupSound;     // Sound for picking up the powerup
    [SerializeField] private AudioClip powerupEndSound;        // Sound when powerup duration ends
    [SerializeField] private float powerupDuration = 10f;
    [SerializeField] private float invincibilityDuration = 10f;

    private bool hasPowerup = false;
    private bool isInvincible = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            ActivatePowerup();
            Destroy(other.gameObject); // Destroy the powerup object
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasPowerup && collision.gameObject.CompareTag("Asteroid"))
        {
            DestroyAsteroid(collision.gameObject, collision.contacts[0].point); // Destroy asteroid
        }
    }

    void ActivatePowerup()
    {
        // Play powerup pickup sound
        if (powerupPickupSound != null)
        {
            AudioSource.PlayClipAtPoint(powerupPickupSound, Camera.main.transform.position);
        }

        StartCoroutine(PowerupTimer());
        StartCoroutine(InvincibilityTimer());
    }

    IEnumerator PowerupTimer()
    {
        hasPowerup = true;
        yield return new WaitForSeconds(powerupDuration);
        hasPowerup = false;

        // Play powerup end sound
        if (powerupEndSound != null)
        {
            AudioSource.PlayClipAtPoint(powerupEndSound, Camera.main.transform.position);
        }
    }

    IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    void DestroyAsteroid(GameObject asteroid, Vector3 collisionPoint)
    {
        Destroy(asteroid);

        if (asteroidDestroyPS != null)
        {
            ParticleSystem ps = Instantiate(asteroidDestroyPS, collisionPoint, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration);
        }

        if (asteroidDestroySound != null)
        {
            AudioSource.PlayClipAtPoint(asteroidDestroySound, Camera.main.transform.position);
        }
    }
}
