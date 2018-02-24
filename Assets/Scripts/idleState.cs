using state;

using UnityEngine;

public class idleState : State<AI>
{
    private static idleState instance;

    float timer = 5.0f;

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
        if (timer <= 0 && AI.Instance.transform.position.x != AI.Instance.spawn.transform.position.x)
        {

            float step = AI.Instance.movementSpeed * Time.deltaTime;
            AI.Instance.transform.position = Vector3.MoveTowards(AI.Instance.transform.position, AI.Instance.spawn.transform.position, step);

        }
        timer -= Time.deltaTime;

        if(owner.currentState == (int)AI.AIState.chase)
        {
            owner.stateMachine.ChangeState(chaseState.Instance);
        }
    }
}
