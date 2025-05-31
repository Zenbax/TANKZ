using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamageable
{
    public float health = 50f;
    public GameObject destructionEffect;

    private GameObject sniper;
    private GameObject rapidFire;
    private GameObject explosive;

    void Awake()
    {
        // Automatically find children by name
        sniper = transform.Find("Sniper")?.gameObject;
        rapidFire = transform.Find("RapidFire")?.gameObject;
        explosive = transform.Find("Explosive")?.gameObject;

        // Deactivate all at start
        sniper?.SetActive(false);
        rapidFire?.SetActive(false);
        explosive?.SetActive(false);
    }

    public void SetPowerUp(TurretType type)
    {
        sniper?.SetActive(false);
        rapidFire?.SetActive(false);
        explosive?.SetActive(false);

        switch (type)
        {
            case TurretType.Sniper:
                sniper?.SetActive(true);
                break;
            case TurretType.RapidFire:
                rapidFire?.SetActive(true);
                break;
            case TurretType.Explosive:
                explosive?.SetActive(true);
                break;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            DestroyWall();
        }
    }

    private void DestroyWall()
    {
        if (destructionEffect != null)
        {
            GameObject effect = Instantiate(destructionEffect, transform.position, Quaternion.identity);

            // Auto-destroy the particle effect after it finishes playing
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                float duration = ps.main.duration + ps.main.startLifetime.constantMax;
                Destroy(effect, duration);
            }
            else
            {
                Destroy(effect, 3f); // fallback: 3 seconds
            }
        }

        Destroy(gameObject);
    }
}