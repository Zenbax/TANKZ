using _Game.Features.GameModes.Arcade.Scripts.Tank.MonoBehaviour;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public CameraController cameraController;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    void Start()
    {
        mapGenerator.Generate();         // Generates map and sets .size
        cameraController.FitToGround();  // Camera now uses mapGenerator reference
        SpawnPlayers();                  // Players use updated size
    }

    void SpawnPlayers()
    {
        int size = mapGenerator.size;

        // Left and right side spawns
        Vector3 player1Pos = new Vector3(1f, 0.5f, size / 2f);
        Vector3 player2Pos = new Vector3(size - 2f, 0.5f, size / 2f);

        Instantiate(player1Prefab, player1Pos, Quaternion.identity);
        Instantiate(player2Prefab, player2Pos, Quaternion.identity);
    }
}