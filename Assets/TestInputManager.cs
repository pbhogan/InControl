using System;
using System.Collections;
using UnityEngine;
using InControl;


public class TestInputManager : MonoBehaviour
{
	GUIStyle style = new GUIStyle();


	void Start()
	{
		InputManager.Setup();

		InputManager.OnDeviceAttached += inputDevice => Debug.Log( "Attached: " + inputDevice.Name );
		InputManager.OnDeviceDetached += inputDevice => Debug.Log( "Detached: " + inputDevice.Name );
		InputManager.OnActiveDeviceChanged += inputDevice => Debug.Log( "Active device changed to: " + inputDevice.Name );
	}


	void FixedUpdate()
	{
		InputManager.Update();

		// bool jump = InputManager.ActiveDevice.GetControl( InputControlType.Action1 );

		if (InputManager.Devices[0].GetControl( InputControlType.Action1 ).WasPressed)
		{
			Debug.Log( "JUMP!" );
		}
	}


	void SetColor( Color color )
	{
		style.normal.textColor = color;
	}


	void OnGUI()
	{
		var w = 300;
		var x = 10;
		var y = 10;

		SetColor( Color.white );

		GUI.Label( new Rect( x, y, x + w, y + 10 ), "Devices:", style );
		y += 15;

		foreach (var inputDevice in InputManager.Devices)
		{
			bool active = InputManager.ActiveDevice == inputDevice;
			Color color = active ? Color.yellow : Color.white;

			y = 25;

			SetColor( color );

			GUI.Label( new Rect( x, y, x + w, y + 10 ), inputDevice.Name, style );
			y += 15;

			GUI.Label( new Rect( x, y, x + w, y + 10 ), "UpdateTime: " + inputDevice.UpdateTime, style );
			y += 15;

			foreach (var analog in inputDevice.Analogs)
			{
				SetColor( analog.State ? Color.green : color );
				GUI.Label( new Rect( x, y, x + w, y + 10 ), analog.Handle + ": " + analog.Value, style );
				y += 15;
			}

			foreach (var button in inputDevice.Buttons)
			{
				SetColor( button.State ? Color.green : color );
				GUI.Label( new Rect( x, y, x + w, y + 10 ), button.Handle + ": " + (button.State ? "True" : ""), style );
				y += 15;
			}

			x += 300;
		}
	}
}
