using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTetromino : MonoBehaviour
{
    private Rigidbody2D rb;
    private TetrisBlock tb;

    private RigidbodyConstraints2D before;

    [HideInInspector]
    public bool isSpawned;

    public void Spawn(float gravityScale, float pieceScale)
    {
        rb = this.GetComponent<Rigidbody2D>();
        tb = this.GetComponent<TetrisBlock>();

        before = rb.constraints;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = gravityScale;
        tb.enabled = false;

        this.transform.localScale = new Vector3(pieceScale, pieceScale, pieceScale);

        isSpawned = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSpawned)
            return;

        if (collision.tag == "BlockKill")
            Destroy(gameObject);
    }
}
