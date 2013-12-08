using System;
using UnityEngine;
using InControl;


public class SimpleExample : MonoBehaviour
{
	public GameObject target;


	void Start()
	{
		InputManager.Setup();
		InputManager.UnityInputDeviceManager.AttachDevice( new UnityInputDevice( new FPSProfile() ) );

		Debug.Log( "Ready!" );
	}


	void Update()
	{
		InputManager.Update();

		if (InputManager.ActiveDevice.Action1.IsPressed)
		{
			target.renderer.material.color = Color.green;
		}
		else if (InputManager.ActiveDevice.Action2.IsPressed)
		{
			target.renderer.material.color = Color.red;
		}
		else if (InputManager.ActiveDevice.Action3.IsPressed)
		{
			target.renderer.material.color = Color.blue;
		}
		else if (InputManager.ActiveDevice.Action4.IsPressed)
		{
			target.renderer.material.color = Color.yellow;
		}
		else
		{
			target.renderer.material.color = Color.white;
		}

		target.transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * InputManager.ActiveDevice.Direction.x, Space.World );
		target.transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * InputManager.ActiveDevice.Direction.y, Space.World );
		target.transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * InputManager.ActiveDevice.RightStickX, Space.World );
		target.transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * InputManager.ActiveDevice.RightStickY, Space.World );
	}
}

