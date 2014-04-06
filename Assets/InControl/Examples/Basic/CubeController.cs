using System;
using UnityEngine;
using InControl;


namespace BasicExample
{
	public class CubeController : MonoBehaviour
	{
		void Update()
		{
			// Use last device which provided input.
			var inputDevice = InputManager.ActiveDevice;

			// Rotate target object with left stick.
			transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.LeftStickX, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.LeftStickY, Space.World );
		}
	}
}

