using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace InControl
{
	public sealed class AutoDiscover : Attribute {}


	public class InputDeviceProfile
	{
		public string Name { get; protected set; }
		public string Meta { get; protected set; }

		public float Sensitivity { get; protected set; }
		public float DeadZone { get; protected set; }

		public InputControlAnalogMapping[] AnalogMappings { get; protected set; }
		public InputControlButtonMapping[] ButtonMappings { get; protected set; }
		
		protected List<string> SupportedPlatforms; // TODO: Refactor to array
		protected List<string> JoystickNames; // TODO: Refactor to array


		public InputDeviceProfile()
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
				if (SupportedPlatforms == null || SupportedPlatforms.Count == 0)
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
			get { return JoystickNames != null && JoystickNames.Count > 0; } 
		}


		public bool HasJoystickName( string joystickName )
		{
			if (!IsJoystick)
			{
				return false;
			}
			return JoystickNames.Contains( joystickName, StringComparer.OrdinalIgnoreCase );
		}
	}
}

