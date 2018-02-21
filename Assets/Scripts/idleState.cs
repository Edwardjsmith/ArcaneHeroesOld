using state;

using UnityEngine;

public class idleState : State<AI>
{
    private static idleState instance;

    private idleState()
    {
        if(instance != null)
        {
            return;
        }

        instance = this;
    }

    public static idleState Instance
    {
        get
        {
            if(instance == null)
            {
                new idleState();
            }
            return instance;
        }
    }

   

    public override void EnterState(AI owner)
    {
        Debug.Log("Entering idle");
    }

 
    public override void ExitState(AI owner)
    {
        Debug.Log("Exiting idle");
    }

    public override void UpdateState(AI owner)
    {
        if(owner.currentState == (int)AI.AIState.chase)
        {
            owner.stateMachine.ChangeState(chaseState.Instance);
        }
    }
}
