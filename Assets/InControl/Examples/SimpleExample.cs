using System;
using UnityEngine;
using InControl;


public class SimpleExample : MonoBehaviour
{
	public GameObject target;
	Vector3 targetPosition;


	void Start()
	{
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

		var z = InputManager.ActiveDevice.GetControl( InputControlType.ScrollWheel );
		targetPosition.z = Mathf.Clamp( targetPosition.z + z, -10.0f, 25.0f );
		target.transform.position = Vector3.MoveTowards( target.transform.position, targetPosition, Time.deltaTime * 25.0f );
	}
}

