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
        // Handle bouncing
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Calculate the reflection vector
            Vector3 incomingVelocity = rb.linearVelocity;
            Vector3 normal = collision.contacts[0].normal; // Get the collision normal
            Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, normal);

            // Apply the reflected velocity while maintaining the speed
            rb.linearVelocity = reflectedVelocity.normalized * speed;
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