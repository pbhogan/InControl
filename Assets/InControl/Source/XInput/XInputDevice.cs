#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


namespace InControl
{
	public class XInputDevice : InputDevice
	{
		public int DeviceIndex { get; private set; }
		GamePadState state;


		public XInputDevice( int deviceIndex )
			: base( "XInput Controller" )
		{
			DeviceIndex = deviceIndex;
			SortOrder = deviceIndex;

			Meta = "XInput Controller #" + deviceIndex;

			AddControl( InputControlType.LeftStickX, "LeftStickX" );
			AddControl( InputControlType.LeftStickY, "LeftStickY" );
			AddControl( InputControlType.RightStickX, "RightStickX" );
			AddControl( InputControlType.RightStickY, "RightStickY" );

			AddControl( InputControlType.LeftTrigger, "LeftTrigger" );
			AddControl( InputControlType.RightTrigger, "RightTrigger" );

			AddControl( InputControlType.DPadUp, "DPadUp" );
			AddControl( InputControlType.DPadDown, "DPadDown" );
			AddControl( InputControlType.DPadLeft, "DPadLeft" );
			AddControl( InputControlType.DPadRight, "DPadRight" );

			AddControl( InputControlType.Action1, "Action1" );
			AddControl( InputControlType.Action2, "Action2" );
			AddControl( InputControlType.Action3, "Action3" );
			AddControl( InputControlType.Action4, "Action4" );

			AddControl( InputControlType.LeftBumper, "LeftBumper" );
			AddControl( InputControlType.RightBumper, "RightBumper" );

			AddControl( InputControlType.LeftStickButton, "LeftStickButton" );
			AddControl( InputControlType.RightStickButton, "RightStickButton" );

			AddControl( InputControlType.Start, "Start" );
			AddControl( InputControlType.Back, "Back" );

			QueryState();
		}


		public override void Update( ulong updateTick, float deltaTime )
		{
			QueryState();

			UpdateWithValue( InputControlType.LeftStickX, state.ThumbSticks.Left.X, updateTick );
			UpdateWithValue( InputControlType.LeftStickY, state.ThumbSticks.Left.Y, updateTick );
			UpdateWithValue( InputControlType.RightStickX, state.ThumbSticks.Right.X, updateTick );
			UpdateWithValue( InputControlType.RightStickY, state.ThumbSticks.Right.Y, updateTick );

			UpdateWithValue( InputControlType.LeftTrigger, state.Triggers.Left, updateTick );
			UpdateWithValue( InputControlType.RightTrigger, state.Triggers.Right, updateTick );

			UpdateWithState( InputControlType.DPadUp, state.DPad.Up == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.DPadDown, state.DPad.Down == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.DPadLeft, state.DPad.Left == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.DPadRight, state.DPad.Right == ButtonState.Pressed, updateTick );

			UpdateWithState( InputControlType.Action1, state.Buttons.A == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.Action2, state.Buttons.B == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.Action3, state.Buttons.X == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.Action4, state.Buttons.Y == ButtonState.Pressed, updateTick );

			UpdateWithState( InputControlType.LeftBumper, state.Buttons.LeftShoulder == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.RightBumper, state.Buttons.RightShoulder == ButtonState.Pressed, updateTick );

			UpdateWithState( InputControlType.LeftStickButton, state.Buttons.LeftStick == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.RightStickButton, state.Buttons.RightStick == ButtonState.Pressed, updateTick );

			UpdateWithState( InputControlType.Start, state.Buttons.Start == ButtonState.Pressed, updateTick );
			UpdateWithState( InputControlType.Back, state.Buttons.Back == ButtonState.Pressed, updateTick );
		}


		public override void Vibrate( float leftMotor, float rightMotor )
		{
			GamePad.SetVibration( (PlayerIndex) DeviceIndex, leftMotor, rightMotor );
		}


		void QueryState()
		{
			state = GamePad.GetState( (PlayerIndex) DeviceIndex, GamePadDeadZone.Circular );
		}


		public bool IsConnected
		{
			get { return state.IsConnected; }
		}
	}
}
#endif
