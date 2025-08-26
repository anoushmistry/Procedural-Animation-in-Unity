using System;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset,
    }

    private EnvironmentInteractionContext context;
    
    [SerializeField] private TwoBoneIKConstraint leftBoneIkConstraint;
    [SerializeField] private TwoBoneIKConstraint rightBoneIkConstraint;
    [SerializeField] private MultiRotationConstraint leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint rightMultiRotationConstraint;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private CapsuleCollider rootCollider;


    private void Awake()
    {
        ValidateConstraints();
        
        context = new EnvironmentInteractionContext(leftBoneIkConstraint, rightBoneIkConstraint,
            leftMultiRotationConstraint, rightMultiRotationConstraint, rigidbody, rootCollider);
        
        InitializeStates();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(leftBoneIkConstraint, "Left bone IK constraint is not assigned");
        Assert.IsNotNull(rightBoneIkConstraint, "Right bone IK constraint is not assigned");
        Assert.IsNotNull(leftMultiRotationConstraint, "Left multi rotation constraint is not assigned");
        Assert.IsNotNull(rightMultiRotationConstraint, "Right multi rotation constraint is not assigned");
        Assert.IsNotNull(rigidbody, "Rigidbody is not assigned");
        Assert.IsNotNull(rootCollider, "RootCollider is not assigned");
    }

    public void InitializeStates()
    {
       m_States.Add(EEnvironmentInteractionState.Reset, new ResetState(context,EEnvironmentInteractionState.Reset));
       m_States.Add(EEnvironmentInteractionState.Search, new ResetState(context,EEnvironmentInteractionState.Search));
       m_States.Add(EEnvironmentInteractionState.Approach, new ResetState(context,EEnvironmentInteractionState.Approach));
       m_States.Add(EEnvironmentInteractionState.Rise, new ResetState(context,EEnvironmentInteractionState.Rise));
       m_States.Add(EEnvironmentInteractionState.Touch, new ResetState(context,EEnvironmentInteractionState.Touch));

       CurrentState = m_States[EEnvironmentInteractionState.Reset];
    }
}
