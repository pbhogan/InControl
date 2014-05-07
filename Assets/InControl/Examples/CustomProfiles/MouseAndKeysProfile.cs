using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class MouseAndKeysProfile : UnityInputDeviceProfile
	{
		public MouseAndKeysProfile()
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
			LowerDeadZone = 0.0f;

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Fire - Mouse",
					Target = InputControlType.Action1,
					Source = MouseButton0
				},
				new InputControlMapping
				{
					Handle = "Fire - Keyboard",
					Target = InputControlType.Action1,
					Source = KeyCodeButton( KeyCode.F )
				},
				new InputControlMapping
				{
					Handle = "AltFire",
					Target = InputControlType.Action2,
					Source = MouseButton2
				},
				new InputControlMapping
				{
					Handle = "Middle",
					Target = InputControlType.Action3,
					Source = MouseButton1
				},
				new InputControlMapping
				{
					Handle = "Jump",
					Target = InputControlType.Action4,
					Source = KeyCodeButton( KeyCode.Space )
				},
			};

			AnalogMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Move X",
					Target = InputControlType.LeftStickX,
					Source = KeyCodeAxes(new KeyCode[]{KeyCode.A, KeyCode.LeftArrow} , new KeyCode[]{KeyCode.D, KeyCode.RightArrow} )
				},
				new InputControlMapping
				{
					Handle = "Move Y",
					Target = InputControlType.LeftStickY,
					Source = KeyCodeAxes( new KeyCode[]{KeyCode.S, KeyCode.DownArrow}, new KeyCode[]{KeyCode.W, KeyCode.UpArrow} )
				},
				new InputControlMapping
				{
					Handle = "Look X",
					Target = InputControlType.RightStickX,
					Source = MouseXAxis,
					Raw    = true
				},
				new InputControlMapping
				{
					Handle = "Look Y",
					Target = InputControlType.RightStickY,
					Source = MouseYAxis,
					Raw    = true
				},
				new InputControlMapping
				{
					Handle = "Look Z",
					Target = InputControlType.ScrollWheel,
					Source = MouseScrollWheel,
					Raw    = true
				}
			};
		}
	}
}

