using System;
using UnityEngine;


namespace InControl
{
	public class InputDevice
	{
		public static readonly InputDevice Null = new InputDevice( "NullInputDevice" );

		internal int SortOrder = int.MaxValue;

		public string Name { get; protected set; }
		public string Meta { get; protected set; }

		public ulong LastChangeTick { get; protected set; }

		public InputControl[] Controls { get; protected set; }

		public TwoAxisInputControl LeftStick { get; protected set; }
		public TwoAxisInputControl RightStick { get; protected set; }
		public TwoAxisInputControl DPad { get; protected set; }


		public InputDevice( string name )
		{
			Name = name;
			Meta = "";

			LastChangeTick = 0;

			const int numInputControlTypes = (int) InputControlType.Count + 1;
			Controls = new InputControl[numInputControlTypes];

			LeftStick = new TwoAxisInputControl();
			RightStick = new TwoAxisInputControl();
			DPad = new TwoAxisInputControl();
		}


		public InputControl GetControl( InputControlType inputControlType )
		{
			var control = Controls[ (int) inputControlType ];
			return control ?? InputControl.Null;
		}


		// Warning: this is not efficient. Don't use it unless you have to, m'kay?
		public static InputControlType GetInputControlTypeByName( string inputControlName )
		{
			return (InputControlType) Enum.Parse( typeof(InputControlType), inputControlName );
		}


		// Warning: this is not efficient. Don't use it unless you have to, m'kay?
		public InputControl GetControlByName( string inputControlName )
		{
			var inputControlType = GetInputControlTypeByName( inputControlName );
			return GetControl( inputControlType );
		}


		public InputControl AddControl( InputControlType inputControlType, string handle )
		{
			var inputControl = new InputControl( handle, inputControlType );
			Controls[ (int) inputControlType ] = inputControl;
			return inputControl;
		}


		public void UpdateWithState( InputControlType inputControlType, bool state, ulong updateTick )
		{
			GetControl( inputControlType ).UpdateWithState( state, updateTick );
		}


		public void UpdateWithValue( InputControlType inputControlType, float value, ulong updateTick )
		{
			GetControl( inputControlType ).UpdateWithValue( value, updateTick );
		}


		public void PreUpdate( ulong updateTick, float deltaTime )
		{
			int controlCount = Controls.GetLength( 0 );
			for (int i = 0; i < controlCount; i++)
			{
				var control = Controls[i];
				if (control != null)
				{
					control.PreUpdate( updateTick );
				}
			}
		}


		public virtual void Update( ulong updateTick, float deltaTime )
		{
			// Implemented by subclasses.
		}


		public void PostUpdate( ulong updateTick, float deltaTime )
		{
			// Apply post-processing to controls.
			int controlCount = Controls.GetLength( 0 );
			for (int i = 0; i < controlCount; i++)
			{
				var control = Controls[i];
				if (control != null)
				{
					if (control.RawValue.HasValue)
					{
						control.UpdateWithValue( control.RawValue.Value, updateTick );
					}
					else
					if (control.PreValue.HasValue)
					{
						control.UpdateWithValue( ProcessAnalogControlValue( control, deltaTime ), updateTick );
					}

					control.PostUpdate( updateTick );

					if (control.HasChanged)
					{
						LastChangeTick = updateTick;
					}
				}
			}

			// Update two-axis controls.
			LeftStick.Update( LeftStickX, LeftStickY, updateTick );
			RightStick.Update( RightStickX, RightStickY, updateTick );

			var dpv = DPadVector;
			DPad.Update( dpv.x, dpv.y, updateTick );
		}


		float ProcessAnalogControlValue( InputControl control, float deltaTime )
		{
			var analogValue = control.PreValue.Value;

			var obverseTarget = control.Obverse;
			if (obverseTarget.HasValue)
			{
				var obverseControl = GetControl( obverseTarget.Value );
				if (obverseControl.PreValue.HasValue)
				{
					analogValue = ApplyCircularDeadZone( analogValue, obverseControl.PreValue.Value, control.LowerDeadZone, control.UpperDeadZone );
				}
				else
				{
					analogValue = ApplyDeadZone( analogValue, control.LowerDeadZone, control.UpperDeadZone );
				}
			}
			else
			{
				analogValue = ApplyDeadZone( analogValue, control.LowerDeadZone, control.UpperDeadZone );
			}

			return ApplySmoothing( analogValue, control.LastValue, deltaTime, control.Sensitivity );
		}


