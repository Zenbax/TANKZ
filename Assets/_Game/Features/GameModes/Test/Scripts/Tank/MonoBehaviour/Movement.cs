using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
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
        // Move forward and backward
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 movement = transform.forward * move;
        rb.MovePosition(rb.position + movement);

        // Rotate left and right
        float rotate = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotate, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }
}