using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimerRoundCounter : MonoBehaviour
{
    public GameMaster gameMaster;
    [Space]
    public TextMesh roundTimer;
    public TextMesh roundTimerShadow;
    [Space]
    public TextMesh roundCounter;

	private void Update()
	{
		if (gameMaster.state == GameMaster.GameStates.GAME_START)
		{
			//start with 0 time remaining
			UpdateRoundTimer(0);

			//update round count
			int maxRounds = gameMaster.curDifficulty.screensClears;
			UpdateRoundNumber(maxRounds, maxRounds);
		}

		if (gameMaster.state == GameMaster.GameStates.GAME_PLAYING)
		{
			//update timer
			float remainingRoundTime = gameMaster.screenClearRemainingTime;
			UpdateRoundTimer(remainingRoundTime);

			//update round counter
			int maxRounds = gameMaster.curDifficulty.screensClears;
			int roundsRemaining = maxRounds - gameMaster.screnClearsDone; ;
			UpdateRoundNumber(roundsRemaining, maxRounds);
		}

		if (gameMaster.state == GameMaster.GameStates.GAME_END)
		{
			//end with 0 trime remaining
			UpdateRoundTimer(0);

			//end with 0 rounds remaining
			int maxRounds = gameMaster.curDifficulty.screensClears;
			UpdateRoundNumber(0, maxRounds);
		}
	}

	public void UpdateRoundTimer(float time)
	{
		int minutes_i = (int)Mathf.Floor(time / 60.0f);
		int seconds_i = (int)Mathf.Floor(time) % 60;

		string minutes = minutes_i.ToString();
		string seconds = seconds_i.ToString();

		//add a zero for better spacing (2:04 instead of 2:4)
		if (seconds_i < 10)
		{
			seconds = "0" + seconds;
		}

		roundTimer.text = minutes + ":" + seconds;
		roundTimerShadow.text = minutes + ":" + seconds;
	}

	public void UpdateRoundNumber(int roundsRemaining, int maxRounds)
	{
		string text = "ROUND: " + roundsRemaining.ToString() + "/" + maxRounds.ToString();

		roundCounter.text = text;
	}
}
