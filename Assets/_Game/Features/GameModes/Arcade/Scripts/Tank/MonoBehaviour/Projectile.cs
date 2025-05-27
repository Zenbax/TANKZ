using UnityEngine;

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
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        Debug.Log("HIT2");
        if (damageable != null)
        {
            Debug.Log("HIT3");
            if ((collision.collider == shooterCollider && canDamageShooter) ||
                collision.collider != shooterCollider)
            {
                Debug.Log("HIT3");
                damageable.TakeDamage(damage);
            }
        }

        if (collision.collider == shooterCollider)
        {
            rb.isKinematic = true;
            return;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 incomingVelocity = rb.linearVelocity;
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, normal);
            rb.linearVelocity = reflectedVelocity.normalized * speed;
            return;
        }

        if (explosive)
        {
            Debug.Log("BOOM! Explosive projectile hit: " + collision.gameObject.name);
            // TODO: Add explosion logic
        }

        gameObject.SetActive(false);
    }
}