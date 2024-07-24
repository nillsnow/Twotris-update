using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public bool isEnabled;
    public float period = 1.5f;

    [Space(5)]
    public TextMeshProUGUI text;

    public UnityEvent onTimeout;

    [HideInInspector]
    public float nextActionTime = 0.0f;

    [HideInInspector]
    public float remainingTime;

    private void Update()
    {
        if (text)
            text.text = Mathf.RoundToInt(remainingTime).ToString();

        remainingTime = nextActionTime - Time.time;

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            if (isEnabled)
                onTimeout.Invoke();
        }
    }
}
