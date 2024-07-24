using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_line : MonoBehaviour
{
    private TetrisBlock self;
    private TetrisScreen screen;

    public GameObject AudioSourceInstance;
    public AudioClip aclip;

    public void FillLine()
    {
        self = GetComponent<TetrisBlock>();
        screen = self.screen;

        int selfRoundedX = Mathf.RoundToInt(transform.position.x);
        int selfRoundedY = Mathf.RoundToInt(transform.position.y);

        int bottomLeftX = Mathf.RoundToInt(screen.bottomLeft.position.x);
        int bottomLeftY = Mathf.RoundToInt(screen.bottomLeft.position.y);

        for (int i = 0; i <= TetrisScreen.Width - 1; i++)
        {
            if (!screen.grid[i, selfRoundedY - bottomLeftY])
            {
                screen.SpawnCube(i, selfRoundedY - bottomLeftY);
            }
        }

        var bruh = Instantiate(AudioSourceInstance);
        bruh.GetComponent<AudioPlayAndDestroy>().toPlay = aclip;
    }
}
