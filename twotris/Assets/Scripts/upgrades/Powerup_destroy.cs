using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_destroy : MonoBehaviour
{
    private TetrisBlock self;
    private TetrisScreen screen;

    public GameObject ParticleInstance;

    public GameObject AudioSourceInstance;
    public AudioClip explodering;

    public void Destroy()
    {
        self = GetComponent<TetrisBlock>();
        screen = self.screen;
        screen.explosions = screen.explosions + 1;

        int roundedX = Mathf.RoundToInt(transform.position.x);
        int roundedY = Mathf.RoundToInt(transform.position.y);

        for (int i = -2; i <= 2; i++)
        {
            if (screen.DestroyCube(roundedX + i, roundedY))
                Instantiate(ParticleInstance, new Vector3(roundedX + i, roundedY, -1), Quaternion.identity);
        }

        for (int i = -2; i <= 2; i++)
        {
            if (screen.DestroyCube(roundedX, roundedY + i))
                Instantiate(ParticleInstance, new Vector3(roundedX, roundedY + i, -1), Quaternion.identity);
        }


        self.screen.shakeCamera();
        var bruh = Instantiate(AudioSourceInstance);
        bruh.GetComponent<AudioPlayAndDestroy>().toPlay = explodering;
    }
}
