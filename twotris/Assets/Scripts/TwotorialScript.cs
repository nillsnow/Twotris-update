using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwotorialScript : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject twotorialCanvas;

    [Space(5)]
    public Animator BottomLeftBlockAnimator;
    public Animator TwotoritalMenuAnimator;

    public void GoToMenu()
	{
        mainMenuCanvas.SetActive(true);
        twotorialCanvas.SetActive(false);
        BottomLeftBlockAnimator.SetTrigger("Show");
    }

    public void GoToTwotorial()
	{
        mainMenuCanvas.SetActive(false);
        twotorialCanvas.SetActive(true);
        BottomLeftBlockAnimator.SetTrigger("Hide");
        TwotoritalMenuAnimator.SetTrigger("Transition");
    }
}
