using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCounterScreen : MonoBehaviour
{
	[Space(5)]
	public GameObject waveSquare;

    public int number;

	[Space(5)]
	public GameObject marginLeft;
	public GameObject marginRight;

	private float width = 7;
	private float singleBlockWidth;

	[HideInInspector]
	public GameMaster gameMaster;

	private GameObject currentBlock;

	private bool isLeft;

	private void Start()
	{
		StartCoroutine(WaitABit());
		isLeft = transform.parent.name.Contains("Left");
	}

	private void Update()
	{
		currentBlock = GameObject.Find("block" + (gameMaster.curDifficulty.screensClears - gameMaster.screnClearsDone).ToString());

		if (currentBlock)
			currentBlock.GetComponent<WaveSquareScript>().Disappear();
	}

	public void UpdateBlocks(int maxScreenClears)
	{
		//width = 7;//Mathf.Abs(marginLeft.transform.position.x) + Mathf.Abs(marginRight.transform.position.x);

		DeleteBlocks();

		number = maxScreenClears;

		singleBlockWidth = width / number;

		for (int i = 0; i < number; i++)
		{
			var pos = new Vector3(transform.position.x + (i * singleBlockWidth) - 3.5f + singleBlockWidth/2, transform.position.y);

			var obj = Instantiate(waveSquare, pos, Quaternion.identity);

			obj.transform.localScale = new Vector3(singleBlockWidth - 0.1f, obj.transform.localScale.y);
			obj.transform.parent = transform;
			obj.tag = isLeft ? "WaveCounterBlockL" : "WaveCounterBlock"; //kept deleting
			obj.name ="block" + i.ToString();
		}
	}

	private void DeleteBlocks()
    {
		foreach (GameObject block in GameObject.FindGameObjectsWithTag(isLeft ? "WaveCounterBlockL" : "WaveCounterBlock"))
        {
			Destroy(block);
		}
	}

	IEnumerator WaitABit()
	{
		yield return null;

		UpdateBlocks(gameMaster.curDifficulty.screensClears);

		yield break;
	}
}
