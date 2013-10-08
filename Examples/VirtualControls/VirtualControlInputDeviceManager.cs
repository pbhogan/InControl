using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class VirtualControlInputDeviceManager : InputDeviceManager, FMultiTouchableInterface
	{
		InputDevice device;
		List<FTouch> touches = new List<FTouch>();
		List<IVirtualControl> virtualControls = new List<IVirtualControl>();

		public FContainer Node = new FContainer();


		public VirtualControlInputDeviceManager()
		{
			device = new InputDevice( "VirtualControlInputDevice", 2, 2 );
			device.AddAnalogControl( InputControlType.LeftStickX, "Virtual Stick" );
			device.AddAnalogControl( InputControlType.LeftStickY, "Virtual Stick" );
			device.AddButtonControl( InputControlType.Action1, "Virtual Button 1" );
			device.AddButtonControl( InputControlType.Action2, "Virtual Button 2" );
			InputManager.AttachDevice( device );

			var virtualStick = new VirtualControlStick( InputControlType.LeftStickX, InputControlType.LeftStickY );
			Node.AddChild( virtualStick );
			virtualControls.Add( virtualStick );

			var buttonPosition1 = new Vector2( Futile.screen.halfWidth - 128, -Futile.screen.halfHeight + 64 );
			var virtualButton1 = new VirtualControlButton( InputControlType.Action1, "VirtualInput/ButtonA", buttonPosition1 );
			Node.AddChild( virtualButton1 );
			virtualControls.Add( virtualButton1 );

			var buttonPosition2 = new Vector2( Futile.screen.halfWidth - 64, -Futile.screen.halfHeight + 128 );
			var virtualButton2 = new VirtualControlButton( InputControlType.Action2, "VirtualInput/ButtonB", buttonPosition2 );
			Node.AddChild( virtualButton2 );
			virtualControls.Add( virtualButton2 );

			Futile.touchManager.AddMultiTouchTarget( this );
		}


		public override void Update( float updateTime, float deltaTime )
		{
			if (touches.Count > 0)
			{
				foreach (var virtualControl in virtualControls)
				{
					virtualControl.Update( touches, device, updateTime );
				}

				touches.Clear();
			}
		}


		#region FMultiTouchableInterface implementation

		public void HandleMultiTouch( FTouch[] touches )
		{
			this.touches.AddRange( touches );
		}

		#endregion
	}
}

