using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerControlAuthorative : NetworkBehaviour
{
    [SerializeField]
    private float walkSpeed = 3.5f;

    [SerializeField]
    private float runSpeedOffset = 2.0f;

    [SerializeField]
    private float lateralSpeed = 3.0f;

    [SerializeField]
    private float jumpSpeed = 8.0f;

    [SerializeField]
    private float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    [SerializeField]
    private Vector2 defaultInitialPositionOnPlane = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

    private CharacterController characterController;

    private Animator animator;

    // client caches animation states
    private PlayerState oldPlayerState = PlayerState.Idle;

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
            PlayerCameraFollow.Instance.FollowPlayer(transform.Find("PlayerCameraRoot"));
        }
    }

    void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }
        ClientVisuals();
    }

    private void Jump()
    {
        if (characterController.isGrounded)
        {
            // saut
            moveDirection.y = jumpSpeed;
            UpdatePlayerStateServerRpc(PlayerState.Jump);
        }
        else
        {
            // si le joueur n'est pas au sol, le saut n'est pas possible
            return;
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
        // y axis client rotation
        Vector3 inputRotation = Vector3.zero;

        // horizontal direction
        Vector3 horizontalDirection = transform.TransformDirection(Vector3.right);
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 horizontalMovement = horizontalDirection * horizontalInput;

        // forward & backward direction
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 forwardMovement = direction * forwardInput;

        //Jump
        bool jumpInput = Input.GetKeyDown(KeyCode.Space);

        // change animation states
        if (forwardInput == 0)
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        if(horizontalInput != 0 && forwardInput == 0)
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        else if (forwardInput > 0 && forwardInput <= 1 && !ActiveRunningActionKey())
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        else if (forwardInput > 0 && ActiveRunningActionKey())
        {
            forwardMovement = direction * (walkSpeed + runSpeedOffset);
            UpdatePlayerStateServerRpc(PlayerState.Run);
        }
        else if (forwardInput < 0)
            UpdatePlayerStateServerRpc(PlayerState.ReverseWalk);
        if (jumpInput == true)
        {
            UpdatePlayerStateServerRpc(PlayerState.Jump);
            Jump();
        }

        // client is responsible for moving itself
        // appliquer la gravité
        moveDirection.y -= gravity * Time.deltaTime;

        // mouvement
        Vector3 movement = (horizontalMovement + forwardMovement) * walkSpeed + moveDirection;
        characterController.Move(movement * Time.deltaTime);
    }

    private static bool ActiveRunningActionKey()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }
}