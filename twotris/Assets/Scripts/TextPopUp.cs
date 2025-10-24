using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    public void SpawnPopUp(string text, int importance, bool staying)
    {
        GetComponent<TextMesh>().text = text;

        if (importance == 0)
		{
            GetComponent<TextMesh>().color = Color.white;
            GetComponent<TextMesh>().fontSize = 56;
        }
        else if (importance == 1)
		{
            GetComponent<TextMesh>().color = new Color(0.9686274510f, 0.8745098039f, 0.5254901961f);
            GetComponent<TextMesh>().fontSize = 64;
        }

        if (!staying)
            return;

        GetComponent<Animator>().SetBool("Staying", staying);
    }

    public void EndPopUp()
    {
        Destroy(gameObject);
    }
}
