using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundEffects : MonoBehaviour
{
    public AudioSource click1;
    public AudioSource click2;
    public AudioSource click3;

    public void PlayClick1()
	{
        click1.Play();
	}
    public void PlayClick2()
    {
        click2.Play();
    }
    public void PlayClick3()
    {
        click3.Play();
    }
}
