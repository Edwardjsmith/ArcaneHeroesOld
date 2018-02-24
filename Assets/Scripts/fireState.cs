using state;

using UnityEngine;

public class fireState : State<AI>
{
    private static fireState instance;

    private fireState()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    public static fireState Instance
    {
        get
        {
            if (instance == null)
            {
                new fireState();
            }
            return instance;
        }
    }



    public override void EnterState(AI owner)
    {
        Debug.Log("Entering fire");
    }


    public override void ExitState(AI owner)
    {
        Debug.Log("Exiting fire");
    }

    public override void UpdateState(AI owner)
    {
        AI.Instance.attack.SetActive(true);
        AI.Instance.fire(AI.Instance.attack, AI.Instance.attackSpawn, AI.Instance.attackSpawn);







        if (owner.currentState == (int)AI.AIState.chase)
        {
            owner.stateMachine.ChangeState(chaseState.Instance);
        }
        if (owner.currentState == (int)AI.AIState.idle)
        {
            owner.stateMachine.ChangeState(idleState.Instance);
        }
    }
}
