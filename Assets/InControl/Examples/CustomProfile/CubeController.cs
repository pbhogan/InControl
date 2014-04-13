using System;
using UnityEngine;
using InControl;


namespace CustomProfileExample
{
	public class CubeController : MonoBehaviour
	{
		Vector3 targetPosition;


		void Start()
		{
			// Get the starting position of the object.
			targetPosition = transform.position;
		}


		void Update()
		{
			// Use last device which provided input.
			var inputDevice = InputManager.ActiveDevice;

			// Set target object material color based on which action is pressed.
			if (inputDevice.Action1) {
				renderer.material.color = Color.green;
			} else
			if (inputDevice.Action2) {
				renderer.material.color = Color.red;
			} else
			if (inputDevice.Action3) {
				renderer.material.color = Color.blue;
			} else
			if (inputDevice.Action4) {
				renderer.material.color = Color.yellow;
			} else {
				renderer.material.color = Color.white;
			}

			// Rotate target object with both sticks and d-pad.
			transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.Direction.X, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.Direction.Y, Space.World );
			transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.RightStickX, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.RightStickY, Space.World );

			// Zoom target object with scroll wheel.
			var z = inputDevice.GetControl( InputControlType.ScrollWheel );
			targetPosition.z = Mathf.Clamp( targetPosition.z + z, -10.0f, 25.0f );
			transform.position = Vector3.MoveTowards( transform.position, targetPosition, Time.deltaTime * 25.0f );

			// Only supported on Windows with XInput and Xbox 360 controllers.
			InputManager.ActiveDevice.Vibrate( inputDevice.LeftTrigger, inputDevice.RightTrigger );
		}
	}
}

