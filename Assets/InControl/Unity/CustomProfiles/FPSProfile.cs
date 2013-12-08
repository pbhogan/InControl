using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class FPSProfile : UnityInputDeviceProfile
	{
		public FPSProfile()
		{
			Name = "FPS Keyboard/Mouse";
			Meta = "A keyboard and mouse combination profile appropriate for FPS.";

			SupportedPlatforms = new[]
			{
				"Windows",
				"Mac",
				"Linux"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.0f;

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Spacebar",
					Target = InputControlType.Action1,
					Source = KeyCodeButton( KeyCode.Space )
				},
				new InputControlMapping
				{
					Handle = "A Key",
					Target = InputControlType.Action2,
					Source = KeyCodeButton( KeyCode.A )
				},
				new InputControlMapping
				{
					Handle = "S Key",
					Target = InputControlType.Action3,
					Source = KeyCodeButton( KeyCode.S )
				},
				new InputControlMapping
				{
					Handle = "D Key",
					Target = InputControlType.Action4,
					Source = KeyCodeButton( KeyCode.D )
				}
			};

			AnalogMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Move X",
					Target = InputControlType.LeftStickX,
					Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
				},
				new InputControlMapping
				{
					Handle = "Move Y",
					Target = InputControlType.LeftStickY,
					Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
				},
				new InputControlMapping
				{
					Handle = "Look X",
					Target = InputControlType.RightStickX,
					Source = new UnityMouseAxisSource( "x" ),
					Raw    = true
				},
				new InputControlMapping
				{
					Handle = "Look Y",
					Target = InputControlType.RightStickY,
					Source = new UnityMouseAxisSource( "y" ),
					Raw    = true
				}
			};
		}
	}
}

