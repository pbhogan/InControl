using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	public class UnknownDeviceProfile : UnityInputDeviceProfile
	{
		const int MaxUnityButtons = 20;
		const int MaxUnityAnalogs = 10;


		public UnknownDeviceProfile( string joystickName )
		{
			Name = "Unknown Device";
			if (joystickName != "")
			{
				Name += " (" + joystickName + ")";
			}

			Meta = "";
			Sensitivity = 1.0f;
			DeadZone = 0.2f;

			SupportedPlatforms = null;
			JoystickNames = new[] { joystickName };

			AnalogMappings = new InputControlMapping[ MaxUnityAnalogs ];
			for (int i = 0; i < MaxUnityAnalogs; i++)
			{
				AnalogMappings[i] = new InputControlMapping
				{
					Handle = "Analog " + i,
					Source = Analog( i ),
					Target = (InputControlType) i
				};
			}

			ButtonMappings = new InputControlMapping[ MaxUnityButtons ];
			for (int i = 0; i < MaxUnityButtons; i++)
			{
				ButtonMappings[i] = new InputControlMapping
				{
					Handle = "Button " + i,
					Source = Button( i ),
					Target = (InputControlType) i
				};
			}
		}
	}
}

