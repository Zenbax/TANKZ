using UnityEngine;

namespace _Game.Features.GameModes.Arcade.Scripts.Tank.MonoBehaviour
{
    public class MapGenerator : UnityEngine.MonoBehaviour
    {
        [Header("Map Size")]
        public int minSize = 20;
        public int maxSize = 28;

        [HideInInspector] public int size;

        public GameObject wallPrefab;
        public GameObject destructibleWallPrefab;
        public GameObject groundPlane;

        [Range(0f, 1f)] public float destructibleChance = 0.15f;
        public int spawnBuffer = 3;

        public void Generate()
        {
            size = Random.Range(minSize, maxSize + 1);
            SetupGroundPlane();

            PowerUpManager powerUpManager = FindObjectOfType<PowerUpManager>();

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    Vector3 pos = new Vector3(x + 0.5f, 0f, z + 0.5f);

                    // Outer border
                    if (x == 0 || x == size - 1 || z == 0 || z == size - 1)
                    {
                        Instantiate(wallPrefab, pos, Quaternion.identity);
                        continue;
                    }

                    // Skip spawn corridors
                    if (x < spawnBuffer || x >= size - spawnBuffer)
                        continue;

                    // Less dense wall logic: small wall clusters
                    bool isWallCluster =
                        (x % 5 == 2 && z % 5 == 2) ||
                        (x % 6 == 3 && z % 4 == 1) ||
                        (x % 7 == 0 && z % 3 == 0 && Random.value > 0.5f);

                    if (isWallCluster)
                    {
                        Instantiate(wallPrefab, pos, Quaternion.identity);
                        continue;
                    }

                    // Destructibles
                    if (Random.value < destructibleChance)
                    {
                        GameObject go = Instantiate(destructibleWallPrefab, pos, Quaternion.identity);
                        DestructibleWall wall = go.GetComponent<DestructibleWall>();
                        powerUpManager?.Register(wall);
                    }
                }
            }
        }

        private void SetupGroundPlane()
        {
            if (groundPlane == null) return;

            groundPlane.transform.localScale = new Vector3(size / 10f, 1f, size / 10f);
            groundPlane.transform.position = new Vector3(size / 2f, 0f, size / 2f);
        }
    }
}