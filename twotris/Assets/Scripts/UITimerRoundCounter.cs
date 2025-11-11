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
		int maxRounds;
		float remainingRoundTime;
		int roundsRemaining;

		switch (gameMaster.state)
        {
			case GameMaster.GameStates.GAME_START:
				//start with 0 time remaining
				UpdateRoundTimer(0);

				//update round count
				maxRounds = gameMaster.curDifficulty.screensClears;
				UpdateRoundNumber(maxRounds, maxRounds);
				break;
			case GameMaster.GameStates.GAME_PLAYING:
				//update timer
				remainingRoundTime = gameMaster.screenClearRemainingTime;
				UpdateRoundTimer(remainingRoundTime);

				//update round counter
				maxRounds = gameMaster.curDifficulty.screensClears;
				roundsRemaining = maxRounds - gameMaster.screnClearsDone;
				UpdateRoundNumber(roundsRemaining, maxRounds);
				break;
			case GameMaster.GameStates.GAME_END:
				//end with 0 trime remaining
				UpdateRoundTimer(0);

				//end with 0 rounds remaining
				maxRounds = gameMaster.curDifficulty.screensClears;
				UpdateRoundNumber(0, maxRounds);
				break;


			case GameMaster.GameStates.GAME_NONE:
				ToggleTextVisibility(false, false);
				break;


			/*case GameMaster.GameStates.TWOTORIAL_COUNTDOWN:
				ToggleTextVisibility(true, false);

				//update timer
				remainingRoundTime = gameMaster.screenClearRemainingTime;
				UpdateRoundTimer(remainingRoundTime);
				break;*/
			case GameMaster.GameStates.TWOTORIAL_ROUNDS:
				ToggleTextVisibility(true, true);
				//update timer
				remainingRoundTime = gameMaster.screenClearRemainingTime;
				UpdateRoundTimer(remainingRoundTime);

				//update round counter
				maxRounds = gameMaster.curDifficulty.screensClears;
				roundsRemaining = maxRounds - gameMaster.screnClearsDone;
				UpdateRoundNumber(roundsRemaining, maxRounds);
				break;

		}
	}

	public void ToggleTextVisibility(bool timerVisibility, bool counterVisibility)
	{
		roundTimer.gameObject.SetActive(timerVisibility);
		roundTimerShadow.gameObject.SetActive(timerVisibility);
		roundCounter.gameObject.SetActive(counterVisibility);
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
