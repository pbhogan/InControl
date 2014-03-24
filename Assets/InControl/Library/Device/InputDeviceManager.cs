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


		public virtual void Update( ulong updateTick, float deltaTime )
		{
		}
	}
}

