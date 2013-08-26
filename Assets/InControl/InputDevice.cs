using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace InControl
{
	public class InputDevice
	{
		public static readonly InputDevice Null = new InputDevice( "NullInputDevice" );

		public string Name { get; protected set; }
		public string Meta { get; protected set; }

		public InputControl[] Analogs { get; protected set; } // TODO: Unify Analogs and Buttons. Use polymorphism.
		public InputControl[] Buttons { get; protected set; }

		public float LastChangeTime { get; protected set; }

		InputControl[] controlTable;
		int filledAnalogCount = 0;
		int filledButtonCount = 0;


		public InputDevice( string name, int analogCount = 0, int buttonCount = 0 )
		{
			Name = name;
			Meta = "";

			Analogs = new InputControl[ analogCount ];
			Buttons = new InputControl[ buttonCount ];

			LastChangeTime = 0.0f;

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


		public virtual void Update( float updateTime, float deltaTime )
		{
		}


		public void UpdateLastChangeTime( float updateTime )
		{
			if (Analogs.Any( analog => analog.HasChanged ) ||
				Buttons.Any( button => button.HasChanged ))
			{
				LastChangeTime = updateTime;
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
