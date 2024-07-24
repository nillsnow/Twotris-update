using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyUpdate : MonoBehaviour
{
    public TMP_Dropdown dropdown;

	private void Start()
	{
		UpdateDifficulty();
	}

	public void UpdateDifficulty()
    {
        MenuInformation.Difficulty = dropdown.value;
    }
}
