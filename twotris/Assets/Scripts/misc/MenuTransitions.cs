using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitions : MonoBehaviour
{
	private Animator anim;
	private Coroutine transition_;

	private void Start()
	{
		anim = GetComponent<Animator>();

		anim.SetTrigger("Fadein");
	}


	public void QuitGame()
	{
		Application.Quit();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			QuitGame();
	}

	public void GotoGame()
	{
		transition_ = StartCoroutine(Transition());
	}

	IEnumerator Transition()
	{
		anim.SetTrigger("Fadeout");

		yield return new WaitForSeconds(0.7f);

		SceneManager.LoadScene("Tetris");
	}
}
