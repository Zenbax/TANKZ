using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private string verticalInput = "Vertical1";
    [SerializeField] private string horizontalInput = "Horizontal1";
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float rotationSpeed = 100.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("hello tankzzz");
    }

    void Update()
    {
        // Move forward and backward    // Move forward and backward (still smooth)
        float move = Input.GetAxis(verticalInput) * speed * Time.deltaTime;
        Vector3 movement = transform.forward * move;
        rb.MovePosition(rb.position + movement);

        // Rotate left/right — raw input cancels out if both keys are pressed
        float rotate = Input.GetAxisRaw(horizontalInput) * rotationSpeed * Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotate, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }
}