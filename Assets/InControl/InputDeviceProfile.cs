using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TinyJSON;
using UnityEngine;


namespace InControl
{
	public class InputDeviceProfile
	{
		public const int MaxUnityButtons = 20;
		public const int MaxUnityAnalogs = 10;

		string name = "Unknown";
		string meta = "";

		float sensitivity = 1.0f;
		float deadZone = 0.2f;

		InputControlAnalogMapping[] analogMappings;
		InputControlButtonMapping[] buttonMappings;
		
		List<string> supportedPlatforms = new List<string>();
		List<string> joystickNames = new List<string>();


		public InputDeviceProfile()
		{
		}


		public InputDeviceProfile( string joystickName )
		{
			if (joystickName != "")
			{
				name += " (" + joystickName + ")";
			}

			supportedPlatforms = new List<string>() { "*" };
			joystickNames = new List<string>() { joystickName };

			analogMappings = new InputControlAnalogMapping[ MaxUnityAnalogs ];
			for (int i = 0; i < MaxUnityAnalogs; i++)
			{
				analogMappings[i] = new InputControlAnalogMapping( i );
			}

			buttonMappings = new InputControlButtonMapping[ MaxUnityButtons ];
			for (int i = 0; i < MaxUnityButtons; i++)
			{
				buttonMappings[i] = new InputControlButtonMapping( i );
			}
		}


		public bool IsSupportedOnThisPlatform
		{
			get 
			{
				foreach (var platform in supportedPlatforms)
				{
					if (platform == "*" || InputManager.Platform.Contains( platform.ToUpper() ))
					{
						return true;
					}
				}
				return false;
			}
		}


		public bool IsJoystick 
		{ 
			get { return joystickNames.Count > 0; } 
		}


		public bool HasJoystickName( string joystickName )
		{
			return joystickNames.Contains( joystickName, StringComparer.OrdinalIgnoreCase );
		}


		public string Name
		{
			get { return name; }
		}


		public string Meta
		{
			get { return meta; }
		}


		public float Sensitivity
		{
			get { return sensitivity; }
		}


		public float DeadZone
		{
			get { return deadZone; }
		}


		public InputControlAnalogMapping[] AnalogMappings
		{
			get { return analogMappings; }
		}


		public InputControlButtonMapping[] ButtonMappings
		{
			get { return buttonMappings; }
		}
	}
}

