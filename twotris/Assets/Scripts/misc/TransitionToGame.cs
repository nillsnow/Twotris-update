using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToGame : MonoBehaviour
{
    private Animator anim;
	private Coroutine transition_;

	private void Start()
	{
		anim = GetComponent<Animator>();

		anim.SetTrigger("WipeIn");
	}

	public void GotoMenu()
	{
		transition_ = StartCoroutine(Transition());
	}

	IEnumerator Transition()
	{
		anim.SetTrigger("WipeOut");

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene("Menu");
	}
}
