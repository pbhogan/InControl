using UnityEngine;


namespace InterfaceMovement
{
	public class ButtonFocus : MonoBehaviour
	{
		void Update()
		{
			// Get focused button.
			var focusedButton = transform.parent.GetComponent<ButtonManager>().focusedButton;

			// Move toward same position as focused button.
			transform.position = Vector3.MoveTowards( transform.position, focusedButton.transform.position, Time.deltaTime * 10.0f );
		}
	}
}
