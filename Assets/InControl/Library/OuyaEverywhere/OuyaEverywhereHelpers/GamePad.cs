#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using tv.ouya.console.api;

namespace OuyaEverywhereHelpers
{

	public enum ButtonState
	{
		Pressed,
		Released
	}


	public struct GamePadButtons
	{
		// a-o
		// b-a
		// x-u
		// y-y
		ButtonState start, leftStick, rightStick, leftShoulder, rightShoulder, o, a, u, y;

		internal GamePadButtons( ButtonState start,  ButtonState leftStick, ButtonState rightStick,
								ButtonState leftShoulder, ButtonState rightShoulder, ButtonState o, ButtonState a,
								ButtonState u, ButtonState y )
		{
			this.start = start;
			this.leftStick = leftStick;
			this.rightStick = rightStick;
			this.leftShoulder = leftShoulder;
			this.rightShoulder = rightShoulder;
			this.o = o;
			this.a = a;
			this.u = u;
			this.y = y;
		}


		public ButtonState Start
		{
			get { return start; }
		}


		public ButtonState LeftStick
		{
			get { return leftStick; }
		}


		public ButtonState RightStick
		{
			get { return rightStick; }
		}


		public ButtonState LeftShoulder
		{
			get { return leftShoulder; }
		}


		public ButtonState RightShoulder
		{
			get { return rightShoulder; }
		}


		public ButtonState O
		{
			get { return o; }
		}


		public ButtonState A
		{
			get { return a; }
		}


		public ButtonState U
		{
			get { return u; }
		}


		public ButtonState Y
		{
			get { return y; }
		}
	}


	public struct GamePadDPad
	{
		ButtonState up, down, left, right;

		internal GamePadDPad( ButtonState up, ButtonState down, ButtonState left, ButtonState right )
		{
			this.up = up;
			this.down = down;
			this.left = left;
			this.right = right;
		}


		public ButtonState Up
		{
			get { return up; }
		}


		public ButtonState Down
		{
			get { return down; }
		}


		public ButtonState Left
		{
			get { return left; }
		}


		public ButtonState Right
		{
			get { return right; }
		}
	}


	public struct GamePadThumbSticks
	{
		public struct StickValue
		{
			float x, y;

			internal StickValue(float x, float y)
			{
				this.x = x;
				this.y = y;
			}

			public float X
			{
				get { return x; }
			}

			public float Y
			{
				get { return y; }
			}
		}

		StickValue left, right;


		internal GamePadThumbSticks( StickValue left, StickValue right )
		{
			this.left = left;
			this.right = right;
		}


		public StickValue Left
		{
			get { return left; }
		}


		public StickValue Right
		{
			get { return right; }
		}
	}


	public struct GamePadTriggers
	{
		float left;
		float right;


		internal GamePadTriggers( float left, float right )
		{
			this.left = left;
			this.right = right;
		}


		public float Left
		{
			get { return left; }
		}


		public float Right
		{
			get { return right; }
		}
	}


	public struct GamePadState
	{
		bool isConnected;
		GamePadButtons buttons;
		GamePadDPad dPad;
		GamePadThumbSticks thumbSticks;
		GamePadTriggers triggers;

		internal GamePadState( PlayerIndex playerIndex)
		{
			this.isConnected = OuyaSDK.OuyaInput.IsControllerConnected((int)playerIndex);;

			if (!isConnected) {
				buttons = new GamePadButtons(
					ButtonState.Released, 
					ButtonState.Released, 
					ButtonState.Released, 
					ButtonState.Released, 
					ButtonState.Released, 
					ButtonState.Released, 
					ButtonState.Released, 
					ButtonState.Released,
					ButtonState.Released
					);

				dPad = new GamePadDPad(
					ButtonState.Released,
					ButtonState.Released,
					ButtonState.Released,
					ButtonState.Released
					);

				thumbSticks = new GamePadThumbSticks(new GamePadThumbSticks.StickValue(0,0), new GamePadThumbSticks.StickValue(0,0));
				triggers = new GamePadTriggers(0,0);

			}else{
				buttons = new GamePadButtons(
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_MENU) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_L3) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_R3) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_L1) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_R1) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_O) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_A) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_U) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_Y) ? ButtonState.Pressed : ButtonState.Released
					);
				dPad = new GamePadDPad(
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_DPAD_UP) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_DPAD_DOWN) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_DPAD_LEFT) ? ButtonState.Pressed : ButtonState.Released,
					OuyaSDK.OuyaInput.GetButton((int)playerIndex, OuyaController.BUTTON_DPAD_RIGHT) ? ButtonState.Pressed : ButtonState.Released
					);
				
				thumbSticks = new GamePadThumbSticks(
					new GamePadThumbSticks.StickValue(
					OuyaSDK.OuyaInput.GetAxis((int)playerIndex, OuyaController.AXIS_LS_X),
					OuyaSDK.OuyaInput.GetAxis((int)playerIndex, OuyaController.AXIS_LS_Y)
					),
					new GamePadThumbSticks.StickValue(
					OuyaSDK.OuyaInput.GetAxis((int)playerIndex, OuyaController.AXIS_RS_X),
					OuyaSDK.OuyaInput.GetAxis((int)playerIndex, OuyaController.AXIS_RS_Y)
					)
				);
				triggers = new GamePadTriggers(
					OuyaSDK.OuyaInput.GetAxis((int)playerIndex, OuyaController.AXIS_L2),
					OuyaSDK.OuyaInput.GetAxis((int)playerIndex, OuyaController.AXIS_R2)
					);
			}
		}


		public bool IsConnected
		{
			get { return isConnected; }
		}


		public GamePadButtons Buttons
		{
			get { return buttons; }
		}


		public GamePadDPad DPad
		{
			get { return dPad; }
		}


		public GamePadTriggers Triggers
		{
			get { return triggers; }
		}


		public GamePadThumbSticks ThumbSticks
		{
			get { return thumbSticks; }
		}
	}


	public enum PlayerIndex
	{
		One = 0,
		Two,
		Three,
		Four
	}

/*
	public enum GamePadDeadZone
	{
		Circular,
		IndependentAxes,
		None
	}
*/

	public class GamePad
	{
		public static GamePadState GetState( PlayerIndex playerIndex )
		{
			return new GamePadState( playerIndex );
		}

		/*
		public static GamePadState GetState( PlayerIndex playerIndex, GamePadDeadZone deadZone )
		{
			return new GamePadState(result == Utils.Success, state, deadZone);
		}
		*/
	}
}
#endif

