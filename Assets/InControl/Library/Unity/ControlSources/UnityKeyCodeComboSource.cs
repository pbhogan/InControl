using System;
using UnityEngine;


namespace InControl
{
	public class UnityKeyCodeComboSource : InputControlSource
	{
		KeyCode[] keyCodeList;


		public UnityKeyCodeComboSource( params KeyCode[] keyCodeList )
		{
			this.keyCodeList = keyCodeList;
		}


		public float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}


		public bool GetState( InputDevice inputDevice )
		{
			for (int i = 0; i < keyCodeList.Length; i++)
			{
				if (!Input.GetKey( keyCodeList[i] ))
				{
					return false;
				}
			}
			return true;
		}
	}
}

