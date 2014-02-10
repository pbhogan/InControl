using System;
using UnityEngine;
using InControl;


public class SimpleExample : MonoBehaviour
{
	public GameObject target;
	Vector3 targetPosition;


	void Start()
	{
		InputManager.EnableXInput = true;
		InputManager.Setup();

		// Add a custom device profile.
		InputManager.AttachDevice( new UnityInputDevice( new FPSProfile() ) );

		// Get the starting position of the object.
		targetPosition = target.transform.position;

		Debug.Log( "InControl (version " + InputManager.Version + ")" );
	}


	void Update()
	{
		InputManager.Update();

		// Use last device which provided input.
		var inputDevice = InputManager.ActiveDevice;

		// Set target object material color based on which action is pressed.
		if (inputDevice.Action1)
		{
			target.renderer.material.color = Color.green;
		}
		else 
		if (inputDevice.Action2)
		{
			target.renderer.material.color = Color.red;
		}
		else 
		if (inputDevice.Action3)
		{
			target.renderer.material.color = Color.blue;
		}
		else 
		if (inputDevice.Action4)
		{
			target.renderer.material.color = Color.yellow;
		}
		else
		{
			target.renderer.material.color = Color.white;
		}

		// Rotate target object with both sticks and d-pad.
		target.transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.Direction.x, Space.World );
		target.transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.Direction.y, Space.World );
		target.transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.RightStickX, Space.World );
		target.transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.RightStickY, Space.World );

		// Zoom target object with scroll wheel.
		var z = inputDevice.GetControl( InputControlType.ScrollWheel );
		targetPosition.z = Mathf.Clamp( targetPosition.z + z, -10.0f, 25.0f );
		target.transform.position = Vector3.MoveTowards( target.transform.position, targetPosition, Time.deltaTime * 25.0f );

		// Only supported on Windows with XInput and Xbox 360 controllers.
		InputManager.ActiveDevice.Vibrate( inputDevice.LeftTrigger, inputDevice.RightTrigger );
	}
}

