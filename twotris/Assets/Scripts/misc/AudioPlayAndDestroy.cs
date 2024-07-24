using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayAndDestroy : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip toPlay;

    public bool isEnabled;

    void Start()
    {
        if (!isEnabled)
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();

        audioSource.clip = toPlay;

        audioSource.Play();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
            Destroy(gameObject);
    }
}
