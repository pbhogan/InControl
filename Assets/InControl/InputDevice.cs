using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class InputDevice
	{
		public string Name { get; protected set; }
		public string Meta { get; protected set; }

		public InputDeviceProfile Profile { get; protected set; }
		public InputControl[] Analogs { get; protected set; }
		public InputControl[] Buttons { get; protected set; }

		public float UpdateTime { get; protected set; }

		protected InputControl[] controlTable;
		static InputControl nullControl = new InputControl( "N/A" ); // TODO: Create a NullInputControl?

		public static readonly InputDevice Null = new InputDevice();


		public InputDevice()
		{
			Name = "";
			Meta = "";
		}


		public InputDevice( InputDeviceProfile deviceProfile )
		{
			Profile = deviceProfile;

			Name = deviceProfile.Name;
			Meta = deviceProfile.Meta;

			controlTable = new InputControl[ InputManager.NumInputControlTypes ];

			var analogMappingCount = Profile.AnalogMappings.Length;
			Analogs = new InputControl[ analogMappingCount ];
			for (int i = 0; i < analogMappingCount; i++)
			{
				Analogs[i] = new InputControl( Profile.AnalogMappings[i].Handle );
				var targetControl = (int) Profile.AnalogMappings[i].Target;
				controlTable[targetControl] = Analogs[i];
			}

			var buttonMappingCount = Profile.ButtonMappings.Length;
			Buttons = new InputControl[ buttonMappingCount ];
			for (int i = 0; i < buttonMappingCount; i++)
			{
				Buttons[i] = new InputControl( Profile.ButtonMappings[i].Handle );
				var targetControl = (int) Profile.ButtonMappings[i].Target;
				controlTable[targetControl] = Buttons[i];
			}
		}


		public InputControl GetControl( Enum inputAnalog )
		{
			if (controlTable == null)
			{
				return nullControl;
			}
			int controlIndex = Convert.ToInt32( inputAnalog );
			var control = controlTable[controlIndex];
			return control == null ? nullControl : control;
		}


		public void Update( float updateTime )
		{
			if (Profile == null)
			{
				return;
			}

			bool stateChanged = false;
		
			var analogMappingCount = Profile.AnalogMappings.Length;
			for (int i = 0; i < analogMappingCount; i++)
			{
				var analogMapping = Profile.AnalogMappings[i];
				var unityValue = GetAnalogValue( analogMapping.Source );

				if (analogMapping.HasPositiveTargetRange && unityValue == 0.0f && Analogs[i].UpdateTime == 0.0f)
				{
					// Ignore initial input stream for triggers, because they report 
					// zero incorrectly until the value changes for the first time.
					// Example: wired Xbox controller on Mac.
					continue;
				}
				
				var smoothValue = SmoothAnalogValue( unityValue, Analogs[i].LastValue );
				var mappedValue = analogMapping.MapValue( smoothValue );
				Analogs[i].UpdateWithValue( mappedValue, updateTime );
				stateChanged = stateChanged || Analogs[i].HasChanged;
			}

			var buttonMappingCount = Profile.ButtonMappings.Length;
			for (int i = 0; i < buttonMappingCount; i++)
			{
				var buttonSource = Profile.ButtonMappings[i].Source;
				Buttons[i].UpdateWithState( GetButtonState( buttonSource ), updateTime );
				stateChanged = stateChanged || Buttons[i].HasChanged;
			}

			if (stateChanged)
			{
				UpdateTime = updateTime;
			}
		}


		protected virtual float GetAnalogValue( string source )
		{
			return 0.0f;
		}


		protected virtual bool GetButtonState( string source )
		{
			return false;
		}

		
		float SmoothAnalogValue( float thisValue, float lastValue )
		{
			if (Profile.IsJoystick)
			{
				// Apply dead zone.
				thisValue = Mathf.InverseLerp( Profile.DeadZone, 1.0f, Mathf.Abs( thisValue ) ) * Mathf.Sign( thisValue );

				// Apply sensitivity (how quickly the value adapts to changes).
				float maxDelta = Time.deltaTime * Profile.Sensitivity * 100.0f;
				
				// Move faster towards zero when changing direction.
				if (Mathf.Sign( lastValue ) != Mathf.Sign( thisValue )) 
				{
				   maxDelta *= 2;
				}
				
				return Mathf.Clamp( lastValue + Mathf.Clamp( thisValue - lastValue, -maxDelta, maxDelta ), -1.0f, 1.0f );
			}
			else
			{
				return thisValue;
			}
		}


		public InputControl LeftStickX { get { return GetControl( InputControlType.LeftStickX ); } }
		public InputControl LeftStickY { get { return GetControl( InputControlType.LeftStickY ); } }
		public InputControl LeftStickButton { get { return GetControl( InputControlType.LeftStickButton ); } }

		public InputControl RightStickX { get { return GetControl( InputControlType.RightStickX ); } }
		public InputControl RightStickY { get { return GetControl( InputControlType.RightStickY ); } }
		public InputControl RightStickButton { get { return GetControl( InputControlType.RightStickButton ); } }

		public InputControl DPadUp { get { return GetControl( InputControlType.DPadUp ); } }
		public InputControl DPadDown { get { return GetControl( InputControlType.DPadDown ); } }
		public InputControl DPadLeft { get { return GetControl( InputControlType.DPadLeft ); } }
		public InputControl DPadRight { get { return GetControl( InputControlType.DPadRight ); } }
				
		public InputControl Action1 { get { return GetControl( InputControlType.Action1 ); } }
		public InputControl Action2 { get { return GetControl( InputControlType.Action2 ); } }
		public InputControl Action3 { get { return GetControl( InputControlType.Action3 ); } }
		public InputControl Action4 { get { return GetControl( InputControlType.Action4 ); } }

		public InputControl LeftTrigger { get { return GetControl( InputControlType.LeftTrigger ); } }
		public InputControl RightTrigger { get { return GetControl( InputControlType.RightTrigger ); } }

		public InputControl LeftBumper { get { return GetControl( InputControlType.LeftBumper ); } }
		public InputControl RightBumper { get { return GetControl( InputControlType.RightBumper ); } }


		public Vector2 LeftStickVector
		{
			get
			{
				return new Vector2( LeftStickX.Value, LeftStickY.Value ).normalized;
			}
		}


		public Vector2 RightStickVector
		{
			get
			{
				return new Vector2( RightStickX.Value, RightStickY.Value ).normalized;
			}
		}


		public Vector2 DPadVector
		{
			get
			{
				var l = DPadLeft;
				var r = DPadRight;
				var u = DPadUp;
				var d = DPadDown;

				var x = l.State ? -l.Value : r.Value;
				var y = u.State ? u.Value : -d.Value;

				return new Vector2( x, InputManager.InvertYAxis ? -y : y ).normalized;
			}
		}


		public Vector2 Direction
		{
			get
			{
				var dpv = DPadVector;
				return dpv == Vector2.zero ? LeftStickVector : dpv;
			}
		}
	}
}
