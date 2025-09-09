using UnityEngine;

public abstract class
    EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    protected EnvironmentInteractionContext Context;
    private float moveAwayOffset = 0.005f;
    private bool shouldReset;

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
            shouldReset = true;
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
        Context.CurrentIkTargetTransform.position =
            new Vector3(offsetPosition.x, Context.InteractionPointYOffset, offsetPosition.z);
    }

    protected bool CheckShouldReset()
    {
        if (shouldReset)
        {
            Context.LowestDistance = Mathf.Infinity;
            shouldReset = false;
            return true;
        }

        bool isBadAngle = CheckIsBadAngle();
        bool isPlayerStopped = Context.Rigidbody.linearVelocity == Vector3.zero;
        bool isMovingAway = CheckIsMovingAway();


        bool isJumping = Mathf.Round(Context.Rigidbody.linearVelocity.y) >= 1;

        if (isPlayerStopped /*|| isMovingAway*/ || isBadAngle || isJumping)
        {
            Context.LowestDistance = Mathf.Infinity;
            return true;
        }

        return false;
    }

    protected bool CheckIsMovingAway()
    {
        float currentDistanceToTarget =
            Vector3.Distance(Context.RootTransform.position, Context.ClosestPointOnColliderFromShoulder);

        bool isSearchingForNewInteraction = Context.CurrentIntersectingCollider == null;
        if (isSearchingForNewInteraction) return false;

        bool isGettingCloserToTarget = currentDistanceToTarget <= Context.LowestDistance;
        if (isGettingCloserToTarget)
        {
            Context.LowestDistance = currentDistanceToTarget;
            return false;
        }

        bool isMovingAway = currentDistanceToTarget > Context.LowestDistance + moveAwayOffset;
        if (isMovingAway)
        {
            Context.LowestDistance = Mathf.Infinity;
            return false;
        }

        return false;
    }

    private bool CheckIsBadAngle()
    {
        if (Context.CurrentIntersectingCollider == null) return false;

        Vector3 targetDirection =
            Context.ClosestPointOnColliderFromShoulder - Context.CurrentShoulderTransform.position;
        Vector3 shoulderDirection = Context.CurrentBodySide == EnvironmentInteractionContext.EBodySide.RIGHT
            ? Context.RootTransform.right
            : -Context.RootTransform.right;

        float dotProduct = Vector3.Dot(shoulderDirection, targetDirection.normalized);
        bool isBadAngle = dotProduct < 0;

        return isBadAngle;
    }
}