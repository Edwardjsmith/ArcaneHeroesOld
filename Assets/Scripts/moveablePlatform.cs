using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveablePlatform : MonoBehaviour {

    
    

    float movementMin;
    public float movementMax;
    public bool moveInX;
    public bool moveInY;

    float startPosX;
    float startPosY;

    float movement;
    // Use this for initialization

    public int offset;
    void start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;

    }

    // Update is called once per frame
    void Update ()
    {
        pingPong(moveInX, moveInY);
    }

    void pingPong(bool isX, bool isY)
    {
        if (isY == true)
        {
            transform.position = new Vector2(transform.position.x, Mathf.PingPong(Time.time, movementMax - startPosY) + offset);
        }
        if(isX == true)
        {
            transform.position = new Vector2(Mathf.PingPong(Time.time, movementMax - startPosX) + offset, transform.position.y);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

}
