using System;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Sirenix.OdinInspector;

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

    private EnvironmentInteractionContext _context;
    
    [SerializeField] private TwoBoneIKConstraint leftBoneIkConstraint;
    [SerializeField] private TwoBoneIKConstraint rightBoneIkConstraint;
    [SerializeField] private MultiRotationConstraint leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint rightMultiRotationConstraint;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private CapsuleCollider rootCollider;


    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        if (_context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, 0.03f);
        }
    }

    void Awake()
    {
        ValidateConstraints();
        
        _context = new EnvironmentInteractionContext(leftBoneIkConstraint, rightBoneIkConstraint,
            leftMultiRotationConstraint, rightMultiRotationConstraint, rigidbody, rootCollider, transform.root);
        
        InitializeStates();
        ConstructEnvironmentDetectionCollider();
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
       m_States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context,EEnvironmentInteractionState.Reset));
       m_States.Add(EEnvironmentInteractionState.Search, new SearchState(_context,EEnvironmentInteractionState.Search));
       m_States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context,EEnvironmentInteractionState.Approach));
       m_States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context,EEnvironmentInteractionState.Rise));
       m_States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context,EEnvironmentInteractionState.Touch));

       CurrentState = m_States[EEnvironmentInteractionState.Reset];
    }

    private void ConstructEnvironmentDetectionCollider()
    {
        float wingSpan = rootCollider.height;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingSpan,wingSpan,wingSpan);
        boxCollider.center = new Vector3(rootCollider.center.x, rootCollider.center.y + (0.25f * wingSpan),rootCollider.center.z + (0.5f * wingSpan));
        boxCollider.isTrigger = true;
    }
}
