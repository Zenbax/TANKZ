using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    private Rigidbody rb;
    private float damage;
    private float speed;
    private bool explosive;
    private Collider shooterCollider;
    private Fire shooterFire;
    private float spawnTime;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(ProjectileData data, Vector3 direction, Collider shooter)
    {
        damage = data.damage;
        speed = data.speed;
        explosive = data.explosive;
        shooterCollider = shooter;
        shooterFire = shooter.GetComponent<Fire>();
        spawnTime = Time.time;
        hasHit = false;

        rb.isKinematic = false;
        rb.linearVelocity = direction * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Has hit: " + hasHit);
        // if (hasHit) return;
        hasHit = true;

        GameObject target = collision.gameObject;

        // 🚫 Avoid immediate self-hit (first 0.3s after spawn)
        if (collision.collider == shooterCollider && Time.time - spawnTime < 0.3f)
        {
            hasHit = false;
            Debug.Log("Has hit 2: " + hasHit);
            return;
        }

        Debug.Log("collision.collider: " + collision.collider);
        Debug.Log("shooterCollider: " + shooterCollider);
        // ☠️ Self-kill
        if (collision.collider == shooterCollider && Time.time - spawnTime < 0.3f)
        {
            var self = shooterCollider.GetComponent<IDamageable>();
            self?.TakeDamage(damage);
            Deactivate();
            Destroy(shooterCollider.gameObject);
            return;
        }

        // 💥 Deal damage
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);

            // Check for powerup
            if (target.CompareTag("DestructibleWall"))
            {
                foreach (Transform child in target.transform)
                {
                    if (child.gameObject.activeSelf)
                    {
                        if (child.name == "Sniper") shooterFire?.ApplyPowerUp(TurretType.Sniper);
                        if (child.name == "RapidFire") shooterFire?.ApplyPowerUp(TurretType.RapidFire);
                        if (child.name == "Explosive") shooterFire?.ApplyPowerUp(TurretType.Explosive);
                    }
                }

                Deactivate();
                return;
            }
        }

        // Wall bounce
        if (target.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflected = Vector3.Reflect(rb.linearVelocity, normal);
            rb.linearVelocity = reflected.normalized * speed;
        }
        else
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}