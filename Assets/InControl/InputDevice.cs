using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class InputDevice
	{
		public static readonly InputDevice Null = new InputDevice( "NullInputDevice" );

		public string Name { get; protected set; }
		public string Meta { get; protected set; }

		public UnityInputDeviceProfile Profile { get; protected set; } // TODO: Move into UnityInputDevice
		public InputControl[] Analogs { get; protected set; } // TODO: Unify Analogs and Buttons. Use polymorphism.
		public InputControl[] Buttons { get; protected set; }

		public float UpdateTime { get; protected set; }

		InputControl[] controlTable;
		int filledAnalogCount = 0;
		int filledButtonCount = 0;

		float lastUpdateTime;


		public InputDevice( string name, int analogCount = 0, int buttonCount = 0 )
		{
			Name = name;
			Meta = "";

			Analogs = new InputControl[ analogCount ];
			Buttons = new InputControl[ buttonCount ];

			UpdateTime = 0.0f;
			lastUpdateTime = 0.0f;

			var numInputControlTypes = (int) InputControlType.Count + 1;
			controlTable = new InputControl[ numInputControlTypes ];
		}


		public InputControl GetControl( Enum inputControlType )
		{
			int controlIndex = Convert.ToInt32( inputControlType );
			var control = controlTable[controlIndex];
			return control == null ? InputControl.Null : control;
		}


		public void AddAnalogControl( Enum target, string handle = "" )
		{
			SetAnalogControl( filledAnalogCount++, target, handle );
		}


		public void SetAnalogControl( int i, Enum target, string handle = "" )
		{
			Analogs[i] = new InputControl( handle, target.ToString() );
			var controlIndex = Convert.ToInt32( target );
			controlTable[controlIndex] = Analogs[i];
		}


		public void AddButtonControl( Enum target, string handle = "" )
		{
			SetButtonControl( filledButtonCount++, target, handle );
		}


		public void SetButtonControl( int i, Enum target, string handle = "" )
		{
			Buttons[i] = new InputControl( handle, target.ToString() );
			var controlIndex = Convert.ToInt32( target );
			controlTable[controlIndex] = Buttons[i];
		}


		public void Update( float updateTime )
		{
			if (Profile == null)
			{
				return;
			}

			float deltaTime = updateTime - lastUpdateTime;

			// TODO: Move Unity specific stuff here into UnityInputDevice		

			var analogMappingCount = Profile.AnalogMappings.Length;
			for (int i = 0; i < analogMappingCount; i++)
			{
				var analogMapping = Profile.AnalogMappings[i];
				var unityValue = GetAnalogValue( analogMapping.Source );

				if (analogMapping.TargetRangeIsNotComplete && unityValue == 0.0f && Analogs[i].UpdateTime == 0.0f)
				{
					// Ignore initial input stream for triggers, because they report 
					// zero incorrectly until the value changes for the first time.
					// Example: wired Xbox controller on Mac.
					continue;
				}

				var smoothValue = SmoothAnalogValue( unityValue, Analogs[i].LastValue, deltaTime );
				var mappedValue = analogMapping.MapValue( smoothValue );
				Analogs[i].UpdateWithValue( mappedValue, updateTime );
			}

			var buttonMappingCount = Profile.ButtonMappings.Length;
			for (int i = 0; i < buttonMappingCount; i++)
			{
				var buttonSource = Profile.ButtonMappings[i].Source;
				Buttons[i].UpdateWithState( GetButtonState( buttonSource ), updateTime );
			}

			lastUpdateTime = updateTime;
		}


		// TODO: This belongs in UnityInputDevice
		protected virtual float GetAnalogValue( string source )
		{
			return 0.0f;
		}


		// TODO: This belongs in UnityInputDevice
		protected virtual bool GetButtonState( string source )
		{
			return false;
		}


		public void AdvanceUpdateTime( float updateTime )
		{
			bool stateChanged = false;

			foreach (var analog in Analogs)
			{
				stateChanged = stateChanged || analog.HasChanged;
			}

			foreach (var button in Buttons)
			{
				stateChanged = stateChanged || button.HasChanged;
			}

			if (stateChanged)
			{
				UpdateTime = updateTime;
			}
		}

		
		float SmoothAnalogValue( float thisValue, float lastValue, float deltaTime )
		{
			if (Profile.IsJoystick)
			{
				// Apply dead zone.
				thisValue = Mathf.InverseLerp( Profile.DeadZone, 1.0f, Mathf.Abs( thisValue ) ) * Mathf.Sign( thisValue );

				// Apply sensitivity (how quickly the value adapts to changes).
				float maxDelta = deltaTime * Profile.Sensitivity * 100.0f;
				
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
