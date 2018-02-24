using state;
using UnityEngine;

public class chaseState : State<AI>
{
    private static chaseState instance;

    private chaseState()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    public static chaseState Instance
    {
        get
        {
            if (instance == null)
            {
                new chaseState();
            }
            return instance;
        }
    }



    public override void EnterState(AI owner)
    {
        Debug.Log("Entering chase");
    }


    public override void ExitState(AI owner)
    {
        Debug.Log("Exiting chase");
    }

    public override void UpdateState(AI owner)
    {
        if (AI.Instance.grounded == true)
        {
            AI.Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(-AI.Instance.movementSpeed / 2, 0);
        }
        



        if (owner.currentState == (int)AI.AIState.idle)
        {
            owner.stateMachine.ChangeState(idleState.Instance);
        }
        if(owner.currentState == (int)AI.AIState.fire)
        {
            owner.stateMachine.ChangeState(fireState.Instance);
        }
    }
}
