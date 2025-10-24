using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Events;

public class TwotorialMaster : MonoBehaviour
{
    public TwotSplashHandlerer splashHandlerer;
    public TextMeshProUGUI debugStepUI;

    [System.Serializable]
    public class TwotorialGameObject
    {
        public string name;
        public GameObject gameObject;
    }

    [System.Serializable]
    public class TwotorialInt
    {
        public string name;
        public int Int;
    }

    [System.Serializable]
    public class TwotorialStep
    {
        [Header("##############################################")]
        public string stepName;

        [Header("Splashes")]
        [Space(20)]
        public bool mainSplashEnabled;
        [TextArea(1, 3)]
        public string mainSplashText;
        [Space(10)]
        public bool secondarySplashEnabled;
        [TextArea(1, 2)]
        public string secondarySplashText;

        [Header("Event")]
        public UnityEvent stepEvent;

        [Header("Continuity")]
        [Space(20)]
        public bool stepTimedEnabled;
        public float stepLength;
        [Space(10)]
        public bool evenetEndEnabled;

        [Header("Includes")]
        public MiddlePart.tetrominoes spawnOnlyTetrominoe = null;
        public MiddlePart.powerups spawnOnlyPowerup = null;
        public TwotorialGameObject[] includedGameObjects;
        public TwotorialInt[] includedInts;
    }

    [Space(5)]
    public TwotorialStep[] twotorialSteps;
    public int twotCurrentStepNumber;


    private GameMaster gameMaster;

    private TwotorialStep twotCurrentStep;

    private Coroutine runStep;
    private bool isRunningStep = false;

    private float nextTetrominoSpawn = 0;

    public void TwotorialSetup(GameMaster gm)
    {
        Debug.Log("Twotorial master reponded");

        gameMaster = gm;

        SetCurrentStep(0);
    }

    public void EvaluateStep(int stepNumber)
    {
        twotCurrentStep.stepEvent.Invoke();

        switch (stepNumber)
        {
            case 0:
                //nothing special
                break;
            case 1:
                twotCurrentStep.includedGameObjects[0].gameObject.GetComponent<KeyBindSetupScript>().SetKeybindVisibility(true);
                break;
            case 2:
                twotCurrentStep.includedGameObjects[0].gameObject.GetComponent<KeyBindSetupScript>().SetKeybindVisibility(false);
                gameMaster.state = GameMaster.GameStates.TWOTORIAL_PLAYING;
                break;
            case 3:
                gameMaster.state = GameMaster.GameStates.TWOTORIAL_SPECIFIC_PIECE;
                break;
            case 4:
                gameMaster.state = GameMaster.GameStates.TWOTORIAL_SPECIFIC_PIECE;
                break;
            case 5:
                gameMaster.StartTicking();
                gameMaster.state = GameMaster.GameStates.TWOTORIAL_COUNTDOWN;
                break;
            case 6:
            case 7:
                gameMaster.state = GameMaster.GameStates.TWOTORIAL_ROUNDS;
                break;
            case 8:
                gameMaster.EndTicking();
                gameMaster.state = GameMaster.GameStates.GAME_PAUSED;
                gameMaster.middle.CalculateScoreClear();

                foreach (TetrisBlock block in GameObject.FindObjectsOfType<TetrisBlock>())
                {
                    if (block.enabled)
                    {
                        block.addToGrid(true);
                        block.enabled = false;
                    }
                }

                break;
            case 10:
                foreach (GameObject popup in GameObject.FindGameObjectsWithTag("PopUp"))
                {
                    popup.GetComponent<Animator>().SetBool("Staying", false);
                }
                gameMaster.middle.sLeft.clearBlocks();
                gameMaster.middle.sRight.clearBlocks();

                //fill first three layers with empty blocks 
                for (int h = 0; h < 5; h++)
                {
                    for (int w = 0; w < TetrisScreen.Width; w++)
                    {
                        gameMaster.middle.sLeft.SpawnCube(w, h);
                        gameMaster.middle.sRight.SpawnCube(w, h);
                    }
                }

                gameMaster.state = GameMaster.GameStates.TWOTORIAL_PLAYING;
                break;
            case 11:
                gameMaster.state = GameMaster.GameStates.GAME_PAUSED;
                break;
            case 12:
                gameMaster.ResetRoundTime();

                //reset boards without counting up score
                gameMaster.middle.sLeft.clearBlocks();
                gameMaster.middle.sRight.clearBlocks();
                gameMaster.middle.sLeft.isDead = false;
                gameMaster.middle.sRight.isDead = false;
                gameMaster.middle.sLeft.hasActiveBlock = false;
                gameMaster.middle.sRight.hasActiveBlock = false;

                gameMaster.twotorialEnabled = false;

                gameMaster.state = GameMaster.GameStates.GAME_PLAYING;
                break;
            default:
                break;
        }

        if (isRunningStep)
        {
            isRunningStep = false;
            StopCoroutine(runStep);
        }

        isRunningStep = true;
        runStep = StartCoroutine(RunStep());
    }

