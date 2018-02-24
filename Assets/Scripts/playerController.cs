using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : gameEntity
{

    //Everything concerning movement and jumping

    
   // int numberOfSpells;
   
   

    public Slider health;
    public Slider mana;

    public Text healthAmount;

    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject life4;

    public GameObject respawn;

    float playerHealth;
    public float maxHealth = 50;

    public float playerLives = 4;

    int i;
	new void Start ()
    {
        facingRight = true;
        
        //numberOfSpells = Spells.Length;

        spellSelect = 0;
        changeSpell(spellSelect);

        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        playerHealth = maxHealth;
    }
	


    


    //Change spell

    void changeSpell(int num)
    {
        int choice = num;
        for(int i = 0; i < Spells.Length; i++)
        {
            if(i == choice)
            {
                Spells[i].gameObject.SetActive(true);
            }
            else
            {
                Spells[i].gameObject.SetActive(false);
            }
        }
    }

    void cycleSpellsUp()
    {
        spellSelect++;

        if (spellSelect > Spells.Length - 1)
        {
            spellSelect = 0;
        }

        changeSpell(spellSelect);
    }
    void cycleSpellsDown()
    {
        spellSelect--;

        if (spellSelect < 0)
        {
            spellSelect = 2;
        }

        changeSpell(spellSelect);
    }



    IEnumerator takeDamageAfter(float time)
    {
        yield return new WaitForSeconds(time);
    }


    // Update is called once per frame
    void Update ()
    {

        showSpell = spellSelect;
        coolDown -= Time.deltaTime;
        health.value = calculateHealth();
        healthAmount.text = "Health: " + playerHealth.ToString();
        mana.value = coolDown;

        if (playerLives <= 3)
        {
            life4.SetActive(false);
        }
        if (playerLives <= 2)
        {
            life3.SetActive(false);
        }
        if (playerLives <= 1)
        {
            life2.SetActive(false);
        }
        if (playerLives <= 0)
        {
            life1.SetActive(false);
        }


        /*if (grounded == false)
        {
            rigid.drag = 0;
        }
        else
        {
            rigid.drag = 1;
        }*/

        
        //Movement, jumping and attacking
        if (Input.GetKeyDown("space") && grounded == true)
        {
            anim.SetInteger("state", 2);
            jump();
        }
        else  if (Input.GetKey("r") && coolDown <= 0)
        {
            anim.SetInteger("state", 3);

            StartCoroutine(ExecuteSpellAfterTime(animationTime));
            coolDown = 2.0f;
        }
        else if (Input.GetKey("a"))
        {
            facingRight = false;
            moveLeft();

            if (grounded == true)
            {
                anim.SetInteger("state", 1);
            }
        }
        else if (Input.GetKey("d"))
        {

            facingRight = true;
            moveRight();
            if (grounded == true)
            {
                anim.SetInteger("state", 1);
            }
        }
        else
        {
            if (grounded == true)
            {
                anim.SetInteger("state", 0);
            }
        }

        flip(0.6f);
        //End movement


        if (Input.GetKeyDown("tab"))
        {
            cycleSpellsUp();
        }
        if (Input.GetKeyDown("`"))
        {
            cycleSpellsDown();
        }




        /*
        for (int i = 0; i < numberOfSpells; i++)
        {
            if(Input.GetKeyDown("" + i))
            {
                spellSelect = i;

                changeSpell(spellSelect);
            }
        }
      */

        

    }

    float calculateHealth()
    {
        return playerHealth / maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            playerHealth -= 10;

            takeDamageAfter(2.0f);
        }

        if (playerHealth <= 0)
        {

            playerLives--;
            gameObject.transform.position = respawn.transform.position;
            playerHealth = 50;

            if (playerLives <= 0)
            {
                Debug.Log("Game over");
            }

        }
    }

}
