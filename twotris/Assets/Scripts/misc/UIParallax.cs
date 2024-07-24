using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParallax : MonoBehaviour
{
    public float moveModifier;

    [Space(5)]
    public bool isInCanvas;

    private RectTransform rTransform;
    private Vector3 cStartPos;

    private Vector3 pz;
    private Vector3 StartPos;

    // Use this for initialization
    void Start()
    {
        rTransform = GetComponent<RectTransform>();

        if (rTransform)
            cStartPos = rTransform.anchoredPosition;

        StartPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		pz.z = 0;
        if (isInCanvas)
		{
            rTransform.anchoredPosition = new Vector3(
                cStartPos.x + (pz.x * moveModifier),
                cStartPos.y + (pz.y * moveModifier), 0);
        }
        else
		{
            gameObject.transform.position = pz;
            //Debug.Log("Mouse Position: " + pz);

            transform.position = new Vector3(StartPos.x + (pz.x * moveModifier), StartPos.y + (pz.y * moveModifier), 0);
            //move based on the starting position and its modified value.
        }
    }
}