    private void Update()
    {
        if (twotCurrentStep.spawnOnlyTetrominoe.shape != null)
        {
            if ((Time.time + 1) > nextTetrominoSpawn) //spawning tetrominos
            {
                nextTetrominoSpawn = (Time.time + 1) + gameMaster.curDifficulty.tetrominoSpawn + (UnityEngine.Random.value - 0.5f);
                gameMaster.middle.spawnSpecificPiece(twotCurrentStep.spawnOnlyTetrominoe);
            }
        }

        if (twotCurrentStep.spawnOnlyPowerup.powerup != null)
        {
            if (Time.time > nextTetrominoSpawn) //spawning tetrominos
            {
                nextTetrominoSpawn = Time.time + gameMaster.curDifficulty.tetrominoSpawn + (UnityEngine.Random.value - 0.5f);
                gameMaster.middle.spawnSpecificPowerup(twotCurrentStep.spawnOnlyPowerup);
            }
        }

        switch (twotCurrentStepNumber)
        {
            case 2:
                TetrisScreen[] screens =
                {
                    gameMaster.middle.sLeft,
                    gameMaster.middle.sRight
                };

                int checkedHeight = twotCurrentStep.includedInts[0].Int;
                
                for (int b = 0; b < 2; b++)
                {
                    for (int i = 0; i < TetrisScreen.Width; i++)
                    {
                        if (screens[b].grid[i, checkedHeight] != null)
                        {
                            Debug.Log((b == 0 ? "Left" : "Right") + " - Hit on line " + checkedHeight);
                            EndCurrentStep();
                        }
                    }
                }
                break;
            case 3:
                if (gameMaster.middle.sLeft.explosions >= twotCurrentStep.includedInts[0].Int)
                    EndCurrentStep();
                if (gameMaster.middle.sRight.explosions >= twotCurrentStep.includedInts[0].Int)
                    EndCurrentStep();
                break;
            case 4:
                if (gameMaster.middle.sLeft.linefilles >= twotCurrentStep.includedInts[0].Int)
                    EndCurrentStep();
                if (gameMaster.middle.sRight.linefilles >= twotCurrentStep.includedInts[0].Int)
                    EndCurrentStep();
                break;
            case 7:
                if (gameMaster.screnClearsDone > 0) //first screenclear
                    EndCurrentStep();
                break;
            case 10:
                if (gameMaster.middle.sLeft.isDead || gameMaster.middle.sLeft.isDead)
                    EndCurrentStep();
                break;
            default:
                break;
        }
    }

    IEnumerator RunStep()
    {
        splashHandlerer.EditSplashVisibility(twotCurrentStep.mainSplashEnabled, twotCurrentStep.secondarySplashEnabled);
        splashHandlerer.SetMainSplash(twotCurrentStep.mainSplashText);
        splashHandlerer.SetSecondarySplash(twotCurrentStep.secondarySplashText);

        if (twotCurrentStep.stepTimedEnabled)
        {
            float totalTime = twotCurrentStep.stepLength;

            while (totalTime > 0)
            {
                totalTime -= Time.deltaTime;

                if (!isRunningStep)
                {
                    AdvanceStep();
                    yield break;
                }

                UpdateDebugString(twotCurrentStep, totalTime);
                yield return null;
            }

            AdvanceStep();
            yield break;
        }

        if (twotCurrentStep.evenetEndEnabled)
        {
            while (isRunningStep)
                yield return null;

            Debug.Log("Event triggered!");

            AdvanceStep();
            yield break;
        }

        Debug.LogWarning("Got to end of RunStep – did you select an ending method?");
        yield break;
    }

    private void SetCurrentStep(int number)
    {
        if (number >= twotorialSteps.Length - 1)
            number = twotorialSteps.Length - 1;

        if (number < 0)
            number = 0;

        twotCurrentStepNumber = number;
        twotCurrentStep = twotorialSteps[number];

        //###DEBUG INFORMATION
        UpdateDebugString(twotCurrentStep);

        EvaluateStep(number);
    }

    public void AdvanceStep()
    {
        if (twotCurrentStepNumber == twotorialSteps.Length - 1)
            return;

        if (isRunningStep)
        {
            StopCoroutine(runStep);
            isRunningStep = false;
        }

        int nextNum = twotCurrentStepNumber + 1;
        SetCurrentStep(nextNum);
    }

    public void EndCurrentStep()
    {
        if (!twotCurrentStep.evenetEndEnabled)
            return;

        isRunningStep = false;
    }

    private void UpdateDebugString(TwotorialStep step, float remainingTime = 999)
    {
        string debugInfo = step.stepName + "\n";

        float time = step.stepLength;
        if (remainingTime != 999)
            time = remainingTime;

        if (step.stepTimedEnabled)
            debugInfo = debugInfo + Math.Round(time, 2).ToString() + "s\n";

        if (step.evenetEndEnabled)
            debugInfo = debugInfo + "Event triggered";

        debugStepUI.text = debugInfo;
    }
}
