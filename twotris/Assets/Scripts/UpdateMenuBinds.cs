using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateMenuBinds : MonoBehaviour
{
	public TMP_Dropdown dropdown;

	[Space(5)]

	public CanvasKeybind up;
	public CanvasKeybind down;
	public CanvasKeybind left;
	public CanvasKeybind right;

	[Space(5)]
	public TMP_InputField leftInput;
	public TMP_InputField rightInput;

	private void Start()
	{
		//wasd is default and wont be changing (i think)
		MenuInformation.lUp = KeyCode.W;
		MenuInformation.lDown = KeyCode.S;
		MenuInformation.lLeft = KeyCode.A;
		MenuInformation.lRight = KeyCode.D;

		UpdateBindings();
		UpdateNames();
	}

	public void UpdateNames()
	{
		MenuInformation.leftName = leftInput.text;
		MenuInformation.rightName = rightInput.text;
	}

	public void UpdateBindings()
	{
		if (dropdown.value == 0) // arrows
		{
			MenuInformation.rUp = KeyCode.UpArrow;
			up.UpdateString(MenuInformation.rUp);

			MenuInformation.rDown = KeyCode.DownArrow;
			down.UpdateString(MenuInformation.rDown);

			MenuInformation.rLeft = KeyCode.LeftArrow;
			left.UpdateString(MenuInformation.rLeft);

			MenuInformation.rRight = KeyCode.RightArrow;
			right.UpdateString(MenuInformation.rRight);
		}
		else if (dropdown.value == 1) //ijkl
		{
			MenuInformation.rUp = KeyCode.I;
			up.UpdateString(MenuInformation.rUp);

			MenuInformation.rDown = KeyCode.K;
			down.UpdateString(MenuInformation.rDown);

			MenuInformation.rLeft = KeyCode.J;
			left.UpdateString(MenuInformation.rLeft);

			MenuInformation.rRight = KeyCode.L;
			right.UpdateString(MenuInformation.rRight);
		}
	}
}
