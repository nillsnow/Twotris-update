using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class TetrisScreen : MonoBehaviour
{
    //[HideInInspector]
    [Header("debug")]
    public int score;
    public int curScore;
    public bool isInverted;

    [HideInInspector]
    public int scorePerBlock;
    [HideInInspector]
    public int scorePerLine;

    public bool hasActiveBlock;
    public bool calcScore;
    [Space(5)]
    public bool isDead;

    public LayerMask blockLayerMask;

    [Header("Spawning Tetrominos")]
    public GameObject nextToSpawn;
    public Transform spawnPoint;

    [Header("Bottom left position of the screen")]
    public Transform bottomLeft;

    [Header("Tetromino settings")]
    public KeyCode keyUp;
    public KeyCode keyLeft;
    public KeyCode keyDown;
    public KeyCode keyRight;

    [HideInInspector]
    public float fallTime = 1.2f;

    private TetrisBlock itemT;

    [Space(10)]
    public static int Width = 7;
    public static int Height = 12;

    public Transform[,] grid = new Transform[Width, Height];

    public GameObject Square;
    public GameObject PopUp;

    [Space(10)]
    public Shaker CamShaker;
    public ShakePreset CamShakePreset;
    [Space(5)]
    public GameObject lineParticleConfetti;
    public GameObject AudioSourceInstance;
    public AudioClip pop1;
    public AudioClip pop2;
    public AudioClip thud;
    public AudioClip thud2;
    public AudioClip grabTetromino;
    [Space(5)]
    public TextMesh scoreText;
    private int scoreNumber;

    public GameObject waveBackground;
    public GameMaster gameMaster;

    [Space(5)]
    public WaveCounterScreen waveCounter;

    [Space(5)]
    public Animator overflowAnimator;

    [HideInInspector]
    public bool wasJustPlaced;

    public MiddlePart middle;

	private void Start()
	{
        waveCounter.gameMaster = gameMaster;
    }

	void Update()
    {
        overflowAnimator.SetBool("Overflow", isDead);

        if (scoreText)
            scoreText.text = scoreNumber.ToString("D5");

        if (waveBackground)
		{
            Vector3 pos = waveBackground.transform.position;

            float desiredHeight = 0f;
            if (gameMaster.state == GameMaster.GameStates.GAME_PLAYING)
                desiredHeight = (1 - (gameMaster.nextScreenClear - Time.time) / gameMaster.curDifficulty.screenClearDelay) * 12.5f;

            float height = desiredHeight - pos.y;

            waveBackground.transform.position = new Vector3(pos.x, pos.y + height * Time.deltaTime * 7f); //7f being how fast i want it to be
        }


        if (nextToSpawn)
        {
            if (isDead/* || hasActiveBlock*/)
            {
                nextToSpawn = null;
                return;
            }

            GameObject item = Instantiate(nextToSpawn, spawnPoint.position, Quaternion.identity);
            item.transform.SetParent(spawnPoint);

            itemT = item.GetComponent<TetrisBlock>();
            itemT.screen = this;

            itemT.previousTime = Time.time;

            var a = Instantiate(AudioSourceInstance);
            a.GetComponent<AudioPlayAndDestroy>().toPlay = grabTetromino;

            nextToSpawn = null;
        }

        if (itemT)
            hasActiveBlock = itemT.enabled;

        if (calcScore)
        {
            calculateScoreClear();
            calcScore = false;
        }
    }

    public void calculateScoreClear()
    {
        if (isDead)
        {
            clearBlocks();
            isDead = false;
            hasActiveBlock = false;
            return;
        }

        calculateScore(true);
    }

    int blocks;
    int lines;
    public void calculateScore(bool spawnPopUps = false)
    {
        lines = 0;
        blocks = 0;

        if (spawnPopUps)
            StartCoroutine(CalculateBlocksDelayed());
        else
            CalculateBlocks();
    }
    IEnumerator CalculateBlocksDelayed() //cool wave effect for popups
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);

        //grid = new Transform[Width, Height];

        //calculate lines
        for (int i = Height - 1; i >= 0; i--)
        {
            if (hasLine(i, grid))
            {
                if (bottomLeft.position.x < 6) //if screen on right, display score to the left of it
                {
                    Instantiate(lineParticleConfetti, new Vector3(bottomLeft.position.x + Width + 0.5f, bottomLeft.position.y + i, -1), Quaternion.identity);
                    SpawnScorePopUp(scorePerLine.ToString(), new Vector3(bottomLeft.position.x + Width + 0.5f, bottomLeft.position.y + i), 1);
                }
                else
				{
                    Instantiate(lineParticleConfetti, new Vector3(bottomLeft.position.x - 1.5f, bottomLeft.position.y + i, -1), Quaternion.identity);
                    SpawnScorePopUp(scorePerLine.ToString(), new Vector3(bottomLeft.position.x - 1.5f, bottomLeft.position.y + i), 1);
				}

                var a = Instantiate(AudioSourceInstance);
                a.GetComponent<AudioPlayAndDestroy>().toPlay = pop2;
                scoreNumber += scorePerLine;
                lines++;
            }
            yield return wait;
        }

        //calculate blocks
        foreach (Transform block in spawnPoint)
        {
            if (block.childCount != 0)
            {
                foreach (Transform childBlock in block)
                {
                    if (!(block.name.StartsWith("NoPoints") || block.name.StartsWith("Powerup_InvertOpponent")))
                    {
                        var a = Instantiate(AudioSourceInstance);
                        a.GetComponent<AudioPlayAndDestroy>().toPlay = pop1;
                        SpawnScorePopUp(scorePerBlock.ToString(), childBlock.position);
                        scoreNumber += scorePerBlock;
                        blocks++;
                    }
                    childBlock.gameObject.SetActive(false);
                    yield return wait;
                }
                continue;
            }

            if (!(block.name.StartsWith("NoPoints") || block.name.StartsWith("Powerup_InvertOpponent")))
            {
                var a = Instantiate(AudioSourceInstance);
                a.GetComponent<AudioPlayAndDestroy>().toPlay = pop1;
                SpawnScorePopUp(scorePerBlock.ToString(), block.position);
                scoreNumber += scorePerBlock;
                blocks++;
            }
            block.gameObject.SetActive(false);
            yield return wait;
        }

        clearBlocks();
        curScore = lines * scorePerLine + blocks * scorePerBlock;
        score += curScore;
        scoreNumber = score; //setting score text 
        curScore = 0;
        isDead = false;
        isInverted = false;

        hasActiveBlock = false;
        yield break;
    }
    private void CalculateBlocks()
    {
        //calculating lines
        for (int i = Height - 1; i >= 0; i--)
        {
            if (hasLine(i, grid))
            {
                lines++;
            }
        }
        //calculate per block
        foreach (Transform block in spawnPoint) 
        {
            if (block.childCount != 0)
            {
                foreach (Transform childBlock in block)
                {
                    if (!block.name.StartsWith("NoPoints"))
                    {
                        blocks++;
                    }
                }
                continue;
            }

            if (!block.name.StartsWith("NoPoints"))
                blocks++;
        }

        curScore = lines * scorePerLine + blocks * scorePerBlock;
    }

    public void Thud()
	{
        var a = Instantiate(AudioSourceInstance);
        if (Random.value > 0.5f)
            a.GetComponent<AudioPlayAndDestroy>().toPlay = thud;
        else
            a.GetComponent<AudioPlayAndDestroy>().toPlay = thud2;
    }

    public void SpawnCube(int x, int y)
    {
        x = Mathf.Clamp(x, 0, Width);
        y = Mathf.Clamp(y, 0, Height);

        GameObject item = Instantiate(Square, new Vector3(bottomLeft.position.x + x, bottomLeft.position.y + y, 0), Quaternion.identity);
        item.transform.SetParent(spawnPoint);

        grid[x, y] = item.transform;
    }

    public void SpawnScorePopUp(string text, Vector3 pos, int importance = 0) //0 = basic, 1 = big scor))
    {
        GameObject item = Instantiate(PopUp, pos, Quaternion.identity);
        item.GetComponent<TextPopUp>().SpawnPopUp(text, importance);
    }

    public bool DestroyCube(float x, float y)
    {
        Vector3 pos = new Vector3(x, y, 0);

        RaycastHit2D hit = Physics2D.CircleCast(pos, 0.1f, Vector2.zero, 0.1f, blockLayerMask);

        if (!hit)
            return false;

        if (hit.transform.gameObject.name.StartsWith("Powerup_InvertOpponent"))
            return false;

        Destroy(hit.collider.transform.gameObject);

        return true;
    }

    void clearBlocks()
    {
        grid = new Transform[Width, Height];

        foreach (Transform child in spawnPoint)
        {
            Destroy(child.gameObject);
        }
    }

    bool hasLine(int i, Transform[,] grid)
    {
        for (int j = 0; j < Width; j++)
        {
            Vector3 pos = new Vector3(bottomLeft.position.x + j, bottomLeft.position.y + i, 0);

            RaycastHit2D hit = Physics2D.CircleCast(pos, 0.1f, Vector2.zero, 0.1f, blockLayerMask);

            if (hit && hit.transform.gameObject.name.StartsWith("Powerup_InvertOpponent")) // hardcoded because FRICK you
                return false;

            if (grid[j, i] == null)
                return false;
        }
        return true;
    }


    //camera shit aiodhbnsajofbajkdbahjifgbahjdahjkdfahjfklůabfa
    public void shakeCamera()
    {
        if (CamShaker)
            CamShaker.Shake(CamShakePreset);
        else
            Debug.LogError("No camera to shake!");
    }
}
