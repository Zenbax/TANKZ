using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Fire))]
public class PlayerInputHandler : MonoBehaviour
{
    private TankControls controls;
    private Fire fireComponent;

    public int playerNumber = 1;

    private Vector2 moveInput;
    private bool firePressed;

    private void Awake()
    {
        Debug.Log($"[Awake] Player {playerNumber} InputHandler initialized on: {gameObject.name}");
        controls = new TankControls();
        fireComponent = GetComponent<Fire>();
    }

    private void OnEnable()
    {
        controls.Enable();

        Debug.Log("[OnEnable] Setting up input for player: " + playerNumber);

        if (playerNumber == 1)
        {
            controls.Player.Fire1.performed += ctx => Fire();
        }
        else
        {
            controls.Player.Fire2.performed += ctx => Fire();
        }
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Fire()
    {
        if (Time.time > fireComponent.GetLastShotTime() + fireComponent.GetFireRate())
        {
            Debug.Log($"[Fire] Player {playerNumber} fired!");
            fireComponent.Shoot();
        }
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
}