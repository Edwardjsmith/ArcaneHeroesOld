using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using state;



    

    public class AI : gameEntity
    {
        public enum AIState
        {
            idle,
            chase
        }

        public int currentState;
        public GameObject player;
        public GameObject attack;
        public Transform attackSpawn;


        public AIStateMachine<AI> stateMachine { get; set; }

        new private void Start()
        {
            stateMachine = new AIStateMachine<AI>(this);
            stateMachine.ChangeState(idleState.Instance);
            rigid = gameObject.GetComponent<Rigidbody2D>();

        }

        private void Update()
        {
            if (Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(player.transform.position.x, 0, 0)) <= 10)
            {
                currentState = (int)AIState.chase;

                facingRight = false;

                if (Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(player.transform.position.x, 0, 0)) >= 5)
                {
                    rigid.AddForce(new Vector2(-movementSpeed, 0));
                }
                else
                {


                    attack.SetActive(true);
                    Instantiate(attack, attackSpawn.transform.position, transform.rotation);
                }
            }
            else
            {
                currentState = (int)AIState.idle;
            }


            stateMachine.Update();
        }
    }
