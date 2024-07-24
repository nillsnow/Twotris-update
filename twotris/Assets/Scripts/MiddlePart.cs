using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiddlePart : MonoBehaviour
{
    [Header("Screens")]
    public GameObject screenLeft;
    [HideInInspector]
    public TetrisScreen sLeft;

    public GameObject screenRight;
    [HideInInspector]
    public TetrisScreen sRight;
    [Space(10)]
    public GameObject paddleLeft;
    [HideInInspector]
    public Grabby pLeft;

    public GameObject paddleRight;
    [HideInInspector]
    public Grabby pRight;

    [HideInInspector]
    public float gravityScale = 3f;
    [HideInInspector]
    public float gravityScalePowerup= 3f;

    [Header("Spawn settings - position reffering to spawn")]
    public float pieceScale = 0.5f;
    public GameObject position;
    public GameObject positionLeft;
    public GameObject positionRight;

    [Space(10)]
    public bool spawn;

    [System.Serializable] //shape array
    public class tetrominoes
    {
        public GameObject shape;
        public bool dontSpawn;
        [Range(0, 100)]
        public int weight;
    }

    [Space(10)]
    public tetrominoes[] tetrominoArray;


    [System.Serializable]
    public class powerups
    {
        public GameObject powerup;
        [Range(0, 100)]
        public int weightWinning;   //the chance of powerup spawning if the side is winning
        [Range(0, 100)]
        public int weightLosing;    //the chance of powerup spawning if the side is losing
        public int minScoreDifference = 0;
    }

    [Space(10)]
    public powerups[] powerupArray;

    private void Start()
    {
        if (screenLeft)
            sLeft = screenLeft.GetComponent<TetrisScreen>();
        if (screenRight)
            sRight = screenRight.GetComponent<TetrisScreen>();

        if (paddleLeft)
            pLeft = paddleLeft.GetComponent<Grabby>();
        if (paddleRight)
            pRight = paddleRight.GetComponent<Grabby>();


        sLeft.middle = this;
        sRight.middle = this;

        if (pLeft && sLeft)
        {
            pLeft.screen = sLeft;
            pLeft.tetrominoArray = tetrominoArray;
        }
        if (pRight && sRight)
        {
            pRight.screen = sRight;
            pRight.tetrominoArray = tetrominoArray;
        }
    }


    private void Update()
    {
        if (spawn)
        {
            spawnPiece();
            spawn = false;
        }

        if (paddleLeft && screenLeft)
        {
            pLeft.movement = Vector3.zero;
            pLeft.isMoving = !sLeft.hasActiveBlock;

            if (Input.GetKey(sLeft.keyLeft))
            {
                pLeft.movement.x = -1;
            }
            else if (Input.GetKey(sLeft.keyRight))
            {
                pLeft.movement.x = 1;
            }
            if (Input.GetKey(sLeft.keyDown))
            {
                pLeft.movement.y = -1;
            }
            else if (Input.GetKey(sLeft.keyUp))
            {
                pLeft.movement.y = 1;
            }
        }

        if (paddleRight && screenRight)
        {
            pRight.movement = Vector3.zero;
            pRight.isMoving = !sRight.hasActiveBlock;

            if (Input.GetKey(sRight.keyLeft))
            {
                pRight.movement.x = -1;
            }
            else if (Input.GetKey(sRight.keyRight))
            {
                pRight.movement.x = 1;
            }
            if (Input.GetKey(sRight.keyDown))
            {
                pRight.movement.y = -1;
            }
            else if (Input.GetKey(sRight.keyUp))
            {
                pRight.movement.y = 1;
            }
        }
    }

    public void spawnPiece()
    { //tetrominoes[0], position.transform);//
        Vector3 pos = position.transform.position;
        Vector3 spawnPos = new Vector3(pos.x + Random.value * 3.6f - 1.8f, pos.y); //range from -1.8 > 1.8

        tetrominoes potentionalSpawn = tetrominoArray[Random.Range(0, tetrominoArray.Length)];
        int chance = Random.Range(0, 100);

        //if object weight (chance of spawn) over random number, then spawn. otherwise keep looping until
        //we get a suitable shape. chance will probably change over time
        if (potentionalSpawn.weight < chance || potentionalSpawn.dontSpawn)
        {
            //print(potentionalSpawn.weight.ToString() + " < " + chance.ToString());
            spawnPiece();
            return;
        }

        GameObject piece = Instantiate(potentionalSpawn.shape, spawnPos, Quaternion.identity);
        piece.GetComponent<FallingTetromino>().Spawn(gravityScale, pieceScale);
    }

    public void spawnPieceSide()
    {
        //choose which side to spawn for randomly
        float side = Random.value;
        bool isWinning;

        if (side >= .5f) //right side
        {
            isWinning = sRight.score + sRight.curScore > sLeft.score + sLeft.curScore;
            spawnPowerup(positionRight.transform, isWinning);
        }
        else //left side
        {
            isWinning = sRight.score + sRight.curScore < sLeft.score + sLeft.curScore;
            spawnPowerup(positionLeft.transform, isWinning);
        }
    }

    public void spawnPowerup(Transform pos, bool isWinning)
    {
        //Debug.Log("side: " + pos.gameObject.name + " - isWinning: " + isWinning);
        powerups potentionalSpawn = powerupArray[Random.Range(0, powerupArray.Length)];
        int chance = Random.Range(0, 100);

        int weight;
        int scoreDif = 0;
        if (isWinning)
            weight = potentionalSpawn.weightWinning;
        else
		{
            weight = potentionalSpawn.weightLosing;
            scoreDif = Mathf.Abs(sRight.score + sRight.curScore - sLeft.score + sLeft.curScore);
        }

        Debug.Log("score: " + scoreDif + " > " + potentionalSpawn.minScoreDifference.ToString() + "weight" + weight + " < c" + chance + " potential?: " + potentionalSpawn.powerup.name);

        //for powerups that are too game breaking (need a huge score difference)
        if (potentionalSpawn.minScoreDifference > 0 && scoreDif < potentionalSpawn.minScoreDifference)
        {
            spawnPowerup(pos, isWinning);
            return;
        }

        //if object weight (chance of spawn) over random number, then spawn. otherwise keep looping until
        //we get a suitable shape. chance will probably change over time
        if (weight < chance)
        {
            //Debug.Log(weight + " > " + chance.ToString());
            spawnPowerup(pos, isWinning);
            return;
        }

        Debug.Log("spawned: " + potentionalSpawn.powerup.name);

        GameObject piece = Instantiate(potentionalSpawn.powerup, pos.position, Quaternion.identity);
        piece.GetComponent<FallingTetromino>().Spawn(gravityScalePowerup, pieceScale);
    }

    public void CalculateScoreClear()
    {
        sLeft.calculateScoreClear();
        sRight.calculateScoreClear();
    }

    public TetrisScreen GetOppositeScreen(TetrisScreen ts)
	{
        if (ts == sLeft)
            return sRight;

        return sLeft;
	}
}
