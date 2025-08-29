using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    protected EnvironmentInteractionContext context;
    
    public EnvironmentInteractionState(EnvironmentInteractionContext context,
        EnvironmentInteractionStateMachine.EEnvironmentInteractionState stateKey) : base(stateKey)
    {
        this.context = context;
    }

    private Vector3 GetClosestPositionOnCollider(Collider otherCollider, Vector3 positionToCheck)
    {
        return otherCollider.ClosestPoint(positionToCheck);
    }

    protected void StartIkTargetPositionTracking(Collider otherCollider)
    {
        context.SetCurrentSide(GetClosestPositionOnCollider(otherCollider,
            context.RootTransform.position)); // Gets the closest point from the root pos
    }
    protected void UpdateIkTargetPosition(Collider otherCollider)
    {
        
    }
    protected void ResetIkTargetPositionTracking(Collider otherCollider)
    {
        
    }
    
}
