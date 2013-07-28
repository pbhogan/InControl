using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InControl;


public class TestInputManager : MonoBehaviour
{
	GUIStyle style = new GUIStyle();
	List<LogMessage> logMessages = new List<LogMessage>();


	void Start()
	{
		InputManager.OnDeviceAttached += inputDevice => Debug.Log( "Attached: " + inputDevice.Name );
		InputManager.OnDeviceDetached += inputDevice => Debug.Log( "Detached: " + inputDevice.Name );
		InputManager.OnActiveDeviceChanged += inputDevice => Debug.Log( "Active device changed to: " + inputDevice.Name );

		Logger.OnLogMessage += logMessage => logMessages.Add( logMessage );

		InputManager.Setup();
	}


	void FixedUpdate()
	{
		InputManager.Update();

		if (InputManager.ActiveDevice.GetControl( InputControlType.Action1 ).WasPressed)
		{
			Debug.Log( "JUMP!" );
		}

		if (InputManager.ActiveDevice.Action3.WasPressed)
		{
			Debug.Log( "SHOOT!" );
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
		var lineHeight = 15;

		SetColor( Color.white );

		string info = "Devices:";
		info += " (Platform: " + InputManager.Platform + ")";
//		info += " (Joysticks " + InputManager.JoystickHash + ")";
		info += " " + InputManager.ActiveDevice.Direction;

		GUI.Label( new Rect( x, y, x + w, y + 10 ), info, style );

		foreach (var inputDevice in InputManager.Devices)
		{
			bool active = InputManager.ActiveDevice == inputDevice;
			Color color = active ? Color.yellow : Color.white;

			y = 35;

			SetColor( color );

			GUI.Label( new Rect( x, y, x + w, y + 10 ), inputDevice.Name, style );
			y += lineHeight;

			GUI.Label( new Rect( x, y, x + w, y + 10 ), "UpdateTime: " + inputDevice.UpdateTime, style );
			y += lineHeight;

			foreach (var analog in inputDevice.Analogs)
			{
				SetColor( analog.State ? Color.green : color );
				GUI.Label( new Rect( x, y, x + w, y + 10 ), analog.Handle + ": " + analog.Value, style );
				y += lineHeight;
			}

			foreach (var button in inputDevice.Buttons)
			{
				SetColor( button.State ? Color.green : color );
				GUI.Label( new Rect( x, y, x + w, y + 10 ), button.Handle + ": " + (button.State ? "True" : ""), style );
				y += lineHeight;
			}

			x += 200;
		}


		Color[] logColors = { Color.gray, Color.yellow, Color.white };
		SetColor( Color.white );
		x = 10;
		y = Screen.height - (10 + lineHeight);
		for (int i = logMessages.Count - 1; i >= 0; i--)
		{
			var logMessage = logMessages[i];
			SetColor( logColors[(int)logMessage.type] );
			foreach (var line in logMessage.text.Split('\n'))
			{
	        	GUI.Label( new Rect( x, y, Screen.width, y + 10 ), line, style );
				y -= lineHeight;
			}
		}
	}
}
