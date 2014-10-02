using UnityEngine;
using InControl;


namespace InterfaceMovement
{
	public class ButtonManager : MonoBehaviour
	{
		public Button focusedButton;


		void Awake()
		{
			TwoAxisInputControl.StateThreshold = 0.7f;
		}


		void Update()
		{
			// Use last device which provided input.
			var inputDevice = InputManager.ActiveDevice;

			// Move focus with directional inputs.
			if (inputDevice.Direction.Up.WasPressed)
				MoveFocusTo( focusedButton.up );
			
			if (inputDevice.Direction.Down.WasPressed) 
				MoveFocusTo( focusedButton.down );
			
			if (inputDevice.Direction.Left.WasPressed) 
				MoveFocusTo( focusedButton.left );
			
			if (inputDevice.Direction.Right.WasPressed)
				MoveFocusTo( focusedButton.right );
		}
		
		
		void MoveFocusTo( Button newFocusedButton )
		{
			if (newFocusedButton != null)
			{
				focusedButton = newFocusedButton;
			}
		}
	}
}