using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitions : MonoBehaviour
{
	private Animator anim;
	private Coroutine transition_;
	private bool transitionRunning = false;

	private void Start()
	{
		anim = GetComponent<Animator>();

		anim.SetTrigger("Fadein");
	}

    private void Awake()
    {
		transitionRunning = false;
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
		if (transitionRunning)
			return;
		transition_ = StartCoroutine(Transition("Tetris"));
	}

	public void GotoTwotorial()
    {
		if (transitionRunning)
			return;
		transition_ = StartCoroutine(Transition("Twotorial"));
	}

	IEnumerator Transition(string newScene)
	{
		transitionRunning = true;

		anim.SetTrigger("Fadeout");

		yield return new WaitForSeconds(0.7f);

		SceneManager.LoadScene(newScene);
	}
}
