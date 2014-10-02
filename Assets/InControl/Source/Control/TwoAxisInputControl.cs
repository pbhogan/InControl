using System;
using UnityEngine;


namespace InControl
{
	public class TwoAxisInputControl
	{
		public float X { get; protected set; }
		public float Y { get; protected set; }

		public OneAxisInputControl Left { get; protected set; }
		public OneAxisInputControl Right { get; protected set; }
		public OneAxisInputControl Up { get; protected set; }
		public OneAxisInputControl Down { get; protected set; }

		public ulong UpdateTick { get; protected set; }

		bool thisState;
		bool lastState;

		public static float StateThreshold = 0.0f;


		internal TwoAxisInputControl()
		{
			Left = new OneAxisInputControl();
			Right = new OneAxisInputControl();
			Up = new OneAxisInputControl();
			Down = new OneAxisInputControl();
		}


		internal void Update( float x, float y, ulong updateTick )
		{
			lastState = thisState;

			X = x;
			Y = y;

			Left.UpdateWithValue( Mathf.Clamp01( -X ), updateTick, StateThreshold );
			Right.UpdateWithValue( Mathf.Clamp01( X ), updateTick, StateThreshold );

			if (InputManager.InvertYAxis)
			{
				Up.UpdateWithValue( Mathf.Clamp01( -Y ), updateTick, StateThreshold );
				Down.UpdateWithValue( Mathf.Clamp01( Y ), updateTick, StateThreshold );
			}
			else
			{
				Up.UpdateWithValue( Mathf.Clamp01( Y ), updateTick, StateThreshold );
				Down.UpdateWithValue( Mathf.Clamp01( -Y ), updateTick, StateThreshold );
			}

			thisState = Up.State || Down.State || Left.State || Right.State;

			if (thisState != lastState)
			{
				UpdateTick = updateTick;
			}
		}


		public bool State 
		{
			get { return thisState; }
		}


		public bool HasChanged 
		{
			get { return thisState != lastState; }
		}


		public Vector2 Vector
		{
			get { return new Vector2( X, Y ); }
		}


		public static implicit operator bool( TwoAxisInputControl control )
		{
			return control.thisState;
		}


		public static implicit operator Vector2( TwoAxisInputControl control )
		{
			return control.Vector;
		}


		public static implicit operator Vector3( TwoAxisInputControl control )
		{
			return new Vector3( control.X, control.Y );
		}
	}
}

