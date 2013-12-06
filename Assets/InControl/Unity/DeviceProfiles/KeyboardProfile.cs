using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	[AutoDiscover]
	public class KeyboardProfile : UnityInputDeviceProfile
	{
		public KeyboardProfile()
		{
			Name = "Keyboard";
			Meta = "";

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
				new InputControlButtonMapping()
				{
					Handle = "Spacebar",
					Target = InputControlType.Action1,
					Source = KeyCodeButton( KeyCode.Space )
				},
				new InputControlButtonMapping()
				{
					Handle = "A Key",
					Target = InputControlType.Action2,
					Source = KeyCodeButton( KeyCode.A )
				},
				new InputControlButtonMapping()
				{
					Handle = "S Key",
					Target = InputControlType.Action3,
					Source = KeyCodeButton( KeyCode.S )
				},
				new InputControlButtonMapping()
				{
					Handle = "D Key",
					Target = InputControlType.Action4,
					Source = KeyCodeButton( KeyCode.D )
				}
			};

			AnalogMappings = new[]
			{
				new InputControlAnalogMapping()
				{
					Handle = "Arrow Keys X",
					Target = InputControlType.LeftStickX,
					Source = KeyCodeAxis( KeyCode.LeftArrow, KeyCode.RightArrow )
				},
				new InputControlAnalogMapping()
				{
					Handle = "Arrow Keys Y",
					Target = InputControlType.LeftStickY,
					Source = KeyCodeAxis( KeyCode.DownArrow, KeyCode.UpArrow )
				}
			};
		}
	}
}

