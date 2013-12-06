using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace InControl
{
	public sealed class AutoDiscover : Attribute
	{
	}


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


		protected InputControlSource Button0  = new UnityButtonInputControlSource( 0 );
		protected InputControlSource Button1  = new UnityButtonInputControlSource( 1 );
		protected InputControlSource Button2  = new UnityButtonInputControlSource( 2 );
		protected InputControlSource Button3  = new UnityButtonInputControlSource( 3 );
		protected InputControlSource Button4  = new UnityButtonInputControlSource( 4 );
		protected InputControlSource Button5  = new UnityButtonInputControlSource( 5 );
		protected InputControlSource Button6  = new UnityButtonInputControlSource( 6 );
		protected InputControlSource Button7  = new UnityButtonInputControlSource( 7 );
		protected InputControlSource Button8  = new UnityButtonInputControlSource( 8 );
		protected InputControlSource Button9  = new UnityButtonInputControlSource( 9 );
		protected InputControlSource Button10 = new UnityButtonInputControlSource( 10 );
		protected InputControlSource Button11 = new UnityButtonInputControlSource( 11 );
		protected InputControlSource Button12 = new UnityButtonInputControlSource( 12 );
		protected InputControlSource Button13 = new UnityButtonInputControlSource( 13 );
		protected InputControlSource Button14 = new UnityButtonInputControlSource( 14 );
		protected InputControlSource Button15 = new UnityButtonInputControlSource( 15 );
		protected InputControlSource Button16 = new UnityButtonInputControlSource( 16 );
		protected InputControlSource Button17 = new UnityButtonInputControlSource( 17 );
		protected InputControlSource Button18 = new UnityButtonInputControlSource( 18 );
		protected InputControlSource Button19 = new UnityButtonInputControlSource( 19 );

		protected InputControlSource Analog0  = new UnityAnalogInputControlSource( 0 );
		protected InputControlSource Analog1  = new UnityAnalogInputControlSource( 1 );
		protected InputControlSource Analog2  = new UnityAnalogInputControlSource( 2 );
		protected InputControlSource Analog3  = new UnityAnalogInputControlSource( 3 );
		protected InputControlSource Analog4  = new UnityAnalogInputControlSource( 4 );
		protected InputControlSource Analog5  = new UnityAnalogInputControlSource( 5 );
		protected InputControlSource Analog6  = new UnityAnalogInputControlSource( 6 );
		protected InputControlSource Analog7  = new UnityAnalogInputControlSource( 7 );
		protected InputControlSource Analog8  = new UnityAnalogInputControlSource( 8 );
		protected InputControlSource Analog9  = new UnityAnalogInputControlSource( 9 );

		protected InputControlSource KeyCodeButton( KeyCode keyCode )
		{
			return new UnityKeyCodeButtonInputControlSource( keyCode );
		}

		protected InputControlSource KeyCodeAxis( KeyCode negativeKeyCode, KeyCode positiveKeyCode )
		{
			return new UnityKeyCodeAxisInputControlSource( negativeKeyCode, positiveKeyCode );
		}
	}
}

