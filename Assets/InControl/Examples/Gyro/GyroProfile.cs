using System;
using System.Collections;
using UnityEngine;
using InControl;


namespace GyroExample
{
	// This custom profile is enabled by adding it to the Custom Profiles list
	// on the InControlManager script, which is attached to the InControl 
	// game object in this example scene.
	// 
	public class GyroProfile : UnityInputDeviceProfile
	{
		public GyroProfile()
		{
			Name = "Gyroscope";
			Meta = "Gyroscope on iOS.";

			// This profile only works on mobile.
			SupportedPlatforms = new[]
			{
				"iPhone",
				"Android"
			};

			ButtonMappings = new InputControlMapping[]
			{
			};

			AnalogMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Move X",
					Target = InputControlType.LeftStickX,
					Source = new UnityGyroAxisSource( UnityGyroAxisSource.GyroAxis.X ),
					Raw    = true,
					Scale  = 3.0f
				},
				new InputControlMapping
				{
					Handle = "Move Y",
					Target = InputControlType.LeftStickY,
					Source = new UnityGyroAxisSource( UnityGyroAxisSource.GyroAxis.Y ),
					Raw    = true,
					Scale  = 3.0f
				},
			};
		}
	}
}

