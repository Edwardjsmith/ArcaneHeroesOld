using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellChanger : MonoBehaviour {

    public Sprite water;
    public Sprite fire;
    public Sprite wind;

	// Use this for initialization
	
	
	// Update is called once per frame
	void Update ()
    {
        if (playerController.spellSelect == 0)
        {
            GetComponent<SpriteRenderer>().sprite = water;
        }
        if (playerController.spellSelect == 1)
        {
            GetComponent<SpriteRenderer>().sprite = wind;
        }
        if (playerController.spellSelect == 2)
        {
            GetComponent<SpriteRenderer>().sprite = fire;
        }
    }
}
