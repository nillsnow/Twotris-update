using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	private Animator anim;

	public GameMaster gm;

	public GameObject blackOverlay;

	[Header("DO NOT TOUCH!")]
	public bool paused;

	private void Start()
	{
		//transform.GetChild(0).gameObject.SetActive(false); //turn pause menu off at start
		anim = GetComponent<Animator>();

		StartCoroutine(FuckYouUnity());
	}

	IEnumerator FuckYouUnity()
	{
		yield return new WaitForSeconds(0.1f);
		blackOverlay.SetActive(false);
		blackOverlay.transform.parent.gameObject.SetActive(false); //fix FOR PAUSE MENU FLASHING
		yield break;
	}

	public void Pause()
	{
		Time.timeScale = 0;
		paused = true;
		anim.SetBool("Paused", paused);
	}

	public void UnPause()
	{
		paused = false;
		anim.SetBool("Paused", paused);
		Time.timeScale = 1;
	}

	public void GotoMenu()
	{
		paused = false;
		anim.SetBool("Paused", paused);
		Time.timeScale = 1;
		gm.transitionManager.GotoMenu();
	}
}
