using System;
using UnityEngine;


namespace InControl
{
	public class UnityKeyCodeAxesSource : InputControlSource
	{
		KeyCode[] negativeKeyCodes;
		KeyCode[] positiveKeyCodes;


		public UnityKeyCodeAxesSource( KeyCode[] negativeKeyCodes, KeyCode[] positiveKeyCodes )
		{
			this.negativeKeyCodes = negativeKeyCodes;
			this.positiveKeyCodes = positiveKeyCodes;
		}


		public override float GetValue( InputDevice inputDevice )
		{
			int axisValue = 0;

			foreach (KeyCode code in negativeKeyCodes)
			{
				if (Input.GetKey( code ))
				{
					axisValue--;
					break;
				}
			}


			foreach (KeyCode code in positiveKeyCodes)
			{
				if (Input.GetKey( code ))
				{
					axisValue++;
					break;
				}
			}

			return axisValue;
		}


		public override bool GetState( InputDevice inputDevice )
		{
			return !Mathf.Approximately( GetValue( inputDevice ), 0.0f );
		}
	}
}

