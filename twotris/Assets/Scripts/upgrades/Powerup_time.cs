using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_time : MonoBehaviour
{
    private TetrisBlock self;
    private TetrisScreen screen;

    private GameMaster timerObj;
    private GameMaster timer;

    public GameObject AudioSourceInstance;
    public AudioClip aclip;

    public void removeTime()
    {
        self = GetComponent<TetrisBlock>();
        screen = self.screen;
        timerObj = GameObject.FindObjectOfType<GameMaster>();
        timer = timerObj.GetComponent<GameMaster>();

        if (timer)
		{
            timer.nextScreenClear -= 10f;
            var bruh = Instantiate(AudioSourceInstance);
            bruh.GetComponent<AudioPlayAndDestroy>().toPlay = aclip;
        }
    }
}
