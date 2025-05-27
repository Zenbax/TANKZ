using UnityEngine;

namespace _Game.Features.GameModes.Arcade.Scripts.Tank.MonoBehaviour
{
    public class MapGenerator : UnityEngine.MonoBehaviour
    {
        [Header("Map Size")]
        public int minSize = 16;
        public int maxSize = 32;

        [HideInInspector] public int size;

        public GameObject wallPrefab;
        public GameObject destructibleWallPrefab;
        public GameObject groundPlane;

        [Range(0f, 1f)] public float destructibleChance = 0.2f;
        public int spawnBuffer = 3;

        public void Generate()
        {
            size = Random.Range(minSize, maxSize + 1);
            SetupGroundPlane();

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    Vector3 pos = new Vector3(x, 0f, z);

                    if (x == 0 || x == size - 1 || z == 0 || z == size - 1)
                    {
                        Instantiate(wallPrefab, pos, Quaternion.identity);
                        continue;
                    }

                    if (x < spawnBuffer || x >= size - spawnBuffer)
                        continue;

                    if ((x % 6 == 2 && z > 2 && z < size - 3) ||
                        (z % 6 == 3 && x > 2 && x < size - 3))
                    {
                        Instantiate(wallPrefab, pos, Quaternion.identity);
                        continue;
                    }

                    if (Random.value < destructibleChance)
                    {
                        Instantiate(destructibleWallPrefab, pos, Quaternion.identity);
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