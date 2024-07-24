using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasKeybind : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public KeyCode bind = KeyCode.None;

	public Image keyCapBg;
	public Image keyCapImage;

	private void Start()
	{
		if (bind != KeyCode.None)
			UpdateString(bind);
	}

	private void Update()
	{
		if (Input.GetKey(bind))
		{
			keyCapImage.color = new Color(0.6415094f, 0.6415094f, 0.6415094f);
			keyCapBg.color = new Color(0.735849f, 0.735849f, 0.735849f);
		}
		else
		{
			keyCapImage.color = new Color(0.8301887f, 0.8301887f, 0.8301887f); 
			keyCapBg.color = new Color(0.9528302f, 0.9528302f, 0.9528302f);  
		}
	}

	public void UpdateString(KeyCode newBind)
	{
		bind = newBind;

		string text = newBind.ToString().ToLower();

		switch (text)
		{
			case "uparrow":
				text = "↑";
				break;
			case "downarrow":
				text = "↓";
				break;
			case "rightarrow":
				text = "→";
				break;
			case "leftarrow":
				text = "←";
				break;
			default:
				break;
		}

		textField.text = text;
	}
}
