using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;

    public bool canRotate;

    public TetrisScreen screen;

    private int bottomLeftX;
    private int bottomLeftY;

    public UnityEvent OnAddToGrid;

    [HideInInspector]
    public float previousTime;

    private bool isPressingDown;

    //horrible hardcoding
    bool keyLeftJustPressed = true;
    Coroutine leftCoroutine;

    bool keyRightJustPressed = true;
    Coroutine rightCoroutine;

    private void Start()
    {
        if (!screen)
            return;

        bottomLeftX = Mathf.RoundToInt(screen.bottomLeft.parent.position.x);
        bottomLeftY = Mathf.RoundToInt(screen.bottomLeft.parent.position.y);

        if (!validMove())
        {
            screen.isDead = true;
            addToGrid();
            this.enabled = false;
        }

        isPressingDown = Input.GetKey(screen.keyDown);
    }

    void Update()
    {
        if (!screen)
            return;

        if (Input.GetKey((screen.isInverted) ? screen.keyRight : screen.keyLeft)) //LEFT KEY-------------
        {
            if (keyLeftJustPressed)
            {
                leftCoroutine = StartCoroutine(moveLeft());
                keyLeftJustPressed = false;
            }
        }
        else
        {
            if (leftCoroutine != null)
                StopCoroutine(leftCoroutine);
            keyLeftJustPressed = true;
        }

        if (Input.GetKey((screen.isInverted) ? screen.keyLeft : screen.keyRight)) //RIGHT KEY-------------
        {
            if (keyRightJustPressed)
            {
                rightCoroutine = StartCoroutine(moveRight());
                keyRightJustPressed = false;
            }
        }
        else
        {
            if (rightCoroutine != null)
                StopCoroutine(rightCoroutine);
            keyRightJustPressed = true;
        }

        
        if (Input.GetKeyDown((screen.isInverted) ? screen.keyDown : screen.keyUp) && canRotate) //ROTATION -------------
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            if (!validMove())
            {
                //check for valid position on both sides
                transform.position += new Vector3(-1, 0, 0); //right
                if (!validMove())
                    transform.position -= new Vector3(-1, 0, 0);

                transform.position += new Vector3(1, 0, 0); //left
                if (!validMove())
                    transform.position -= new Vector3(1, 0, 0);
            }
            if (!validMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            else
                RotateOverlay(-90);
        }

        float nextFallTime = screen.fallTime;
        //if we hold key while entering placing state, ignore
        if (Input.GetKey((screen.isInverted) ? screen.keyUp : screen.keyDown) && !isPressingDown)
            nextFallTime = screen.fallTime / 10f;

        if (!Input.GetKey((screen.isInverted) ? screen.keyUp : screen.keyDown))
            isPressingDown = false;

        //detect if block under is SOLID, then set the time to something consistent.
        transform.position += new Vector3(0, -1, 0);
        if (!validMove())
        {
            if (Input.GetKeyDown((screen.isInverted) ? screen.keyUp : screen.keyDown))
                nextFallTime = 0;
            else //if you're moving the piece with blocks underneath, i will let u move for longer:)
                if (Input.GetKeyDown((screen.isInverted) ? screen.keyDown : screen.keyUp) ||
                    Input.GetKey(screen.keyLeft) ||
                    Input.GetKey(screen.keyRight) )
                    nextFallTime = screen.fallTime;
        }
        transform.position -= new Vector3(0, -1, 0);

        if (Time.time - previousTime > nextFallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            if (!validMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                addToGrid();
                this.enabled = false;
            }
            previousTime = Time.time;
        }
    }

    IEnumerator moveLeft()
    {   //move once
        transform.position += new Vector3(-1, 0, 0);
        if (!validMove())
            transform.position -= new Vector3(-1, 0, 0);

        //pause
        yield return new WaitForSeconds(0.2f);

        WaitForSeconds wait = new WaitForSeconds(0.075f);
        while (true)
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!validMove())
                transform.position -= new Vector3(-1, 0, 0);

            yield return wait;
        }
    }
    IEnumerator moveRight()
    {   //move once
        transform.position += new Vector3(1, 0, 0);
        if (!validMove())
            transform.position -= new Vector3(1, 0, 0);

        //pause
        yield return new WaitForSeconds(0.2f);

        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (true)
        {
            transform.position += new Vector3(1, 0, 0);
            if (!validMove())
                transform.position -= new Vector3(1, 0, 0);

            yield return wait;
        }
    }

    public void addToGrid(bool silent = false)
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            screen.grid[roundedX - bottomLeftX, roundedY - bottomLeftY] = children;
        }
        OnAddToGrid.Invoke();
        screen.calculateScore();

        StopAllCoroutines();

        screen.wasJustPlaced = true;

        if (silent)
            return;

        screen.shakeCamera();
        screen.Thud();
    }

    bool validMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < bottomLeftX || roundedX >= bottomLeftX + TetrisScreen.Width || roundedY < bottomLeftY || roundedY >= bottomLeftY + TetrisScreen.Height)
            {
                return false;
            }

            if (screen.grid[roundedX - bottomLeftX, roundedY - bottomLeftY] != null)
            {
                return false;
            }
        }

        return true;
    }

    void RotateOverlay(int degrees)
    {
        foreach (Transform child in transform)
        {
            if (child.childCount == 0)
                continue;

            Transform potentialRotation = child.GetChild(0);

            if (potentialRotation.name == "no_rot")
                potentialRotation.RotateAround(potentialRotation.position, new Vector3(0, 0, 1), -degrees);
        }
    }
}

/*
poznámky k typkovi:

- viteslav nezval
-intelektual
-basnik
- moc basni
- Romany, divadelni hry, pica
-socialisticky realismus, surrealismus
- Zastaval moskevske procesy
- rozesel se s skupinou surrealistu, ktery nemel moc rad nevim proc lol
- PRONASLEDOVANY za protektoratu
- 2.5 millionu nemcu z ceska slo a tam byl i viteslav
- VOLAM NECO NARODU
- Komousi nabizeli nejaky neco nevim proste nechapu to moc rychly  nemuzu si zapisovat
- "MUSITE DELAT VELIKI KRASNI VECI! DNES SE MUZE SECKO"
- MILACKU TY MAS V USTECH TRESNI, JAK CHUTNA TI? TAKOVE ODPOVEDI SE TI NEVRATI
- NECHAPU TO PROSTE PROS SI TO MUSIME ZAPISOVAT KDYZ JE TO V UCEBNICI DO PRDELE

Prazsky penis
- muze se pokouset definovat jeho tajemnstvi
*/