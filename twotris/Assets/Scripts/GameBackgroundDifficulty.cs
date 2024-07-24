using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBackgroundDifficulty : MonoBehaviour
{
	private void Start()
	{
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();

		switch(MenuInformation.Difficulty)
		{
			case 0: //easy
				sRenderer.color = new Color(0.4039215f, 0.5667871f, 0.9254902f);
				break;
			case 1: //medium
				sRenderer.color = new Color(0.4039216f, 0.4156863f, 0.9254902f);
				break;
			case 2: //hard
				sRenderer.color = new Color(0.6400944f, 0.4033464f, 0.8301887f);
				break;
			default:
				sRenderer.color = new Color(0.4039216f, 0.4156863f, 0.9254902f);
				break;
		}

	}
}
