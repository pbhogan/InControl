using System;
using UnityEngine;


namespace InControl
{
	public struct InputData
	{
		const int InputMask = 63488; // upper 5 bits
		const int ValueMask = 2047; // lower 11 bits
		const float ValueMaskAsFloat = 2046.0f;
		
		public UInt16 Data;


		public int Input
		{
			get
			{
				return (Data & InputMask) >> 11;
			}

			set
			{
				var inputBits = (value << 11) & InputMask;
				Data = (UInt16) ((Data & ValueMask) | inputBits);
			}
		}


		public float Value
		{
			get
			{
				return BitsToValue( Data & ValueMask );
			}

			set
			{
				Data = (UInt16) ((Data & InputMask) | ValueToBits( value ));
			}
		}


		static int ValueToBits( float value )
		{
			if (value == 0.0f)
			{
				return 0;
			}

			return (int) (Mathf.InverseLerp( -1.0f, 1.0f, value ) * ValueMaskAsFloat); 
		}


		static float BitsToValue( int bits )
		{
			if (bits == 0)
			{
				return 0.0f;
			}

			return Mathf.Lerp( -1.0f, 1.0f, bits / ValueMaskAsFloat );
		}


		public static float AlignValue( float value )
		{
			return value;
	//		return BitsToValue( ValueToBits( value ) );
		}
	}
}

