using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class VirtualControlStick : FContainer, IVirtualControl
	{
		Vector2 delta;
		Vector2 value;
		int touchId;
		InputControlType xAxis;
		InputControlType yAxis;
		FSprite baseSprite;
		FSprite headSprite;


		public VirtualControlStick( InputControlType xAxis, InputControlType yAxis )
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;

			delta = Vector2.zero;
			value = Vector2.zero;
			touchId = -1;
			alpha = 0.5f;

			baseSprite = new FSprite( "VirtualInput/StickBase" );
			baseSprite.SetAnchor( new Vector2( 0.5f, 0.5f ) );
			AddChild( baseSprite );

			headSprite = new FSprite( "VirtualInput/StickHead" );
			headSprite.SetAnchor( new Vector2( 0.5f, 0.5f ) );
			AddChild( headSprite );

			SetPosition( -Futile.screen.halfWidth + 96, -Futile.screen.halfHeight + 96 );
		}


		public void Update( IEnumerable<FTouch> touches, InputDevice device, float updateTime )
		{
			foreach (var touch in touches)
			{
				UpdateWithTouch( touch );
			}

			device.GetControl( xAxis ).UpdateWithValue( value.x, updateTime );
			device.GetControl( yAxis ).UpdateWithValue( value.y, updateTime );

			alpha = Mathf.MoveTowards( alpha, touchId == -1 ? 0.5f : 1.0f, 1.0f / 15.0f );

			headSprite.SetPosition( delta );
		}


		void UpdateWithTouch( FTouch touch )
		{
			if (touchId == -1 && TouchIsBegan( touch ) && TouchIsWithinRange( touch ))
			{
				touchId = touch.fingerId;
				SetPosition( touch.position );
			}

			if (touchId == touch.fingerId)
			{
				if (TouchIsEnded( touch ))
				{
					delta = Vector2.zero;
					value = Vector2.zero;
					touchId = -1;
					return;
				}

				delta = Vector2.ClampMagnitude( touch.position - GetPosition(), 64.0f );
				value = delta / 64.0f;

				return;
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


		bool TouchIsWithinRange( FTouch touch )
		{
			var distance = Futile.screen.halfWidth;
			var origin = new Vector2( -Futile.screen.halfWidth, -Futile.screen.halfHeight );
			return (touch.position - origin).sqrMagnitude <= (distance * distance);
		}
	}
}

