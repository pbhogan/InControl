using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	public class UnityUnknownDeviceProfile : UnityInputDeviceProfile
	{
		public UnityUnknownDeviceProfile( string joystickName )
		{
			Name = "Unknown Device";
			if (joystickName != "")
			{
				Name += " (" + joystickName + ")";
			}

			Meta = "";
			Sensitivity = 1.0f;
			LowerDeadZone = 0.2f;

			SupportedPlatforms = null;
			JoystickNames = new[] { joystickName };

			AnalogMappings = new InputControlMapping[ UnityInputDevice.MaxAnalogs ];
			for (int i = 0; i < UnityInputDevice.MaxAnalogs; i++)
			{
				AnalogMappings[i] = new InputControlMapping
				{
					Handle = "Analog " + i,
					Source = Analog( i ),
					Target = InputControlType.Analog0 + i
				};
			}

			ButtonMappings = new InputControlMapping[ UnityInputDevice.MaxButtons ];
			for (int i = 0; i < UnityInputDevice.MaxButtons; i++)
			{
				ButtonMappings[i] = new InputControlMapping
				{
					Handle = "Button " + i,
					Source = Button( i ),
					Target = InputControlType.Button0 + i
				};
			}
		}


		public override bool IsKnown
		{
			get { return false; }
		}
	}	
}

