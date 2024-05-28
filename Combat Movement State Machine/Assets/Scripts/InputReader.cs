using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
	private Controls controls;

	public Vector2 MovementValue {  get; private set; }

	public event Action JumpEvent;      // Space [Keyboard]			Button South [Gamepad]
	public event Action DodgeEvent;     // Left-Alt [Keyboard]		Right-Shoulder [Gamepad]
	public event Action TargetEvent;    // Left-Shift [Keyboard]	Left-Trigger [Gamepad]
	public event Action CancelEvent;    // Esc [Keyboard]				Left-Shoulder [Gamepad]
	public bool isAttacking { get; private set; }

	void Start()
	{
		controls = new Controls();
		controls.Player.SetCallbacks(this);

		controls.Player.Enable();
	}

	private void OnDestroy()
	{
		controls.Player.Disable();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (!context.performed) {  return; }
		JumpEvent?.Invoke();
	}

	public void OnDodge(InputAction.CallbackContext context)
	{
		if(!context.performed) { return; }
		DodgeEvent?.Invoke();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		MovementValue = context.ReadValue<Vector2>();
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		
	}

	public void OnTarget(InputAction.CallbackContext context)
	{
		if(!context.performed) { return; }
		TargetEvent?.Invoke();
	}

	public void OnCancel(InputAction.CallbackContext context)
	{
		if(!context.performed) { return; }
		CancelEvent?.Invoke();
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		if(context.performed) 
		{
			isAttacking = true;
		}
		else if(context.canceled)
		{
			isAttacking = false;
		}
	}
}
