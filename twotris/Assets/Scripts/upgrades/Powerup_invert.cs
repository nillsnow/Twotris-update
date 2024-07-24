using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_invert : MonoBehaviour
{
    private TetrisBlock self;
    private TetrisScreen screen;

    public GameObject AudioSourceInstance;
    public AudioClip aclip;

    public void InvertOppositeScreen()
    {
        self = GetComponent<TetrisBlock>();

        self.screen.middle.GetOppositeScreen(self.screen).isInverted = true;

        self.screen.shakeCamera();
        var bruh = Instantiate(AudioSourceInstance);
        bruh.GetComponent<AudioPlayAndDestroy>().toPlay = aclip;
    }
}
