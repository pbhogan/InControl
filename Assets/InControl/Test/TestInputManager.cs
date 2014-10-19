using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InControl;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif


/**
 * WARNING: This is NOT an example of how to use InControl.
 * It is intended for testing and troubleshooting the library.
 * It can also be used for create new device profiles as it will
 * show the default Unity mappings for unknown devices.
 **/
namespace InControl
{
	public class TestInputManager : MonoBehaviour
	{
		public Font font;

		GUIStyle style = new GUIStyle();
		List<LogMessage> logMessages = new List<LogMessage>();
		bool isPaused;


		void OnEnable()
		{
			isPaused = false;
			Time.timeScale = 1.0f;

			Logger.OnLogMessage += logMessage => logMessages.Add( logMessage );

//			InputManager.HideDevicesWithProfile( typeof( Xbox360MacProfile ) );

			InputManager.OnDeviceAttached += inputDevice => Debug.Log( "Attached: " + inputDevice.Name );
			InputManager.OnDeviceDetached += inputDevice => Debug.Log( "Detached: " + inputDevice.Name );
			InputManager.OnActiveDeviceChanged += inputDevice => Debug.Log( "Active device changed to: " + inputDevice.Name );

			TestInputMappings();

//			Debug.Log( "Unity Version: " + InputManager.UnityVersion );
		}


		void FixedUpdate()
		{
			CheckForPauseButton();

//			var inputDevice = InputManager.ActiveDevice;
//			if (inputDevice.Direction.Left.WasPressed)
//			{
//				Debug.Log( "Left.WasPressed" );
//			}
//			if (inputDevice.Direction.Left.WasReleased)
//			{
//				Debug.Log( "Left.WasReleased" );
//			}
		}


		void Update()
		{
			if (isPaused)
			{
				CheckForPauseButton();
			}

			if (Input.GetKeyDown( KeyCode.R ))
			{
				Application.LoadLevel( "TestInputManager" );
			}
		}


		void CheckForPauseButton()
		{
			if (Input.GetKeyDown( KeyCode.P ) || InputManager.MenuWasPressed)
			{
				Time.timeScale = isPaused ? 1.0f : 0.0f;
				isPaused = !isPaused;
			}
		}


		void SetColor( Color color )
		{
			style.normal.textColor = color;
		}


		void OnGUI()
		{
			var w = 300;
			var x = 10;
			var y = 10;
			var lineHeight = 15;

			GUI.skin.font = font;
			SetColor( Color.white );

			string info = "Devices:";
			info += " (Platform: " + InputManager.Platform + ")";
//			info += " (Joysticks " + InputManager.JoystickHash + ")";
			info += " " + InputManager.ActiveDevice.Direction.Vector;

//			#if UNITY_EDITOR
//			if (EditorWindow.focusedWindow != null)
//			{
//				info += " " + EditorWindow.focusedWindow.ToString();
//			}
//			#endif

			if (isPaused)
			{
				SetColor( Color.red );
				info = "+++ PAUSED +++";
			}

			GUI.Label( new Rect( x, y, x + w, y + 10 ), info, style );

			SetColor( Color.white );

			foreach (var inputDevice in InputManager.Devices)
			{
				bool active = InputManager.ActiveDevice == inputDevice;
				Color color = active ? Color.yellow : Color.white;

				y = 35;

				SetColor( color );

				GUI.Label( new Rect( x, y, x + w, y + 10 ), inputDevice.Name, style );
				y += lineHeight;

				GUI.Label( new Rect( x, y, x + w, y + 10 ), "SortOrder: " + inputDevice.SortOrder, style );
				y += lineHeight;

				GUI.Label( new Rect( x, y, x + w, y + 10 ), "LastChangeTick: " + inputDevice.LastChangeTick, style );
				y += lineHeight;

				foreach (var control in inputDevice.Controls)
				{
					if (control != null)
					{
						string controlName;

						if (inputDevice.IsKnown)
						{
							controlName = string.Format( "{0} ({1})", control.Target, control.Handle );
						}
						else
						{
							controlName = control.Handle;
						}

						SetColor( control.State ? Color.green : color );
						var label = string.Format( "{0} {1}", controlName, control.State ? "= " + control.Value : "" );
						GUI.Label( new Rect( x, y, x + w, y + 10 ), label, style );
						y += lineHeight;
					}
				}

				SetColor( Color.cyan );
				var anyButton = inputDevice.AnyButton;
				if (anyButton)
				{
					GUI.Label( new Rect( x, y, x + w, y + 10 ), "AnyButton = " + anyButton.Handle, style );
				}

				x += 200;
			}


			Color[] logColors = { Color.gray, Color.yellow, Color.white };
			SetColor( Color.white );
			x = 10;
			y = Screen.height - (10 + lineHeight);
			for (int i = logMessages.Count - 1; i >= 0; i--)
			{
				var logMessage = logMessages[i];
				SetColor( logColors[(int) logMessage.type] );
				foreach (var line in logMessage.text.Split('\n'))
				{
					GUI.Label( new Rect( x, y, Screen.width, y + 10 ), line, style );
					y -= lineHeight;
				}
			}
		}


