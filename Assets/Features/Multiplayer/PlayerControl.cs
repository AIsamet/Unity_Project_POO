using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerControl : NetworkBehaviour
{
    [SerializeField]
    private float walkSpeed = 3.5f;

    [SerializeField]
    private float runSpeedOffset = 2.0f;

    [SerializeField]
    private float strafeSpeed = 3.0f;

    [SerializeField]
    private Vector2 defaultInitialPositionOnPlane = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

    private CharacterController characterController;

    // client caches positions
    private Vector3 oldInputPosition = Vector3.zero;
    private PlayerState oldPlayerState = PlayerState.Idle;

    private Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y), 0,
                   Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y));
        }
    }

    void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }

        ClientMove();
        ClientVisuals();
    }

    private void ClientMove()
    {
        if (networkPositionDirection.Value != Vector3.zero)
        {
            characterController.SimpleMove(networkPositionDirection.Value);
        }
    }

    private void ClientVisuals()
    {
        if (oldPlayerState != networkPlayerState.Value)
        {
            oldPlayerState = networkPlayerState.Value;
            animator.SetTrigger($"{networkPlayerState.Value}");
        }
    }

    private void ClientInput()
    {
        // left & right strafing
        float strafeInput = Input.GetAxis("Horizontal");
        float forwardInput = Input.GetAxis("Vertical");

        // Calculate movement in the horizontal and vertical directions separately
        Vector3 horizontalMovement = transform.right * strafeInput * strafeSpeed;
        Vector3 verticalMovement = transform.forward * forwardInput * walkSpeed;

        // Combine the horizontal and vertical movement vectors to get the final movement vector
        Vector3 inputPosition = horizontalMovement + verticalMovement;

        // Update the player state based on the magnitude of the movement vector
        if (inputPosition.magnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                inputPosition *= runSpeedOffset;
                UpdatePlayerStateServerRpc(PlayerState.Run);
            }
            else
            {
                UpdatePlayerStateServerRpc(PlayerState.Walk);
            }
        }
        else
        {
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        }

        // Let the server know about client position changes
        if (oldInputPosition != inputPosition)
        {
            oldInputPosition = inputPosition;
            UpdateClientPositionServerRpc(inputPosition);
        }
    }



    [ServerRpc]
    public void UpdateClientPositionServerRpc(Vector3 newPosition)
    {
        networkPositionDirection.Value = newPosition;
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }
}