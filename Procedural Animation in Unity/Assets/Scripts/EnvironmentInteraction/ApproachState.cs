using UnityEngine;

public class ApproachState : EnvironmentInteractionState
{
    float elapsedTime = 0.0f;
    private float lerpTime = 5.0f;
    private float approachWeight = 0.5f;
    private float approachRotationWeight = 0.75f;
    private float rotationSpeed = 500f;
    
    private float riseDistanceThreshold = 0.5f;
    private float approachStateDuration = 2.0f;
    public ApproachState(EnvironmentInteractionContext context,
        EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Entering approach state");
        elapsedTime = 0.0f;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        Quaternion expectedGroundRotation = Quaternion.LookRotation(-Vector3.up, Context.RootTransform.forward);
        elapsedTime += Time.deltaTime;
        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation,
            expectedGroundRotation, rotationSpeed * Time.deltaTime);
        
        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight,
            approachRotationWeight, elapsedTime / lerpTime);
        
        Context.CurrentIkConstraint.weight =
            Mathf.Lerp(Context.CurrentIkConstraint.weight, approachWeight, elapsedTime / lerpTime);
    }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isOverStateLifeDuration = elapsedTime >= approachStateDuration;

        if (isOverStateLifeDuration || CheckShouldReset())
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        
        bool isWithinReach =
            Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.CurrentShoulderTransform.position) <
            riseDistanceThreshold;

        bool isClosestPointOnColliderValid = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;

        if (isClosestPointOnColliderValid && isWithinReach)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Rise;
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