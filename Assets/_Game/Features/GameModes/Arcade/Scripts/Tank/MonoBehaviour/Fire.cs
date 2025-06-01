using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour
{
    [Header("Turret Settings")]
    public TurretType turretType;
    public Transform firingPoint;
    public float fireRate = 0.5f;
    [SerializeField] private string fireInput = "Fire1"; // 🔁 Input axis field

    [Header("Projectile Settings")]
    public ProjectileData projectileData;
    public int poolSize = 20;

    private Queue<GameObject> projectilePool = new Queue<GameObject>();
    private Dictionary<GameObject, IProjectile> projectileScripts = new Dictionary<GameObject, IProjectile>();
    private float lastShotTime = 0f;

    private GameObject sniperVisual;
    private GameObject rapidFireVisual;
    private GameObject explosiveVisual;

    void Start()
    {
        InitializePool();
        SetupVisualIndicators();
    }

    void Update()
    {
        if (Input.GetButtonDown(fireInput) && Time.time > lastShotTime + fireRate)
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

        StartCoroutine(ReturnToPool(projectile, projectileData.lifetime));

        if (turretType == TurretType.Explosive)
        {
            ApplyPowerUp(TurretType.Standard);
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

    public void ApplyPowerUp(TurretType newType)
    {
        turretType = newType;

        switch (newType)
        {
            case TurretType.Standard:
                projectileData.damage = 50f;
                projectileData.speed = 10f;
                projectileData.explosive = false;
                projectileData.projectileScale = 0.2f;
                break;
            case TurretType.RapidFire:
                projectileData.damage = 20f;
                projectileData.speed = 20f;
                projectileData.explosive = false;
                projectileData.projectileScale = 0.1f;
                break;
            case TurretType.Sniper:
                projectileData.damage = 100f;
                projectileData.speed = 5f;
                projectileData.explosive = false;
                projectileData.projectileScale = 0.25f;
                break;
            case TurretType.Explosive:
                projectileData.damage = 99f;
                projectileData.speed = 1f;
                projectileData.explosive = true;
                projectileData.projectileScale = 0.4f;
                break;
        }

        UpdatePowerUpVisuals();

        if (newType != TurretType.Explosive)
            StartCoroutine(RevertToStandard());
    }

    private IEnumerator RevertToStandard()
    {
        yield return new WaitForSeconds(10f);
        ApplyPowerUp(TurretType.Standard);
    }

    private void SetupVisualIndicators()
    {
        sniperVisual = transform.Find("Sniper")?.gameObject;
        rapidFireVisual = transform.Find("RapidFire")?.gameObject;
        explosiveVisual = transform.Find("Explosive")?.gameObject;

        DeactivateAllPowerUpVisuals();
    }

    private void UpdatePowerUpVisuals()
    {
        DeactivateAllPowerUpVisuals();

        switch (turretType)
        {
            case TurretType.Sniper:
                sniperVisual?.SetActive(true);
                break;
            case TurretType.RapidFire:
                rapidFireVisual?.SetActive(true);
                break;
            case TurretType.Explosive:
                explosiveVisual?.SetActive(true);
                break;
        }
    }

    private void DeactivateAllPowerUpVisuals()
    {
        sniperVisual?.SetActive(false);
        rapidFireVisual?.SetActive(false);
        explosiveVisual?.SetActive(false);
    }
}