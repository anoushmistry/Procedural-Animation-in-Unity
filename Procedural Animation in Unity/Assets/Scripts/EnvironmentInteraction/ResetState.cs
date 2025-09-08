using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    float elapsedTime = 0.0f;
    private float resetDuration = 3.0f;
    private float lerpDuration = 10.0f;
    private float rotationSpeed = 500f;
    public ResetState(EnvironmentInteractionContext context,
        EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }

    public override void EnterState()
    {
        elapsedTime = 0.0f;
        Debug.Log("Entering Reset State");
        Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        Context.CurrentIntersectingCollider = null;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;
        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset, Context.ColliderCenterY,
            elapsedTime / lerpDuration);
        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight, 0f,
            elapsedTime / lerpDuration);
        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight, 0f,
            elapsedTime / lerpDuration);
        
        //Setting the IK Constraints to the Original Positions
        Context.CurrentIkTargetTransform.localPosition = Vector3.Lerp(Context.CurrentIkTargetTransform.localPosition,
            Context.CurrentOriginalTargetPosition, elapsedTime / lerpDuration);
        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation,
            Context.OriginalTargetRotation, rotationSpeed * Time.deltaTime);
        Debug.Log("In Reset State");
    }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isMoving = Context.Rigidbody.linearVelocity != Vector3.zero;
        if (elapsedTime >= resetDuration && isMoving)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;
        }
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