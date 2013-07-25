using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[DeviceProfile]
	public class KeyboardProfile : InputDeviceProfile
	{
		public KeyboardProfile()
		{
			name = "Keyboard";
			meta = "";

			supportedPlatforms = new List<string>()
			{
				"Windows",
				"Mac",
				"Linux"
			};


			sensitivity = 1.0f;
			deadZone = 0.0f;

			buttonMappings = new InputControlButtonMapping[]
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

			analogMappings = new InputControlAnalogMapping[]
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

