using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    public float spawnInterval = 10f;
    private List<DestructibleWall> allDestructibleWalls = new List<DestructibleWall>();
    private bool allowSpawn = false;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (allowSpawn)
                SpawnRandomPowerUp();
        }
    }

    public void EnableSpawningAfterDelay(float delay)
    {
        StartCoroutine(EnableSpawning(delay));
    }

    IEnumerator EnableSpawning(float delay)
    {
        yield return new WaitForSeconds(delay);
        allowSpawn = true;
    }

    public void ResetSpawning()
    {
        allowSpawn = false;
        foreach (var wall in allDestructibleWalls)
        {
            if (wall != null)
                wall.SetPowerUp(TurretType.Standard);
        }
    }

    void SpawnRandomPowerUp()
    {
        allDestructibleWalls.RemoveAll(wall => wall == null);
        if (allDestructibleWalls.Count == 0) return;
        var wall = allDestructibleWalls[Random.Range(0, allDestructibleWalls.Count)];
        TurretType randomPowerUp = (TurretType)Random.Range(1, 4);
        wall.SetPowerUp(randomPowerUp);
    }

    public void Register(DestructibleWall wall)
    {
        if (!allDestructibleWalls.Contains(wall))
            allDestructibleWalls.Add(wall);
    }
}