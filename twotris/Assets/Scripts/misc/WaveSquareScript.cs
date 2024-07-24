using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSquareScript : MonoBehaviour
{
	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();	
	}

	public void Disappear()
	{
		anim.SetTrigger("Disappear");
	}

	public void DestroySelf()
	{
		Destroy(gameObject);
	}
}
