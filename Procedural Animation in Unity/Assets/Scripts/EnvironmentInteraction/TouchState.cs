using UnityEngine;

public class TouchState : EnvironmentInteractionState
{
    private float elapsedTime = 0.0f;
    private float resetThreshold = 0.5f;

    public TouchState(EnvironmentInteractionContext context,
        EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }

    public override void EnterState()
    {
        elapsedTime = 0.0f;
        Debug.Log("Entering Touch State");
    }

    public override void ExitState()
    {
       
    }

    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;
    }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        if (elapsedTime > resetThreshold)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        StartIkTargetPositionTracking(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        UpdateIkTargetPosition(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        ResetIkTargetPositionTracking(other);
    }
}
