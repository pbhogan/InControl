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

		TestInputMappings();
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
				var label = string.Format( "{0} ({1}) = {2}", analog.Target, analog.Handle, analog.Value );
				GUI.Label( new Rect( x, y, x + w, y + 10 ), label, style );
				y += lineHeight;
			}

			foreach (var button in inputDevice.Buttons)
			{
				SetColor( button.State ? Color.green : color );
				var label = string.Format( "{0} ({1}) = {2}", button.Target, button.Handle, button.State ? "True" : "" );
				GUI.Label( new Rect( x, y, x + w, y + 10 ), label, style );
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


	void TestInputMappings()
	{
		var complete = InputControlMapping.Range.Complete;
		var positive = InputControlMapping.Range.Positive;
		var negative = InputControlMapping.Range.Negative;
		var noInvert = false;
		var doInvert = true;

		TestInputMapping( complete, complete, noInvert, -1.0f, 0.0f, 1.0f );
		TestInputMapping( complete, negative, noInvert, -1.0f, -0.5f, 0.0f );
		TestInputMapping( complete, positive, noInvert, 0.0f, 0.5f, 1.0f );

		TestInputMapping( negative, complete, noInvert, -1.0f, 1.0f, 0.0f );
		TestInputMapping( negative, negative, noInvert, -1.0f, 0.0f, 0.0f );
		TestInputMapping( negative, positive, noInvert, 0.0f, 1.0f, 0.0f );

		TestInputMapping( positive, complete, noInvert, 0.0f, -1.0f, 1.0f );
		TestInputMapping( positive, negative, noInvert, 0.0f, -1.0f, 0.0f );
		TestInputMapping( positive, positive, noInvert, 0.0f, 0.0f, 1.0f );

		TestInputMapping( complete, complete, doInvert, 1.0f, 0.0f, -1.0f );
		TestInputMapping( complete, negative, doInvert, 1.0f, 0.5f, 0.0f );
		TestInputMapping( complete, positive, doInvert, 0.0f, -0.5f, -1.0f );

		TestInputMapping( negative, complete, doInvert, 1.0f, -1.0f, 0.0f );
		TestInputMapping( negative, negative, doInvert, 1.0f, 0.0f, 0.0f );
		TestInputMapping( negative, positive, doInvert, 0.0f, -1.0f, 0.0f );

		TestInputMapping( positive, complete, doInvert, 0.0f, 1.0f, -1.0f );
		TestInputMapping( positive, negative, doInvert, 0.0f, 1.0f, 0.0f );
		TestInputMapping( positive, positive, doInvert, 0.0f, 0.0f, -1.0f );
	}


	void TestInputMapping( InputControlMapping.Range sourceRange, InputControlMapping.Range targetRange, bool invert, float expectA, float expectB, float expectC )
	{
		var mapping = new InputControlMapping() {
			SourceRange = sourceRange,
			TargetRange = targetRange,
			Invert      = invert
		};

		float value;

		string sr = "Complete";
		if (sourceRange == InputControlMapping.Range.Negative)
			sr = "Negative";
		else
		if (sourceRange == InputControlMapping.Range.Positive)
			sr = "Positive";

		string tr = "Complete";
		if (targetRange == InputControlMapping.Range.Negative)
			tr = "Negative";
		else
		if (targetRange == InputControlMapping.Range.Positive)
			tr = "Positive";

		value = mapping.MapValue( -1.0f );
		if (Mathf.Abs( value - expectA ) > Single.Epsilon)
		{
			Debug.LogError( "Got unexpected value A " + value + " instead of " + expectA + " (SR = " + sr + ", TR = " + tr + ")" );
		}

		value = mapping.MapValue( 0.0f );
		if (Mathf.Abs( value - expectB ) > Single.Epsilon)
		{
			Debug.LogError( "Got unexpected value B " + value + " instead of " + expectB + " (SR = " + sr + ", TR = " + tr + ")" );
		}

		value = mapping.MapValue( 1.0f );
		if (Mathf.Abs( value - expectC ) > Single.Epsilon)
		{
			Debug.LogError( "Got unexpected value C " + value + " instead of " + expectC + " (SR = " + sr + ", TR = " + tr + ")" );
		}
	}
}
