using UnityEngine;

public class RiseState : EnvironmentInteractionState
{
    private float elapsedTime = 0.0f;
    private float lerpDuration = 5.0f;
    private float riseStateWeight = 1.0f;
    private Quaternion expectedRotation;
    private float rayMaxDistance = 0.5f;
    private LayerMask interactableLayerMask = LayerMask.GetMask("Interactable");
    private float rotationSpeed = 1000f;
    
    private float touchDistanceThreshold = 0.05f;
    private float touchTimeThreshold = 1f;
    

    public RiseState(EnvironmentInteractionContext context,
        EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }

    public override void EnterState()
    {
        elapsedTime = 0.0f;
        Debug.Log("Entering Rise State");
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        CalculateTargetHandRotation();

        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset,
            Context.ClosestPointOnColliderFromShoulder.y, elapsedTime / lerpDuration);

        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight, riseStateWeight,
            elapsedTime / lerpDuration);
        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight,
            riseStateWeight,
            elapsedTime / lerpDuration);

        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation,
            expectedRotation, rotationSpeed * Time.deltaTime);

        elapsedTime += Time.deltaTime;
    }

    private void CalculateTargetHandRotation()
    {
        Vector3 startPosition = Context.CurrentShoulderTransform.position;
        Vector3 endPosition = Context.ClosestPointOnColliderFromShoulder;

        Vector3 directionToTarget = (endPosition - startPosition).normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPosition, directionToTarget, out hit, rayMaxDistance, interactableLayerMask))
        {
            Vector3 surfaceNormal = hit.normal;
            Vector3 targetForward = -surfaceNormal;

            expectedRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        }
    }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        Debug.Log(Vector3.Distance(Context.CurrentIkTargetTransform.position,
            Context.ClosestPointOnColliderFromShoulder));
        if (Vector3.Distance(Context.CurrentIkTargetTransform.position, Context.ClosestPointOnColliderFromShoulder) <
            touchDistanceThreshold && elapsedTime >= touchTimeThreshold)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Touch;
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