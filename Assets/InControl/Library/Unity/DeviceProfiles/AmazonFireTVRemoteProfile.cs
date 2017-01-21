using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class AmazonFireTVRemoteProfile : UnityInputDeviceProfile
	{
        public AmazonFireTVRemoteProfile()
		{
			Name = "Amazon";
			Meta = "Amazon Fire TV Remote on Android";
			
			SupportedPlatforms = new[]
			{
				"Android"			
			};

            JoystickNames = new[]
			{
				"Amazon Fire TV Remote"
			};
						
			Sensitivity = 1.0f;
			LowerDeadZone = 0.05f;
			UpperDeadZone = 0.95f;
			
			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button0
				}
            };
			
			AnalogMappings = new[]
			{
                new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog4,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog4,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog5,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
                    Invert = true
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog5,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				}
			};
		}
	}
}

