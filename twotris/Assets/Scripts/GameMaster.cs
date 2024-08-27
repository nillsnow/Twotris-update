using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public MiddlePart middle;
    public Camera cam;

    [System.Serializable] //difficulty
    public class Difficulty
    {
        public string difficultyName;

        [Space(5)]
        public float screenClearDelay = 30f; //
        public int screensClears = 10; //

        [Space(5)]
        [Range(0.1f, 20f)]
        public float tetrominoSpawn = 0.1f; //
        [Range(0.1f, 20f)]
        public float tetrominoGravity = 0.1f; //
        [Range(0.1f, 20f)]
        public float powerupGravity = 2f;
        [Range(0.1f, 20f)]
        public float tetrominoFallSpeed = 0.1f; //
        [Range(0.1f, 20f)]
        public float grabbySpeed = 0.1f; //
        

        [Space(5)]
        public int scorePerBlock = 4;
        public int scorePerLine = 100;
    }
    public Difficulty[] difficulties;

    [Space(10)]
    public int currentDifficultyIndex;

    public enum GameStates {
        GAME_NONE,      //before start
        GAME_START,     //start countdown
        GAME_PLAYING,   //self descriptive
        GAME_PAUSED,    //self descriptive
        GAME_END,       //self descriptive
    };

    [HideInInspector]
    public float nextTetrominoSpawn = 0.0f;
    [HideInInspector]
    public float nextScreenClear = 0.0f;
    [HideInInspector]
    public float screenClearRemainingTime = 0.0f;

    [Header("Debug")]
    [Space(20)]
    //[HideInInspector]
    public int screnClearsDone;
    public GameStates state;
    public GameObject splashText;
    public GameObject splashTextShadow;
    public TransitionToGame transitionManager;
    public PauseMenu pause;

    [Space(5)]
    public string leftName;
    public string rightName;

    [HideInInspector]
    public int startCountdown;
    private Coroutine startCountdown_;
    private Coroutine endScreen_;

    [HideInInspector]
    public Difficulty curDifficulty;

    [Space(5)]
    public AudioSource countdownWhoosh;
    public AudioSource tickAudio;
    public AudioSource tickAudio2;
    private Coroutine tickingCoroutine;

    private void Start()
    {
        state = GameStates.GAME_START;
        currentDifficultyIndex = MenuInformation.Difficulty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && state != GameStates.GAME_END) //pause menu
            if (pause.paused)
                pause.UnPause();
            else
                pause.Pause();

        switch (state)
        {
            case GameStates.GAME_NONE: //debug purposes
                break;
            case GameStates.GAME_START: //game START--------------------
                tickingCoroutine = null;

                curDifficulty = difficulties[currentDifficultyIndex];
                screnClearsDone = 0;

                middle.gravityScale = curDifficulty.tetrominoGravity;
                middle.gravityScalePowerup = curDifficulty.powerupGravity;

                //grabby speed
                middle.pLeft.speed = curDifficulty.grabbySpeed;
                middle.pRight.speed = curDifficulty.grabbySpeed;

                //fall time
                middle.sLeft.fallTime = curDifficulty.tetrominoFallSpeed;
                middle.sRight.fallTime = curDifficulty.tetrominoFallSpeed;

                //score
                middle.sLeft.scorePerLine = curDifficulty.scorePerLine;
                middle.sLeft.scorePerBlock = curDifficulty.scorePerBlock;

                middle.sRight.scorePerLine = curDifficulty.scorePerLine;
                middle.sRight.scorePerBlock = curDifficulty.scorePerBlock;

                nextScreenClear = Time.time + curDifficulty.screenClearDelay;

                //bindings
                if (MenuInformation.lUp == KeyCode.None) //if we didnt start from menu (for debug)
				{
                    middle.sLeft.keyUp      = KeyCode.W;
                    middle.sLeft.keyDown    = KeyCode.S;
                    middle.sLeft.keyLeft    = KeyCode.A;
                    middle.sLeft.keyRight   = KeyCode.D;

                    middle.sRight.keyUp     = KeyCode.UpArrow;
                    middle.sRight.keyDown   = KeyCode.DownArrow;
                    middle.sRight.keyLeft   = KeyCode.LeftArrow;
                    middle.sRight.keyRight  = KeyCode.RightArrow;
                }
                else
				{
                    middle.sLeft.keyUp      = MenuInformation.lUp;
                    middle.sLeft.keyDown    = MenuInformation.lDown;
                    middle.sLeft.keyLeft    = MenuInformation.lLeft;
                    middle.sLeft.keyRight   = MenuInformation.lRight;

                    middle.sRight.keyUp     = MenuInformation.rUp;
                    middle.sRight.keyDown   = MenuInformation.rDown;
                    middle.sRight.keyLeft   = MenuInformation.rLeft;
                    middle.sRight.keyRight  = MenuInformation.rRight;
				}

                //player names
                leftName = MenuInformation.leftName;
                rightName = MenuInformation.rightName;

                if (startCountdown_ == null)
                    startCountdown_ = StartCoroutine(Countdown());
                break;
            case GameStates.GAME_PLAYING: //game PLAYING--------------------
                if (tickingCoroutine == null)
                    tickingCoroutine = StartCoroutine(CountdownTicking());

                if (Time.time > nextTetrominoSpawn) //spawning tetrominos
                {
                    nextTetrominoSpawn = Time.time + curDifficulty.tetrominoSpawn + (Random.value - 0.5f);
                    middle.spawnPiece();
                    if (Random.value > 0.8) //special powerups based on score
                        middle.spawnPieceSide(); 
                }

                if (Time.time > nextScreenClear) //clearing screen
                {
                    nextScreenClear = Time.time + curDifficulty.screenClearDelay;
                    middle.CalculateScoreClear();
                    screnClearsDone++;
                }

                screenClearRemainingTime = nextScreenClear - Time.time;

                if (screnClearsDone >= curDifficulty.screensClears)
                    state = GameStates.GAME_END; //end game if screen clears
                break;
            case GameStates.GAME_PAUSED: //game PAUSED--------------------
                break;
            case GameStates.GAME_END: //game END--------------------

                StopCoroutine(tickingCoroutine);

                if (endScreen_ == null)
                    endScreen_ = StartCoroutine(EndScreen());

                if (Input.GetKeyDown(KeyCode.Escape))
                    transitionManager.GotoMenu();
                break;
            default:
                break;
        }
    }

    void SetSplashEnable(bool val)
	{
        splashText.SetActive(val);
        splashTextShadow.SetActive(val);
    }

	void SetSplashText(string text)
	{
        splashText.GetComponent<TextMesh>().text = text;
        splashTextShadow.GetComponent<TextMesh>().text = text;
    }

    void SetSplashSize(int newSize)
	{
        splashText.GetComponent<TextMesh>().fontSize = newSize;
        splashTextShadow.GetComponent<TextMesh>().fontSize = newSize;
    }

    private bool lastTick;

    IEnumerator CountdownTicking()
	{
        while (true)
		{
            WaitForSeconds wait = new WaitForSeconds(0.3f); //tickAudio

            if (nextScreenClear - Time.time < 10f)
			{
                wait = new WaitForSeconds(0.15f);
            }

            if (lastTick)
			{
                tickAudio.Play();
                lastTick = false;
            }
            else
			{
                tickAudio2.Play();
                lastTick = true;
            }

            yield return wait;
        }
	}

	IEnumerator Countdown()
    {
        startCountdown = 3;
        SetSplashEnable(false); //disable while transition hapenning

        float defCam = cam.orthographicSize;
        cam.orthographicSize = defCam + startCountdown * 0.5f;

        yield return new WaitForSeconds(1f); //enough time for a transition

        SetSplashEnable(true); //show countdown text
        SetSplashSize(230);

        while (startCountdown != 0)
        {
            cam.orthographicSize = defCam + startCountdown * 0.5f;
            countdownWhoosh.Play();
            splashText.GetComponent<Animator>().SetTrigger("Pulse"); //countdown animation
            SetSplashText(startCountdown.ToString());

            yield return new WaitForSeconds(1f);
            startCountdown--;
        }

        state = GameStates.GAME_PLAYING;
        cam.orthographicSize = defCam;
        //final throb is GO!
        SetSplashText("GO!");
        countdownWhoosh.Play();
        splashText.GetComponent<Animator>().SetTrigger("PulseFadeout");

        yield return new WaitForSeconds(1f);

        SetSplashEnable(false);
    }

    IEnumerator EndScreen()
	{
        yield return new WaitForSeconds(0.2f);

        SetSplashEnable(true);
        SetSplashText("GAME");

        splashText.GetComponent<Animator>().SetTrigger("Pulse");

        yield return new WaitForSeconds(3f);

        splashText.GetComponent<Animator>().SetTrigger("EndScreen");

        //show which side won
        string lname = (leftName == "" || leftName == null) ? "Left" : leftName;
        string rname = (rightName == "" || rightName == null) ? "Right" : rightName;

        string winner = (middle.sLeft.score > middle.sRight.score) ? lname : rname;
        string loser = (middle.sLeft.score < middle.sRight.score) ? lname : rname;

        SetSplashSize(150);

        while (!Input.GetKeyDown(KeyCode.Escape))
		{
            SetSplashSize(150);
            SetSplashText(winner + " won!");

            yield return new WaitForSeconds(5f);

            SetSplashSize(120);
            SetSplashText(loser + " was close though!");

            yield return new WaitForSeconds(3f);
        }            

    }
}
