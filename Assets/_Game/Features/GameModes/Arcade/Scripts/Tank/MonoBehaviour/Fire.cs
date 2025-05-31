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
        projectileScript.Launch(projectileData, firingPoint.forward, GetComponent<Collider>());

        // Removed IgnoreCollision entirely
        StartCoroutine(ReturnToPool(projectile, projectileData.lifetime));
    }

    private IEnumerator ReenableSelfCollision(Collider proj, Collider tank, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (proj != null && tank != null)
        {
            Physics.IgnoreCollision(proj, tank, false);
        }
    }

    private IEnumerator ReturnToPool(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (projectile.activeSelf)
        {
            projectile.SetActive(false);
        }
        projectilePool.Enqueue(projectile);
    }

    // Optional: Apply power-up method
    public void ApplyPowerUp(TurretType newType)
    {
        turretType = newType;

        switch (newType)
        {
            case TurretType.Standard:
                projectileData.damage = 50f;
                projectileData.speed = 10f;
                projectileData.explosive = false;
                break;
            case TurretType.RapidFire:
                projectileData.damage = 25f;
                projectileData.speed = 14f;
                projectileData.explosive = false;
                break;
            case TurretType.Sniper:
                projectileData.damage = 100f;
                projectileData.speed = 20f;
                projectileData.explosive = false;
                break;
            case TurretType.Explosive:
                projectileData.damage = 50f;
                projectileData.speed = 10f;
                projectileData.explosive = true;
                break;
        }

        // Reset after 10 seconds
        StartCoroutine(RevertToStandard());
    }

    private IEnumerator RevertToStandard()
    {
        yield return new WaitForSeconds(10f);
        ApplyPowerUp(TurretType.Standard);
    }
}