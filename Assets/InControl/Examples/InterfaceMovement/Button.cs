using System;
using UnityEngine;
using InControl;


namespace InterfaceMovement
{
	public class Button : MonoBehaviour
	{
		public Button up = null;
		public Button down = null;
		public Button left = null;
		public Button right = null;
		Renderer cubeRenderer;


		void Start()
		{
			cubeRenderer = GetComponent<Renderer>();
		}


		void Update()
		{
			// Find out if we're the focused button.
			bool hasFocus = transform.parent.GetComponent<ButtonManager>().focusedButton == this;

			// Fade alpha in and out depending on focus.
			var color = cubeRenderer.material.color;
			color.a = Mathf.MoveTowards( color.a, hasFocus ? 1.0f : 0.5f, Time.deltaTime * 3.0f );
			cubeRenderer.material.color = color;
		}
	}
}

