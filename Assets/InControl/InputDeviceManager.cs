using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


namespace InControl
{
	public class InputDeviceManager
	{
		protected List<InputDevice> devices = new List<InputDevice>();

		float lastUpdateTime = 0.0f;


		public virtual void Update( float updateTime, float deltaTime )
		{
		}


		public void Update( float updateTime )
		{
			var deltaTime = updateTime - lastUpdateTime;

			Update( updateTime, deltaTime );

			foreach (var device in devices)
			{
				device.UpdateLastChangeTime( updateTime );
			}

			lastUpdateTime = updateTime;
		}
	}
}

