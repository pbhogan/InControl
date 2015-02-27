using System;
using UnityEngine;
using InControl;


namespace MultiplayerExample
{
	public class CubeController : MonoBehaviour
	{
		public int playerNum;
		Renderer cubeRenderer;


		void Start()
		{
			cubeRenderer = GetComponent<Renderer>();
		}


		void Update()
		{
			var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;
			if (inputDevice == null)
			{
				// If no controller exists for this cube, just make it translucent.
				cubeRenderer.material.color = new Color( 1.0f, 1.0f, 1.0f, 0.2f );
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
				cubeRenderer.material.color = Color.green;
			}
			else
			if (inputDevice.Action2)
			{
				cubeRenderer.material.color = Color.red;
			}
			else
			if (inputDevice.Action3)
			{
				cubeRenderer.material.color = Color.blue;
			}
			else
			if (inputDevice.Action4)
			{
				cubeRenderer.material.color = Color.yellow;
			}
			else
			{
				cubeRenderer.material.color = Color.white;
			}
			
			// Rotate target object with both sticks and d-pad.
			transform.Rotate( Vector3.down, 500.0f * Time.deltaTime * inputDevice.Direction.X, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.Direction.Y, Space.World );
			transform.Rotate( Vector3.down, 500.0f * Time.deltaTime * inputDevice.RightStickX, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.RightStickY, Space.World );
		}
	}
}

