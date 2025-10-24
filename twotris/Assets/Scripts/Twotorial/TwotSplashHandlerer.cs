using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwotSplashHandlerer : MonoBehaviour
{
    public TextMesh twotMainSplash;
    public TextMesh twotMainSplash_Shadow;
    [Space]
    public TextMesh twotSecondarySplash;

    public void EditSplashVisibility(bool isFirstVisible, bool isSecondaryVisible)
    {
        twotMainSplash.gameObject.SetActive(isFirstVisible);
        twotMainSplash_Shadow.gameObject.SetActive(isFirstVisible);

        twotSecondarySplash.gameObject.SetActive(isSecondaryVisible);
    }

    public void SetMainSplash(string text)
    {
        twotMainSplash.text = text;
        twotMainSplash_Shadow.text = text;
    }

    public void SetSecondarySplash(string text)
    {
        twotSecondarySplash.text = text;
    }
}
