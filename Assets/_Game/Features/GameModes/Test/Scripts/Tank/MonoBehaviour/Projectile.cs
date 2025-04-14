using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour, IProjectile
{
    private Rigidbody rb;
    private float damage;
    private float speed;
    private bool explosive;
    private Collider shooterCollider;
    private bool canDamageShooter = false;

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

        rb.isKinematic = false;
        rb.linearVelocity = direction * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Handle bouncing (PhysicsMaterial handles the bounce itself)
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Ensure the projectile maintains its speed after bouncing
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
            return;
        }

        // Handle damage logic
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Allow damage to both the shooter and enemies
            if (collision.collider == shooterCollider && canDamageShooter || collision.collider != shooterCollider)
            {
                damageable.TakeDamage(damage);
            }
        }

        if (explosive)
        {
            Debug.Log("Boooooom! Explosive projectile hit: " + collision.gameObject.name);
            // Handle explosion effect here
        }

        // Deactivate the projectile after collision
        gameObject.SetActive(false);
    }
}