using System;
using UnityEngine;
using InControl;


public class SimpleExample : MonoBehaviour
{
	void Start()
	{
		InputManager.Setup();

		Debug.Log( "Ready!" );
	}


	void Update()
	{
		InputManager.Update();

		if (InputManager.ActiveDevice.Action1.WasPressed)
		{
			Debug.Log( "BOOM!" );
		}
	}
}

