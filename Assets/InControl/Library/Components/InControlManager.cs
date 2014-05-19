using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace InControl
{
	public class InControlManager : MonoBehaviour
	{
		public bool invertYAxis = false;
		public bool enableXInput = false;
		public bool useFixedUpdate = false;
		public List<string> customProfiles = new List<string>();


		void OnEnable()
		{
			InputManager.InvertYAxis = invertYAxis;
			InputManager.EnableXInput = enableXInput;
			InputManager.Setup();

			foreach (var className in customProfiles)
			{
				var classType = Type.GetType( className );
				if (classType == null)
				{
					Debug.LogError( "Cannot find class for custom profile: " + className );
				}
				else
				{
					var customProfileInstance = Activator.CreateInstance( classType ) as UnityInputDeviceProfile;
					InputManager.AttachDevice( new UnityInputDevice( customProfileInstance ) );
				}
			}
			
			Debug.Log( "InControl (version " + InputManager.Version + ")" );
		}


		void Update()
		{
			if (!useFixedUpdate || Mathf.Approximately( Time.timeScale, 0.0f ))
			{
				InputManager.Update();
			}
		}


		void FixedUpdate()
		{
			if (useFixedUpdate)
			{
				InputManager.Update();
			}
		}


		void OnApplicationQuit()
		{

		}
	}
}

