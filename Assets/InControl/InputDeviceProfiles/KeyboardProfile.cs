using System;
using System.Collections;
using System.Collections.Generic;


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
					Source = "space"
				},
				new InputControlButtonMapping()
				{
					Handle = "A Key",
					Target = InputControlType.Action2,
					Source = "a"
				},
				new InputControlButtonMapping()
				{
					Handle = "S Key",
					Target = InputControlType.Action3,
					Source = "s"
				},
				new InputControlButtonMapping()
				{
					Handle = "D Key",
					Target = InputControlType.Action4,
					Source = "d"
				}
			};

			AnalogMappings = new InputControlAnalogMapping[]
			{
				new InputControlAnalogMapping()
				{
					Handle = "Arrow Keys X",
					Target = InputControlType.LeftStickX,
					Source = "left right"
				},
				new InputControlAnalogMapping()
				{
					Handle = "Arrow Keys Y",
					Target = InputControlType.LeftStickY,
					Source = "down up"
				}
			};
		}
	}
}

