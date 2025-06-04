using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    private Rigidbody rb;
    private float damage;
    private float speed;
    private bool explosive;
    private Collider shooterCollider;
    private Fire shooterFire;
    private bool ignoredFirstSelfHit = false;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 2f;

    [Header("VFX")]
    public GameObject explosionVFX;

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
        ignoredFirstSelfHit = false;

        rb.isKinematic = false;
        rb.linearVelocity = direction * speed;
        rb.angularVelocity = Vector3.zero;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        transform.localScale = Vector3.one * data.projectileScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;

        if (collision.collider == shooterCollider && !ignoredFirstSelfHit)
        {
            ignoredFirstSelfHit = true;
            return;
        }

        if (collision.collider == shooterCollider)
        {
            shooterCollider.GetComponent<IDamageable>()?.TakeDamage(damage);
            Deactivate();
            return;
        }

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
        }

        if (explosive)
        {
            Explode();
        }
        else
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            if (target.CompareTag("Wall"))
            {
                ContactPoint contact = collision.contacts[0];
                Vector3 normal = contact.normal;
                if (normal != Vector3.zero)
                {
                    Vector3 reflected = Vector3.Reflect(rb.linearVelocity, normal);
                    rb.linearVelocity = reflected.normalized * speed;
                }
            }
            else
            {
                Deactivate();
            }
        }
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit == shooterCollider && !ignoredFirstSelfHit)
            {
                ignoredFirstSelfHit = true;
                continue;
            }

            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }

        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }

        Deactivate();
    }

    private void Deactivate()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}