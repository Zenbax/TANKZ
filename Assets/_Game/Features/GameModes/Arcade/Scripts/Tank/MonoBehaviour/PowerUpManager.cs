using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    public float spawnInterval = 10f;

    private List<DestructibleWall> allDestructibleWalls = new List<DestructibleWall>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomPowerUp();
        }
    }

    void SpawnRandomPowerUp()
    {
        // Clean up nulls in case some walls were destroyed
        allDestructibleWalls.RemoveAll(wall => wall == null);

        if (allDestructibleWalls.Count == 0) return;

        // Pick random wall
        var wall = allDestructibleWalls[Random.Range(0, allDestructibleWalls.Count)];

        // Pick random power-up (excluding Standard)
        TurretType randomPowerUp = (TurretType)Random.Range(1, 4); // 1=RapidFire, 2=Sniper, 3=Explosive

        wall.SetPowerUp(randomPowerUp);
    }

    public void Register(DestructibleWall wall)
    {
        if (!allDestructibleWalls.Contains(wall))
            allDestructibleWalls.Add(wall);
    }
}