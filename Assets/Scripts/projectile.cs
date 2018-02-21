using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {

    public float lifeTime;
    public float speed;
   
	// Use this for initialization
	void Start ()
    {
        
        if(gameEntity.facingRight == false)
        {
            speed = -speed;
        }

        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed, 0));
        Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }


}

