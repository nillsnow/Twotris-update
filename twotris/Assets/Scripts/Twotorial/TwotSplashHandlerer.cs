using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TwotSplashHandlerer : MonoBehaviour
{
    public Animator splashAnimator;
    public TextMeshProUGUI twotMainSplash;
    public TextMeshProUGUI twotSecondarySplash;

    public void EditSplashVisibility(bool isFirstVisible, bool isSecondaryVisible)
    {
        twotMainSplash.gameObject.SetActive(isFirstVisible);
        twotSecondarySplash.gameObject.SetActive(isSecondaryVisible);

        splashAnimator.SetBool("Enabled", isFirstVisible || isSecondaryVisible);
    }

    public void HideSplashGradually()
    {
        splashAnimator.SetBool("Enabled", false);
    }

    public void SetMainSplash(string text)
    {
        twotMainSplash.text = text;
    }

    public void SetSecondarySplash(string text)
    {
        twotSecondarySplash.text = text;
    }
}
