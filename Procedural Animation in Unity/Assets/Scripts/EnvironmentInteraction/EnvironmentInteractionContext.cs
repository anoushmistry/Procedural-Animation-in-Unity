using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionContext : MonoBehaviour
{
   

    private TwoBoneIKConstraint leftBoneIkConstraint;
    private TwoBoneIKConstraint rightBoneIkConstraint;
    private MultiRotationConstraint leftMultiRotationConstraint;
    private MultiRotationConstraint rightMultiRotationConstraint;
    private Rigidbody rigidbody;
    private CapsuleCollider rootCollider;
    
    public EnvironmentInteractionContext(TwoBoneIKConstraint leftBoneIkConstraint,
        TwoBoneIKConstraint rightBoneIkConstraint, MultiRotationConstraint leftMultiRotationConstraint,
        MultiRotationConstraint rightMultiRotationConstraint, Rigidbody rigidbody, CapsuleCollider rootCollider)
    {
        this.leftBoneIkConstraint = leftBoneIkConstraint;
        this.rightBoneIkConstraint = rightBoneIkConstraint;
        this.leftMultiRotationConstraint = leftMultiRotationConstraint;
        this.rightMultiRotationConstraint = rightMultiRotationConstraint;
        this.rigidbody = rigidbody;
        this.rootCollider = rootCollider;
    }
    
    // Read-Only Properties
    public TwoBoneIKConstraint LeftBoneIkConstraint => leftBoneIkConstraint;

    public TwoBoneIKConstraint RightBoneIkConstraint => rightBoneIkConstraint;

    public MultiRotationConstraint LeftMultiRotationConstraint => leftMultiRotationConstraint;

    public MultiRotationConstraint RightMultiRotationConstraint => rightMultiRotationConstraint;

    public Rigidbody Rigidbody => rigidbody;

    public CapsuleCollider RootCollider => rootCollider;
    
    
}