		float ApplyDeadZone( float value, float lowerDeadZone, float upperDeadZone )
		{
			return Mathf.InverseLerp( lowerDeadZone, upperDeadZone, Mathf.Abs( value ) ) * Mathf.Sign( value );
		}


		float ApplyCircularDeadZone( float axisValue1, float axisValue2, float lowerDeadZone, float upperDeadZone )
		{
			var axisVector = new Vector2( axisValue1, axisValue2 );
			var magnitude = Mathf.InverseLerp( lowerDeadZone, upperDeadZone, axisVector.magnitude );
			return (axisVector.normalized * magnitude).x;
		}


		float ApplySmoothing( float thisValue, float lastValue, float deltaTime, float sensitivity )
		{
			// 1.0f and above is instant (no smoothing).
			if (Mathf.Approximately( sensitivity, 1.0f ))
			{
				return thisValue;
			}

			// Apply sensitivity (how quickly the value adapts to changes).
			var maxDelta = deltaTime * sensitivity * 100.0f;

			// Snap to zero when changing direction quickly.
			if (Mathf.Sign( lastValue ) != Mathf.Sign( thisValue ))
			{
				lastValue = 0.0f;
			}

			return Mathf.MoveTowards( lastValue, thisValue, maxDelta );
		}


		Vector2 DPadVector
		{
			get 
			{
				var x = DPadLeft.State ? -DPadLeft.Value : DPadRight.Value;
				var t = DPadUp.State ? DPadUp.Value : -DPadDown.Value;
				var y = InputManager.InvertYAxis ? -t : t;
				return new Vector2( x, y ).normalized;
			}
		}
		
		
		public bool LastChangedAfter( InputDevice otherDevice )
		{
			return LastChangeTick > otherDevice.LastChangeTick;
		}


		public virtual void Vibrate( float leftMotor, float rightMotor )
		{
		}


		public void Vibrate( float intensity )
		{
			Vibrate( intensity, intensity );
		}


		public virtual bool IsSupportedOnThisPlatform
		{
			get { return true; }
		}


		public virtual bool IsKnown
		{
			get { return true; }
		}


		public bool MenuWasPressed
		{
			get
			{
				return GetControl( InputControlType.Back ).WasPressed ||
					GetControl( InputControlType.Start ).WasPressed ||
					GetControl( InputControlType.Select ).WasPressed ||
					GetControl( InputControlType.System ).WasPressed ||
					GetControl( InputControlType.Pause ).WasPressed ||
					GetControl( InputControlType.Menu ).WasPressed;
			}
		}


		public InputControl AnyButton
		{
			get
			{
				int controlCount = Controls.GetLength( 0 );
				for (int i = 0; i < controlCount; i++)
				{
					var control = Controls[i];
					if (control != null && control.IsButton && control.IsPressed)
					{
						return control;
					}
				}

				return InputControl.Null;
			}
		}


		public InputControl LeftStickX { get { return GetControl( InputControlType.LeftStickX ); } }
		public InputControl LeftStickY { get { return GetControl( InputControlType.LeftStickY ); } }

		public InputControl RightStickX { get { return GetControl( InputControlType.RightStickX ); } }
		public InputControl RightStickY { get { return GetControl( InputControlType.RightStickY ); } }

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

		public InputControl LeftStickButton { get { return GetControl( InputControlType.LeftStickButton ); } }
		public InputControl RightStickButton { get { return GetControl( InputControlType.RightStickButton ); } }


		public float DPadX
		{
			get 
			{ 
				return DPad.X; 
			}
		}


		public float DPadY
		{
			get 
			{ 
				return DPad.Y; 
			}
		}


		public TwoAxisInputControl Direction
		{
			get 
			{ 
				return DPad.UpdateTick > LeftStick.UpdateTick ? DPad : LeftStick; 
			}
		}
	}
}

