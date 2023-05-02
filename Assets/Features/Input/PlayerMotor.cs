using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
	private Vector3 playerVelocity;
	public float speed = 5f;
	private bool isGrounded;
	public float gravity = -9.81f;
	public float jumpheight = 1f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
	{
		controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update()
	{
		isGrounded = controller.isGrounded;
	}

	// Receive the inputs from the InputManager and apply them to the character controller
	public void ProcessMovement(Vector2 input)
	{
		Vector3 movementDirection = Vector3.zero;
		movementDirection.x = input.x;
		movementDirection.z = input.y;

        bool isMoving = movementDirection.magnitude > 0;
        animator.SetBool("isWalking", isMoving);

        controller.Move(transform.TransformDirection(movementDirection) * speed * Time.deltaTime);
		playerVelocity.y += gravity * Time.deltaTime;
		if(isGrounded && playerVelocity.y < 0)
			playerVelocity.y = -2.0f;
        controller.Move(playerVelocity * Time.deltaTime);
		Debug.Log(playerVelocity.y);
    }

	public void Jump()
	{
        if (isGrounded)
		{
			playerVelocity.y = Mathf.Sqrt(jumpheight * -3.0f * gravity);
            animator.SetBool("isJumping", true);
        }
	}
}
