using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform target; // The tank's transform
    [SerializeField] private Vector3 offset; // Offset from the target

    // Update is called once per frame
    void LateUpdate()
    {
        // Update the camera's position based on the target's position and the offset
        transform.position = target.position + offset;
    }
}