using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabby : MonoBehaviour
{
    [HideInInspector]
    public bool isMoving;
    [HideInInspector]
    public Vector3 movement;
    [HideInInspector]
    public TetrisScreen screen;
    [HideInInspector]
    public MiddlePart.tetrominoes[] tetrominoArray;

    public float speed = 6;
    [Space(5)]
    public Transform maxBottomLeft;
    public Transform maxTopRight;
    [Space]
    public Grabby otherGrabby;
    public MagnetPickupTetroManager magnetPickupTetroManager;
    [Space]
    public int priority; //chosen at random, decides which magnet picks up blocks first (milliseconds of delay)
    public int lastGrabbedTetrominoInstanceID = 0;

    private SpriteRenderer sr;
    private Vector3 startPos;
    private bool setStartPos;

    private float step;

    private bool singleShotAnim;
    private bool singleShotAnim_;
    private Animator anim;
    
    private void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	private void Update()
    {
        if (isMoving && !screen.isDead) {
            if (singleShotAnim_) //fade in after waking up
            {
                anim.SetTrigger("Fadein");
                singleShotAnim_ = false;
            }

            if (screen.isInverted)
                movement = -movement;

            this.transform.Translate(movement * speed * Time.deltaTime);

            singleShotAnim = true;
            setStartPos = true;
            step = 0f;

            //max margins
            Vector3 pos = transform.position;
            if (pos.x < maxBottomLeft.position.x)
                transform.position = new Vector3(maxBottomLeft.position.x, pos.y, pos.z);
            pos = transform.position;
            if (pos.y < maxBottomLeft.position.y)
                transform.position = new Vector3(pos.x, maxBottomLeft.position.y, pos.z);

            pos = transform.position;
            if (pos.x > maxTopRight.position.x)
                transform.position = new Vector3(maxTopRight.position.x, pos.y, pos.z);
            pos = transform.position;
            if (pos.y > maxTopRight.position.y)
                transform.position = new Vector3(pos.x, maxTopRight.position.y, pos.z);

        }
        else
        {
            singleShotAnim_ = true;

            if (setStartPos)
            {
                if (singleShotAnim)
				{
                    anim.SetTrigger("Fadeout");
                    singleShotAnim = false;
                }

                if (transform.position != startPos)
                {
                    step += Time.deltaTime * speed;

                    transform.position = Vector3.MoveTowards(transform.position, startPos, step);
                }
                else
                    setStartPos = false;
            }
        }
        //sr.enabled = isMoving;
    }  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (screen.hasActiveBlock)
            return;

        if (screen.isDead)
            return;

        if (collision.tag != "Block")
            return;

        FallingTetromino te = collision.gameObject.GetComponent<FallingTetromino>();

        if (!te)
            return;

        if (!te.isSpawned)
            return;

        magnetPickupTetroManager.ValidatePickup(this, te, collision);
    }
}
