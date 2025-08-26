using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    public ResetState(EnvironmentInteractionContext context,
        EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = base.context;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Reset State");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        Debug.Log("In Reset State");
    }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        
    }

    public override void OnTriggerStay(Collider other)
    {
        
    }

    public override void OnTriggerExit(Collider other)
    {
        
    }
}
