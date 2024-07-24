using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    private TextMeshProUGUI self;
    public TextMeshProUGUI secondtext;
    public TextMeshProUGUI startCountdown;
    public GameMaster gm;

    private void Start()
    {
        self = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (gm.state == GameMaster.GameStates.GAME_PLAYING)
        {
            self.text = Mathf.RoundToInt(gm.nextScreenClear - Time.time).ToString();
            secondtext.text = (gm.difficulties[gm.currentDifficultyIndex].screensClears - gm.screnClearsDone).ToString();
        }
        else
        {
            self.text = "Time";
            secondtext.text = "Rounds";
        }

        startCountdown.gameObject.SetActive(gm.state == GameMaster.GameStates.GAME_START);
        if (startCountdown.gameObject.activeSelf)
        {
            startCountdown.text = (gm.startCountdown + 1).ToString();
        }
    }
}
