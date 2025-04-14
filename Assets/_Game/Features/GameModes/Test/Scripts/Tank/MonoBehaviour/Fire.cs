using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour
{
    [Header("Turret Settings")]
    public TurretType turretType; // Enum-based turret selection
    public Transform firingPoint;
    public float fireRate = 0.5f; // Fire delay between shots

    [Header("Projectile Settings")]
    public ProjectileData projectileData; // ScriptableObject storing projectile info
    public int poolSize = 20; // Number of pooled projectiles

    private Queue<GameObject> projectilePool = new Queue<GameObject>();
    private Dictionary<GameObject, IProjectile> projectileScripts = new Dictionary<GameObject, IProjectile>();
    private float lastShotTime = 0f;

    void Start()
    {
        InitializePool();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastShotTime + fireRate)
        {
            Shoot();
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectileData.prefab);
            projectile.SetActive(false);
            projectilePool.Enqueue(projectile);
            projectileScripts[projectile] = projectile.GetComponent<IProjectile>();
        }
    }

    private void Shoot()
    {
        if (projectilePool.Count == 0) return;

        lastShotTime = Time.time;
        GameObject projectile = projectilePool.Dequeue();
        projectile.transform.position = firingPoint.position;
        projectile.transform.rotation = firingPoint.rotation;
        projectile.SetActive(true);

        IProjectile projectileScript = projectileScripts[projectile];
        projectileScript.Launch(projectileData, firingPoint.forward, new Collider() { });

        // Ignore collision with the tank that fired the projectile
        Collider tankCollider = GetComponent<Collider>();
        Collider projectileCollider = projectile.GetComponent<Collider>();
        Physics.IgnoreCollision(projectileCollider, tankCollider, true);

        // Start coroutine to re-enable collision after exiting the tank's collider
        StartCoroutine(ReenableCollision(projectileCollider, tankCollider));

        StartCoroutine(ReturnToPool(projectile, projectileData.lifetime));
    }

    private IEnumerator ReenableCollision(Collider projectileCollider, Collider tankCollider)
    {
        // Wait until the projectile exits the tank's collider
        yield return new WaitUntil(() => !projectileCollider.bounds.Intersects(tankCollider.bounds));
        Physics.IgnoreCollision(projectileCollider, tankCollider, false);
    }

    private IEnumerator ReturnToPool(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (projectile.activeSelf)
        {
            projectile.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
    }
}