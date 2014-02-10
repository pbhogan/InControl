using System;
using UnityEngine;


namespace InControl
{
	public class InputDevice
	{
		public static readonly InputDevice Null = new InputDevice( "NullInputDevice" );

		public int SortOrder = int.MaxValue;

		public string Name { get; protected set; }
		public string Meta { get; protected set; }

		public InputControl[] Analogs { get; protected set; }
		public InputControl[] Buttons { get; protected set; }

		public float LastChangeTime { get; protected set; }

		InputControl[] controlTable;
		int filledAnalogCount;
		int filledButtonCount;


		public InputDevice( string name, int analogCount, int buttonCount )
		{
			Initialize( name, analogCount, buttonCount );
		}


		public InputDevice( string name )
		{
			Initialize( name, 0, 0 );
		}


		void Initialize( string name, int analogCount, int buttonCount )
		{
			Name = name;
			Meta = "";

			Analogs = new InputControl[analogCount];
			Buttons = new InputControl[buttonCount];

			LastChangeTime = 0.0f;

			const int numInputControlTypes = (int) InputControlType.Count + 1;
			controlTable = new InputControl[numInputControlTypes];
		}


		public InputControl GetControl( Enum inputControlType )
		{
			int controlIndex = Convert.ToInt32( inputControlType );
			var control = controlTable[controlIndex];
			return control ?? InputControl.Null;
		}


		public void AddAnalogControl( Enum target, string handle )
		{
			SetAnalogControl( filledAnalogCount++, target, handle );
		}


		public void AddAnalogControl( Enum target )
		{
			SetAnalogControl( filledAnalogCount++, target, target.ToString() );
		}


		public void SetAnalogControl( int i, Enum target, string handle )
		{
			Analogs[i] = new InputControl( handle, target.ToString() );
			var controlIndex = Convert.ToInt32( target );
			controlTable[controlIndex] = Analogs[i];
		}


		public void AddButtonControl( Enum target, string handle )
		{
			SetButtonControl( filledButtonCount++, target, handle );
		}


		public void AddButtonControl( Enum target )
		{
			SetButtonControl( filledButtonCount++, target, target.ToString() );
		}


		public void SetButtonControl( int i, Enum target, string handle )
		{
			Buttons[i] = new InputControl( handle, target.ToString() );
			var controlIndex = Convert.ToInt32( target );
			controlTable[controlIndex] = Buttons[i];
		}


		public virtual void Update( float updateTime, float deltaTime )
		{
		}


		public virtual void Vibrate( float leftMotor, float rightMotor )
		{
		}


		public void Vibrate( float intensity )
		{
			Vibrate( intensity, intensity );
		}


		public void UpdateLastChangeTime( float updateTime )
		{
			int analogCount = Analogs.GetLength( 0 );
			for (int i = 0; i < analogCount; i++)
			{
				if (Analogs[i].HasChanged)
				{
					LastChangeTime = updateTime;
					return;
				}
			}

			int buttonCount = Buttons.GetLength( 0 );
			for (int i = 0; i < buttonCount; i++)
			{
				if (Buttons[i].HasChanged)
				{
					LastChangeTime = updateTime;
					return;
				}
			}
		}


		public bool LastChangedAfter( InputDevice otherDevice )
		{
			return LastChangeTime > otherDevice.LastChangeTime;
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
				return new Vector2( LeftStickX.Value, LeftStickY.Value );
			}
		}


		public Vector2 RightStickVector
		{
			get
			{
				return new Vector2( RightStickX.Value, RightStickY.Value );
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
