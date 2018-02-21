using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staffPos : MonoBehaviour {


    public GameObject player;

    float posX;
    float posY;

    float offsetX = 2.3f;
    float offsetY = 0.3f;

    // Update is called once per frame
    void Update ()
    {
        posX = player.transform.position.x + offsetX;
        posY = player.transform.position.y + offsetY;


        transform.position = new Vector3(posX, posY, 0);
      
    }
}
