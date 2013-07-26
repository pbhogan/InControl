using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace InControl
{
	public sealed class AutoDiscover : Attribute {}


	public class UnityInputDeviceProfile
	{
		public string Name { get; protected set; }
		public string Meta { get; protected set; }

		public float Sensitivity { get; protected set; }
		public float DeadZone { get; protected set; }

		public InputControlAnalogMapping[] AnalogMappings { get; protected set; }
		public InputControlButtonMapping[] ButtonMappings { get; protected set; }
		
		protected string[] SupportedPlatforms;
		protected string[] JoystickNames;


		public UnityInputDeviceProfile()
		{
			Name = "";
			Meta = "";
			Sensitivity = 1.0f;
			DeadZone = 0.2f;
		}


		public bool IsSupportedOnThisPlatform
		{
			get 
			{
				if (SupportedPlatforms == null || SupportedPlatforms.Length == 0)
				{
					return true;
				}

				foreach (var platform in SupportedPlatforms)
				{
					if (InputManager.Platform.Contains( platform.ToUpper() ))
					{
						return true;
					}
				}

				return false;
			}
		}


		public bool IsJoystick 
		{ 
			get { return JoystickNames != null && JoystickNames.Length > 0; } 
		}


		public bool IsNotJoystick
		{ 
			get { return JoystickNames == null || JoystickNames.Length == 0; } 
		}


		public bool HasJoystickName( string joystickName )
		{
			if (IsNotJoystick)
			{
				return false;
			}

			return JoystickNames.Contains( joystickName, StringComparer.OrdinalIgnoreCase );
		}
	}
}

