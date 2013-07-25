using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace InControl
{
	public sealed class DeviceProfile : Attribute {}


	public class InputDeviceProfile
	{
		public const int MaxUnityButtons = 20;
		public const int MaxUnityAnalogs = 10;

		protected string name = "Unknown Device";
		protected string meta = "";

		protected float sensitivity = 1.0f;
		protected float deadZone = 0.2f;

		protected InputControlAnalogMapping[] analogMappings;
		protected InputControlButtonMapping[] buttonMappings;
		
		protected List<string> supportedPlatforms;
		protected List<string> joystickNames;


		public InputDeviceProfile()
		{
		}


		// Refactor into new subclass UnknownDeviceProfile
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
				if (supportedPlatforms == null || supportedPlatforms.Count == 0)
				{
					return true;
				}

				foreach (var platform in supportedPlatforms)
				{
					// TODO: refactor out the "*" from all device profiles.
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
			get { return joystickNames != null && joystickNames.Count > 0; } 
		}


		public bool HasJoystickName( string joystickName )
		{
			if (!IsJoystick)
			{
				return false;
			}
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

