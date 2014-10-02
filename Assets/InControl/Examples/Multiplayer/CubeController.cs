using System;
using UnityEngine;
using InControl;


namespace MultiplayerExample
{
	public class CubeController : MonoBehaviour
	{
		public int playerNum;


		void Update()
		{
			var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;
			if (inputDevice == null)
			{
				// If no controller exists for this cube, just make it translucent.
				renderer.material.color = new Color( 1.0f, 1.0f, 1.0f, 0.2f );
			}
			else
			{
				UpdateCubeWithInputDevice( inputDevice );
			}
		}


		void UpdateCubeWithInputDevice( InputDevice inputDevice )
		{
			// Set object material color based on which action is pressed.
			if (inputDevice.Action1)
			{
				renderer.material.color = Color.green;
			}
			else
			if (inputDevice.Action2)
			{
				renderer.material.color = Color.red;
			}
			else
			if (inputDevice.Action3)
			{
				renderer.material.color = Color.blue;
			}
			else
			if (inputDevice.Action4)
			{
				renderer.material.color = Color.yellow;
			}
			else
			{
				renderer.material.color = Color.white;
			}
			
			// Rotate target object with both sticks and d-pad.
			transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.Direction.X, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.Direction.Y, Space.World );
			transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.RightStickX, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.RightStickY, Space.World );
		}
	}
}

