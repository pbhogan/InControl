using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class VirtualControlButton : FContainer, IVirtualControl
	{
		InputControlType controlType;
		float value;
		int touchId;
		FSprite sprite;


		public VirtualControlButton( InputControlType controlType, string elementName, Vector2 position )
		{
			this.controlType = controlType;

			value = 0.0f;
			touchId = -1;
			alpha = 0.5f;

			sprite = new FSprite( elementName );
			sprite.SetAnchor( new Vector2( 0.5f, 0.5f ) );
			AddChild( sprite );

			SetPosition( position );
		}


		public void Update( IEnumerable<FTouch> touches, InputDevice device, float updateTime )
		{
			value = 0.0f;

			foreach (var touch in touches)
			{
				UpdateWithTouch( touch );
			}

			device.GetControl( controlType ).UpdateWithValue( value, updateTime );

			alpha = Mathf.MoveTowards( alpha, value > 0.0f ? 1.0f : 0.5f, 1.0f / 15.0f );
		}


		void UpdateWithTouch( FTouch touch )
		{
			if (touchId == touch.fingerId)
			{
				if (TouchIsEnded( touch ))
				{
					touchId = -1;
					return;
				}

				value = 1.0f;
				return;
			}

			if (!TouchIsEnded( touch ) && TouchIsWithinDistance( touch, 32.0f ))
			{
				if (TouchIsBegan( touch ) && touchId == -1)
				{
					touchId = touch.fingerId;
				}

				value = 1.0f;
			}
		}


		bool TouchIsBegan( FTouch touch )
		{
			return touch.phase == TouchPhase.Began;
		}


		bool TouchIsEnded( FTouch touch )
		{
			return touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
		}


		bool TouchIsWithinDistance( FTouch touch, float distance )
		{
			return (touch.position - GetPosition()).sqrMagnitude <= (distance * distance);
		}
	}
}

