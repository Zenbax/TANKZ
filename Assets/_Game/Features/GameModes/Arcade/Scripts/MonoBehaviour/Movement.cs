using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private string verticalInput = "Vertical1";
    [SerializeField] private string horizontalInput = "Horizontal1";
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float rotationSpeed = 100.0f;
    [SerializeField] private LayerMask wallLayer; // LayerMask to detect walls
    [SerializeField] private Vector3 boxCastSize = new Vector3(0.4f, 0.5f, 0.4f); // Size of collision box

    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Freeze unwanted physics rotation
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis(verticalInput);
        float rotateInput = Input.GetAxisRaw(horizontalInput);

        // Rotation (raw input ensures no double-press conflict)
        float rotate = rotateInput * rotationSpeed * Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotate, 0f);
        rb.MoveRotation(rb.rotation * turn);

        // Forward movement
        Vector3 direction = transform.forward * moveInput;
        Vector3 movement = direction * speed * Time.deltaTime;
        Vector3 targetPosition = rb.position + movement;

        // Check for wall collision before moving
        if (!Physics.CheckBox(targetPosition, boxCastSize, Quaternion.identity, wallLayer))
        {
            rb.MovePosition(targetPosition);
        }
        else
        {
            Debug.Log("[Movement] Blocked by wall.");
        }
    }

    // Debug gizmo in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 futurePos = Application.isPlaying ? rb.position + transform.forward * speed * Time.deltaTime : transform.position;
        Gizmos.DrawWireCube(futurePos, boxCastSize);
    }
}