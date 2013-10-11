using System;
using UnityEngine;
using InControl;


public class SimpleExample : MonoBehaviour
{
	public GameObject target;


	void Start()
	{
		InputManager.Setup();

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

		target.transform.Rotate( Vector3.up,    80.0f * Time.deltaTime * InputManager.ActiveDevice.Direction.x );
		target.transform.Rotate( Vector3.right, 80.0f * Time.deltaTime * InputManager.ActiveDevice.Direction.y );
	}
}

