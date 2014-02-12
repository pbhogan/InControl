using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class BasicKeyboardProfile : UnityInputDeviceProfile
	{
		public BasicKeyboardProfile()
		{
			Name = "Keyboard";
			Meta = "A simple keyboard profile appropriate for platformers and such.";

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
					Handle = "Arrow Keys X",
					Target = InputControlType.LeftStickX,
					Source = KeyCodeAxis( KeyCode.LeftArrow, KeyCode.RightArrow )
				},
				new InputControlMapping
				{
					Handle = "Arrow Keys Y",
					Target = InputControlType.LeftStickY,
					Source = KeyCodeAxis( KeyCode.DownArrow, KeyCode.UpArrow )
				}
			};
		}
	}
}

