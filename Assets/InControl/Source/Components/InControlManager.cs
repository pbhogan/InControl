using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace InControl
{
	public class InControlManager : MonoBehaviour
	{
		public bool logDebugInfo = false;
		public bool invertYAxis = false;
		public bool enableXInput = false;
		public bool useFixedUpdate = false;
		public bool dontDestroyOnLoad = false;
		public List<string> customProfiles = new List<string>();

		private bool isBeingDestroyed;

		private static InControlManager instance;

        public static InControlManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<InControlManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

		void OnEnable()
		{
			if (instance == null)
            {
                instance = this;
            
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(this);
                }
            }
            else if (this != instance)
            {
                isBeingDestroyed = true; //If destroying, we don't want to reset the InputManager
                Destroy(gameObject);
            }

			if (logDebugInfo)
			{
				Debug.Log( "InControl (version " + InputManager.Version + ")" );
				Logger.OnLogMessage += HandleOnLogMessage;
			}

			InputManager.InvertYAxis = invertYAxis;
			InputManager.EnableXInput = enableXInput;
			InputManager.SetupInternal();

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

			if (dontDestroyOnLoad)
			{
				DontDestroyOnLoad( this );
			}
		}


		void OnDisable()
		{
			if (!isBeingDestroyed)
                InputManager.ResetInternal();
		}


		#if UNITY_ANDROID && INCONTROL_OUYA && !UNITY_EDITOR
		void Start()
		{
			StartCoroutine( CheckForOuyaEverywhereSupport() );
		}


		IEnumerator CheckForOuyaEverywhereSupport()
		{
			while (!OuyaSDK.isIAPInitComplete())
			{
				yield return null;
			}

			OuyaEverywhereDeviceManager.Enable();
		}
		#endif


		void Update()
		{
			if (!useFixedUpdate || Mathf.Approximately( Time.timeScale, 0.0f ))
			{
				InputManager.UpdateInternal();
			}
		}


		void FixedUpdate()
		{
			if (useFixedUpdate)
			{
				InputManager.UpdateInternal();
			}
		}


		void OnApplicationFocus( bool focusState )
		{
			InputManager.OnApplicationFocus( focusState );
		}


		void OnApplicationPause( bool pauseState )
		{
			InputManager.OnApplicationPause( pauseState );
		}


		void OnApplicationQuit()
		{
			InputManager.OnApplicationQuit();
		}


		void HandleOnLogMessage( LogMessage logMessage )
		{
			switch (logMessage.type)
			{
				case LogMessageType.Info:
					Debug.Log( logMessage.text );
					break;
				case LogMessageType.Warning:
					Debug.LogWarning( logMessage.text );
					break;
				case LogMessageType.Error:
					Debug.LogError( logMessage.text );
					break;
			}
		}
	}
}

