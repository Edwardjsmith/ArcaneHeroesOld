using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using state;



    

    public class AI : gameEntity
    {

    private static AI _instance;

    public static AI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AI();
            }

            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    public enum AIState
        {
            idle,
            chase,
            fire
        }

        public int currentState;
        public GameObject player;
        public GameObject attack;
        public Transform attackSpawn;
        public GameObject spawn;


        public AIStateMachine<AI> stateMachine { get; set; }

        new private void Start()
        {
            stateMachine = new AIStateMachine<AI>(this);
            stateMachine.ChangeState(idleState.Instance);
            rigid = gameObject.GetComponent<Rigidbody2D>();
            whatIsGround = LayerMask.GetMask("Water");


            spawn.transform.position = transform.position;

        }

        private void Update()
        {

            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

            if (Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(player.transform.position.x, 0, 0)) <= 10)
            {
                currentState = (int)AIState.chase;

                facingRight = false;

                
                if(Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(player.transform.position.x, 0, 0)) <= 5)
                {
                    currentState = (int)AIState.fire;
                }
            }
            else
            {
                currentState = (int)AIState.idle;
            }


            stateMachine.Update();
        }
    }
