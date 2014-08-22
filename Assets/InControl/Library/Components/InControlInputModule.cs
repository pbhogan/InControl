#if UNITY_4_6
using UnityEngine;
using UnityEngine.EventSystems;
using InControl;


namespace InControl
{
	[AddComponentMenu( "Event/InControl Input Module" )]
	public class InControlInputModule : BaseInputModule
	{
		public enum Button : int
		{
			Action1 = InputControlType.Action1,
			Action2 = InputControlType.Action2,
			Action3 = InputControlType.Action3,
			Action4 = InputControlType.Action4
		}


		public Button submitButton = Button.Action1;
		public Button cancelButton = Button.Action2;
		public float analogMoveThreshold = 0.5f;


		protected InControlInputModule()
		{
			TwoAxisInputControl.StateThreshold = analogMoveThreshold;
		}


		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}

			var shouldActivate = false;
			shouldActivate |= SubmitButton.WasPressed;
			shouldActivate |= CancelButton.WasPressed;
			shouldActivate |= Direction.HasChanged;
			return shouldActivate;
		}


		public override void ActivateModule()
		{
			base.ActivateModule();

			var baseEventData = GetBaseEventData();
			eventSystem.SetSelectedGameObject( eventSystem.firstSelectedObject, baseEventData );
		}


		public override void DeactivateModule()
		{
			base.DeactivateModule();

			var baseEventData = GetBaseEventData();
			eventSystem.SetSelectedGameObject( null, baseEventData );
		}


		public override void Process()
		{
			if (!SendButtonEventToSelectedObject())
			{
				SendAnalogEventToSelectedObject();
			}
		}


		bool SendButtonEventToSelectedObject()
		{
			if (eventSystem.currentSelectedObject == null)
			{
				return false;
			}

			var baseEventData = GetBaseEventData();

			if (SubmitButton.WasPressed)
			{
				ExecuteEvents.Execute( eventSystem.currentSelectedObject, baseEventData, ExecuteEvents.submitHandler );
			}
			else
			if (CancelButton.WasPressed)
			{
				ExecuteEvents.Execute( eventSystem.currentSelectedObject, baseEventData, ExecuteEvents.cancelHandler );
			}

			return baseEventData.used;
		}


		bool SendAnalogEventToSelectedObject()
		{
			var axisEventData = GetAxisEventData( 0.0f, 0.0f, 1.0f );

			if (Direction.Left.WasPressed) axisEventData.moveDir = MoveDirection.Left;
			if (Direction.Right.WasPressed) axisEventData.moveDir = MoveDirection.Right;
			if (Direction.Up.WasPressed) axisEventData.moveDir = MoveDirection.Up;
			if (Direction.Down.WasPressed) axisEventData.moveDir = MoveDirection.Down;

			if (axisEventData.moveDir != MoveDirection.None)
			{
				ExecuteEvents.Execute( eventSystem.currentSelectedObject, axisEventData, ExecuteEvents.moveHandler );
			}

			return axisEventData.used;
		}


		InputDevice Device
		{
			get
			{
				return InputManager.ActiveDevice;
			}
		}


		TwoAxisInputControl Direction
		{
			get
			{
				return Device.Direction;
			}
		}


		InputControl SubmitButton
		{
			get
			{
				return Device.GetControl( (InputControlType) submitButton );
			}
		}


		InputControl CancelButton
		{
			get
			{
				return Device.GetControl( (InputControlType) cancelButton );
			}
		}
	}
}
#endif

