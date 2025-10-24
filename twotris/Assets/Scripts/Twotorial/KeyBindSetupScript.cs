using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindSetupScript : MonoBehaviour
{
    public bool StartEnabled;

    [Space(10)]
    public CanvasKeybind rightUp;
    public CanvasKeybind rightDown;
    public CanvasKeybind rightLeft;
    public CanvasKeybind rightRight;

    [Space(10)]
    public MiddlePart middlePart;

    [Space(10)]
    public GameObject leftBinds;
    public GameObject rightBinds;

    private void Start()
    {
        SetKeybindVisibility(StartEnabled);
    }

    public void UpdateKeybinds()
    {
        if (middlePart.sRight == null)
            return;

        rightUp.bind = middlePart.sRight.keyUp;
        rightDown.bind = middlePart.sRight.keyDown;
        rightLeft.bind = middlePart.sRight.keyLeft;
        rightRight.bind = middlePart.sRight.keyRight;
    }

    public void SetKeybindVisibility(bool isVisible)
    {
        leftBinds.SetActive(isVisible);
        rightBinds.SetActive(isVisible);
    }
}
