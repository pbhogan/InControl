using System;
using System.Collections;
using UnityEngine;
using InControl;


namespace GyroExample
{
	// This is kind of "beta"... while it works on iOS, gyro controls are
	// inconsistent and are usually fine tuned to the games that use them
	// which is somewhat beyond the scope of this project. But, if you 
	// are curious how to go about it, here you go.
	//
	public class CubeController : MonoBehaviour
	{
		void Start()
		{
			// It seems to take Unity iOS a moment to figure out 
			// what orientation the device is in, so we wait a bit
			// before calibration.
			StartCoroutine( CalibrateGyro() );
		}


		IEnumerator CalibrateGyro()
		{
			yield return new WaitForSeconds( 0.1f );
			UnityGyroAxisSource.Calibrate();
		}


		void Update()
		{
			// Touch screen to recalibrate.
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (Input.GetTouch( i ).phase == TouchPhase.Began)
				{
					UnityGyroAxisSource.Calibrate();
				}
			}

			// Use last device which provided input.
			var inputDevice = InputManager.ActiveDevice;

			// Rotate target object with gyroscope mapped to left stick.
			transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.LeftStickX, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.LeftStickY, Space.World );
		}
	}
}

