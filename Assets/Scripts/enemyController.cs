using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : gameEntity {

    public float HP;

    // Update is called once per frame
    void Update ()
    {
       
        

        

    }

        void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9 || collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 12)
        {
            HP -= 10;
        }
    }
}
