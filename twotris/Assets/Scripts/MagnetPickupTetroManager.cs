using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPickupTetroManager : MonoBehaviour
{
	public Grabby magnetLeft;
	public Grabby magnetRight;

	private Coroutine delayTetroValidation;
	private bool b_delayTetroValidation = false;

	[HideInInspector]
	public MiddlePart.tetrominoes[] tetrominoArray;

	private void Awake()
	{
		SetMagnetPriority();
	}

	/* called by each grabby to validate if they're in the right picking a tetromino up
	 * used to check for piece duplication and other fuckery
	 */
	public void ValidatePickup(Grabby grabby, FallingTetromino tetro, Collider2D collision)
	{
		/* check if the piece picked up by the HIGHER priority piece is the same as the one we're attempting
		 * to pick up currently. if that is the case, discard this pick up as it's trying to 
		 * duplicate a tetronimo.
		 */
		if (grabby.priority == 0)
		{
			//if we're already delaying a magnet, just bug out. edge case of an edge case
			if (!b_delayTetroValidation)
			{
				b_delayTetroValidation = true;
				delayTetroValidation = StartCoroutine(DelayTetroValidation(grabby, tetro, collision));
				Debug.LogWarning("less priority pickup attempt! - " + grabby.gameObject.name);
				return;
			}
			Debug.LogError("HOW DID WE GET HERE! - " + grabby.gameObject.name);
		}

		grabby.lastGrabbedTetrominoInstanceID = tetro.GetInstanceID();
		SpawnTetrominoAtScreen(grabby, collision);
	}

	private IEnumerator DelayTetroValidation(Grabby grabby, FallingTetromino tetro, Collider2D collision)
	{
		/* if our magnet isnt a priority, we can delay the pick up by a physics frame,
		 * letting the higher priority magnet snatch the piece. this priority gets 
		 * swapped on a successful non priority pick up (magnet with less priority picks up
		 * a piece), so neither magnet has an advantage in the entire game.
		 */
		yield return new WaitForFixedUpdate();

		
		if (grabby.otherGrabby.lastGrabbedTetrominoInstanceID == tetro.GetInstanceID())
		{
			Debug.LogWarning("duplicate pickup PREVENTED (ID MATCH)! - " + grabby.gameObject.name);
			b_delayTetroValidation = false;
			yield break;
		}

		if (!collision)
		{
			Debug.LogWarning("duplicate pickup PREVENTED (NO PIECE)! - " + grabby.gameObject.name);
			b_delayTetroValidation = false;
			yield break;
		}

		Debug.LogError("less priority pickup successful! - " + grabby.gameObject.name + ", switching priority.");

		//switch magnet priority each LOWER priority pickup - your burden is gone!
		ReverseMagnetPriority();

		grabby.lastGrabbedTetrominoInstanceID = tetro.GetInstanceID();
		SpawnTetrominoAtScreen(grabby, collision);

		b_delayTetroValidation = false;
		yield break;
	}


	//by default one magnet starts with the first piece priority...
	public void SetMagnetPriority()
	{
		int rand = (int)Mathf.Round(Random.value);

		magnetLeft.priority = rand;
		magnetRight.priority = 1 - rand;
	}
	//...that is switched each time a 0 priority magnet picks up a piece (getting rid of their burden)
	public void ReverseMagnetPriority()
	{
		magnetLeft.priority = 1 - magnetLeft.priority;
		magnetRight.priority = 1 - magnetRight.priority;
	}


	private void SpawnTetrominoAtScreen(Grabby grabby, Collider2D collision)
	{
		print("PICKED UP! - " + grabby.name + ", priority - " + grabby.priority.ToString() + ", piece ID - " + grabby.lastGrabbedTetrominoInstanceID.ToString());

		/* we have to get a gameobject of the tetromino to instantiate and since i was a dumbass
		 * and didnt think about this from the beginning, i have to go through the list of 
		 * available tetrominos to get the one we want.
		 */
		
		foreach (MiddlePart.tetrominoes t in tetrominoArray)
		{
			if (collision.name.StartsWith(t.shape.name))
			{
				grabby.screen.nextToSpawn = t.shape;
				grabby.screen.hasActiveBlock = true;

				Destroy(collision.gameObject);
				return;
			}
		}
	}
}
