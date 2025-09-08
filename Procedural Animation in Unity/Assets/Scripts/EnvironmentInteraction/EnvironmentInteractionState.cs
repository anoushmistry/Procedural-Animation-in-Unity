using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    protected EnvironmentInteractionContext Context;

    public EnvironmentInteractionState(EnvironmentInteractionContext context,
        EnvironmentInteractionStateMachine.EEnvironmentInteractionState stateKey) : base(stateKey)
    {
        Context = context;
    }

    private Vector3 GetClosestPositionOnCollider(Collider otherCollider, Vector3 positionToCheck)
    {
        return otherCollider.ClosestPoint(positionToCheck);
    }

    protected void StartIkTargetPositionTracking(Collider otherCollider)
    {
        if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") &&
            Context.CurrentIntersectingCollider == null)
        {
            Context.CurrentIntersectingCollider = otherCollider;
            Context.SetCurrentSide(GetClosestPositionOnCollider(otherCollider,
                Context.RootTransform.position)); // Gets the closest point from the root pos
            SetIkTargetPosition();
        }
    }

    protected void UpdateIkTargetPosition(Collider otherCollider)
    {
        if (otherCollider == Context.CurrentIntersectingCollider)
        {
            SetIkTargetPosition();
        }
    }

    protected void ResetIkTargetPositionTracking(Collider otherCollider)
    {
        if (otherCollider == Context.CurrentIntersectingCollider)
        {
            Context.CurrentIntersectingCollider = null;
            Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        }
    }

    private void SetIkTargetPosition()
    {
        Context.ClosestPointOnColliderFromShoulder = GetClosestPositionOnCollider(Context.CurrentIntersectingCollider,
            new Vector3(Context.CurrentShoulderTransform.position.x, Context.CharacterShoulderHeight,
                Context.CurrentShoulderTransform.position.z));

        Vector3 rayDirection = Context.CurrentShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = 0.05f;
        Vector3 offset = normalizedRayDirection * offsetDistance;
        
        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;
        Context.CurrentIkTargetTransform.position = new Vector3(offsetPosition.x, Context.InteractionPointYOffset , offsetPosition.z);
    }
}