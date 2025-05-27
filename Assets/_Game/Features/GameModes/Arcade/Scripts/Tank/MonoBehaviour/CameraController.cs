using _Game.Features.GameModes.Arcade.Scripts.Tank.MonoBehaviour;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public MapGenerator mapGenerator;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void FitToGround()
    {
        if (mapGenerator == null || mapGenerator.groundPlane == null)
        {
            Debug.LogWarning("MapGenerator or GroundPlane not assigned.");
            return;
        }

        Transform groundPlane = mapGenerator.groundPlane.transform;

        float size = mapGenerator.size;
        float aspectRatio = (float)Screen.width / Screen.height;

        float verticalSize = size / 2f;
        float horizontalSize = (size / aspectRatio) / 2f;

        cam.orthographicSize = Mathf.Max(verticalSize, horizontalSize);

        transform.position = new Vector3(size / 2f, transform.position.y, size / 2f);
    }
}