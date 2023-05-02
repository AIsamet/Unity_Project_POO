using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	private PlayerInput playerInput;
	private PlayerInput.OnFootActions onFoot;

	private PlayerMotor playerMotor;
	private PlayerLook playerLook;

	private Gun gun;

	void Awake()
	{
		playerInput = new PlayerInput();
		onFoot = playerInput.OnFoot;

		playerMotor = GetComponent<PlayerMotor>();
		playerLook = GetComponent<PlayerLook>();

		onFoot.Jump.performed += ctx => playerMotor.Jump(); // performed started and canceled are the three events that can be used
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		// tell the motor to move the player using the value from movement action
		playerMotor.ProcessMovement(onFoot.Movement.ReadValue<Vector2>());
	}

	void LateUpdate()
	{
		// tell the look to look using the value from look action
		playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
	}

	private void OnEnable()
	{
		onFoot.Enable();
	}

	private void OnDisable()
	{
		onFoot.Disable();
	}
}
