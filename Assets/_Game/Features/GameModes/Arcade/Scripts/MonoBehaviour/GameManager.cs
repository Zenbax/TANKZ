// Add this logic to GameManager.cs to remove all projectiles at the end of a round

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _Game.Features.GameModes.Arcade.Scripts.Tank.MonoBehaviour;
using _Game.Features.GameModes.Test.Scripts.Tank.MonoBehaviour;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    public MapGenerator mapGenerator;
    public CameraController cameraController;
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public PowerUpManager powerUpManager; // Ensure reference is assigned in inspector

    [Header("UI")]
    public Slider player1HealthBar;
    public TMP_Text player1HealthText;
    public Slider player2HealthBar;
    public TMP_Text player2HealthText;
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;

    private GameObject player1;
    private GameObject player2;
    private int player1Score = 0;
    private int player2Score = 0;
    private bool waitingToRespawn = false;
    private int size;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        StartNewRound();
    }

    void StartNewRound()
    {
        // Clear old map, tanks, projectiles
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObjects.Clear();

        foreach (var proj in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(proj);
        }

        powerUpManager.ResetSpawning();

        mapGenerator.Generate();
        size = mapGenerator.size;
        cameraController.FitToGround();

        SpawnPlayers();
        powerUpManager.EnableSpawningAfterDelay(10f);
    }

    void SpawnPlayers()
    {
        Vector3 player1Pos = new Vector3(2f, 0.5f, size / 2f);
        Vector3 player2Pos = new Vector3(size - 2f, 0.5f, size / 2f);

        player1 = Instantiate(player1Prefab, player1Pos, Quaternion.identity);
        player2 = Instantiate(player2Prefab, player2Pos, Quaternion.identity);

        player1.GetComponent<Tank>().Init(this, 1);
        player2.GetComponent<Tank>().Init(this, 2);

        spawnedObjects.AddRange(GameObject.FindGameObjectsWithTag("Wall"));
        spawnedObjects.AddRange(GameObject.FindGameObjectsWithTag("DestructibleWall"));
        spawnedObjects.Add(player1);
        spawnedObjects.Add(player2);

        UpdateHealth(1, 100f);
        UpdateHealth(2, 100f);
    }

    public void UpdateHealth(int player, float value)
    {
        if (player == 1)
        {
            player1HealthBar.value = value;
            player1HealthText.text = value.ToString("0");
        }
        else
        {
            player2HealthBar.value = value;
            player2HealthText.text = value.ToString("0");
        }
    }

    public void OnPlayerDeath(int playerNumber)
    {
        if (!waitingToRespawn)
        {
            StartCoroutine(DelayedRespawn());
        }
    }

    IEnumerator DelayedRespawn()
    {
        waitingToRespawn = true;

        yield return new WaitForSeconds(3f);

        bool p1Dead = player1 == null;
        bool p2Dead = player2 == null;

        if (p1Dead && !p2Dead) player2Score++;
        else if (p2Dead && !p1Dead) player1Score++;

        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();

        yield return new WaitForSeconds(0.1f);
        StartNewRound();

        waitingToRespawn = false;
    }
}
