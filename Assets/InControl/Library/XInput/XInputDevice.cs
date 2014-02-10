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
			: base( "Xbox 360 Controller (XInput)", 6, 14 )
		{
			DeviceIndex = deviceIndex;
			SortOrder   = deviceIndex;

			Meta = "XInput Device #" + deviceIndex;

			AddAnalogControl( InputControlType.LeftStickX );
			AddAnalogControl( InputControlType.LeftStickY );
			AddAnalogControl( InputControlType.RightStickX );
			AddAnalogControl( InputControlType.RightStickY );
			AddAnalogControl( InputControlType.LeftTrigger );
			AddAnalogControl( InputControlType.RightTrigger );
			
			AddButtonControl( InputControlType.DPadUp );
			AddButtonControl( InputControlType.DPadDown );
			AddButtonControl( InputControlType.DPadLeft );
			AddButtonControl( InputControlType.DPadRight );
			AddButtonControl( InputControlType.Action1 );
			AddButtonControl( InputControlType.Action2 );
			AddButtonControl( InputControlType.Action3 );
			AddButtonControl( InputControlType.Action4 );
			AddButtonControl( InputControlType.LeftBumper );
			AddButtonControl( InputControlType.RightBumper );
			AddButtonControl( InputControlType.LeftStickButton );
			AddButtonControl( InputControlType.RightStickButton );
			AddButtonControl( InputControlType.Start );
			AddButtonControl( InputControlType.Back );

			QueryState();
		}


		public override void Update( float updateTime, float deltaTime )
		{
			QueryState();

			Analogs[0].UpdateWithValue( state.ThumbSticks.Left.X, updateTime );
			Analogs[1].UpdateWithValue( state.ThumbSticks.Left.Y, updateTime );
			Analogs[2].UpdateWithValue( state.ThumbSticks.Right.X, updateTime );
			Analogs[3].UpdateWithValue( state.ThumbSticks.Right.Y, updateTime );
			Analogs[4].UpdateWithValue( state.Triggers.Left, updateTime );
			Analogs[5].UpdateWithValue( state.Triggers.Right, updateTime );

			Buttons[0].UpdateWithState( state.DPad.Up == ButtonState.Pressed, updateTime );
			Buttons[1].UpdateWithState( state.DPad.Down == ButtonState.Pressed, updateTime );
			Buttons[2].UpdateWithState( state.DPad.Left == ButtonState.Pressed, updateTime );
			Buttons[3].UpdateWithState( state.DPad.Right == ButtonState.Pressed, updateTime );
			Buttons[4].UpdateWithState( state.Buttons.A == ButtonState.Pressed, updateTime );
			Buttons[5].UpdateWithState( state.Buttons.B == ButtonState.Pressed, updateTime );
			Buttons[6].UpdateWithState( state.Buttons.X == ButtonState.Pressed, updateTime );
			Buttons[7].UpdateWithState( state.Buttons.Y == ButtonState.Pressed, updateTime );
			Buttons[8].UpdateWithState( state.Buttons.LeftShoulder == ButtonState.Pressed, updateTime );
			Buttons[9].UpdateWithState( state.Buttons.RightShoulder == ButtonState.Pressed, updateTime );
			Buttons[10].UpdateWithState( state.Buttons.LeftStick == ButtonState.Pressed, updateTime );
			Buttons[11].UpdateWithState( state.Buttons.RightStick == ButtonState.Pressed, updateTime );
			Buttons[12].UpdateWithState( state.Buttons.Start == ButtonState.Pressed, updateTime );
			Buttons[13].UpdateWithState( state.Buttons.Back == ButtonState.Pressed, updateTime );
		}


		public override void Vibrate( float leftMotor, float rightMotor )
		{
			GamePad.SetVibration( (PlayerIndex) DeviceIndex, leftMotor, rightMotor );
		}


		void QueryState()
		{
			state = GamePad.GetState( (PlayerIndex) DeviceIndex );
		}


		public bool IsConnected
		{
			get { return state.IsConnected; }
		}
	}
}