		void OnDrawGizmos()
		{
			Vector3 delta = InputManager.ActiveDevice.Direction.Vector * 4.0f;
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere( delta, 1 );
		}


		void TestInputMappings()
		{
			var complete = InputControlMapping.Range.Complete;
			var positive = InputControlMapping.Range.Positive;
			var negative = InputControlMapping.Range.Negative;
			var noInvert = false;
			var doInvert = true;

			TestInputMapping( complete, complete, noInvert, -1.0f, 0.0f, 1.0f );
			TestInputMapping( complete, negative, noInvert, -1.0f, -0.5f, 0.0f );
			TestInputMapping( complete, positive, noInvert, 0.0f, 0.5f, 1.0f );

			TestInputMapping( negative, complete, noInvert, -1.0f, 1.0f, 0.0f );
			TestInputMapping( negative, negative, noInvert, -1.0f, 0.0f, 0.0f );
			TestInputMapping( negative, positive, noInvert, 0.0f, 1.0f, 0.0f );

			TestInputMapping( positive, complete, noInvert, 0.0f, -1.0f, 1.0f );
			TestInputMapping( positive, negative, noInvert, 0.0f, -1.0f, 0.0f );
			TestInputMapping( positive, positive, noInvert, 0.0f, 0.0f, 1.0f );

			TestInputMapping( complete, complete, doInvert, 1.0f, 0.0f, -1.0f );
			TestInputMapping( complete, negative, doInvert, 1.0f, 0.5f, 0.0f );
			TestInputMapping( complete, positive, doInvert, 0.0f, -0.5f, -1.0f );

			TestInputMapping( negative, complete, doInvert, 1.0f, -1.0f, 0.0f );
			TestInputMapping( negative, negative, doInvert, 1.0f, 0.0f, 0.0f );
			TestInputMapping( negative, positive, doInvert, 0.0f, -1.0f, 0.0f );

			TestInputMapping( positive, complete, doInvert, 0.0f, 1.0f, -1.0f );
			TestInputMapping( positive, negative, doInvert, 0.0f, 1.0f, 0.0f );
			TestInputMapping( positive, positive, doInvert, 0.0f, 0.0f, -1.0f );
		}


		void TestInputMapping( InputControlMapping.Range sourceRange, InputControlMapping.Range targetRange, bool invert, float expectA, float expectB, float expectC )
		{
			var mapping = new InputControlMapping() {
				SourceRange = sourceRange,
				TargetRange = targetRange,
				Invert = invert
			};

			float input;
			float value;

			string sr = "Complete";
			if (sourceRange == InputControlMapping.Range.Negative)
				sr = "Negative";
			else
			if (sourceRange == InputControlMapping.Range.Positive)
				sr = "Positive";

			string tr = "Complete";
			if (targetRange == InputControlMapping.Range.Negative)
				tr = "Negative";
			else
			if (targetRange == InputControlMapping.Range.Positive)
				tr = "Positive";


			input = -1.0f;
			value = mapping.MapValue( input );
			if (Mathf.Abs( value - expectA ) > Single.Epsilon)
			{
				Debug.LogError( "Input of " + input + " got value of " + value + " instead of " + expectA + " (SR = " + sr + ", TR = " + tr + ")" );
			}

			input = 0.0f;
			value = mapping.MapValue( input );
			if (Mathf.Abs( value - expectB ) > Single.Epsilon)
			{
				Debug.LogError( "Input of " + input + " got value of " + value + " instead of " + expectB + " (SR = " + sr + ", TR = " + tr + ")" );
			}

			input = 1.0f;
			value = mapping.MapValue( input );
			if (Mathf.Abs( value - expectC ) > Single.Epsilon)
			{
				Debug.LogError( "Input of " + input + " got value of " + value + " instead of " + expectC + " (SR = " + sr + ", TR = " + tr + ")" );
			}
		}
	}
}


