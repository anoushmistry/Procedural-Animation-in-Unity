using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionContext : MonoBehaviour
{
    public enum EBodySide
    {
        LEFT,
        RIGHT
    }

    private TwoBoneIKConstraint leftBoneIkConstraint;
    private TwoBoneIKConstraint rightBoneIkConstraint;
    private MultiRotationConstraint leftMultiRotationConstraint;
    private MultiRotationConstraint rightMultiRotationConstraint;
    private Rigidbody rigidbody;
    private CapsuleCollider rootCollider;
    private Transform rootTransform;

    public EnvironmentInteractionContext(TwoBoneIKConstraint leftBoneIkConstraint,
        TwoBoneIKConstraint rightBoneIkConstraint, MultiRotationConstraint leftMultiRotationConstraint,
        MultiRotationConstraint rightMultiRotationConstraint, Rigidbody rigidbody, CapsuleCollider rootCollider, Transform rootTransform)
    {
        this.leftBoneIkConstraint = leftBoneIkConstraint;
        this.rightBoneIkConstraint = rightBoneIkConstraint;
        this.leftMultiRotationConstraint = leftMultiRotationConstraint;
        this.rightMultiRotationConstraint = rightMultiRotationConstraint;
        this.rigidbody = rigidbody;
        this.rootCollider = rootCollider;
        this.rootTransform = rootTransform;
    }

    // Read-Only Properties
    public TwoBoneIKConstraint LeftBoneIkConstraint => leftBoneIkConstraint;

    public TwoBoneIKConstraint RightBoneIkConstraint => rightBoneIkConstraint;

    public MultiRotationConstraint LeftMultiRotationConstraint => leftMultiRotationConstraint;

    public MultiRotationConstraint RightMultiRotationConstraint => rightMultiRotationConstraint;

    public Rigidbody Rigidbody => rigidbody;

    public CapsuleCollider RootCollider => rootCollider;
    public Transform RootTransform => rootTransform;

    public TwoBoneIKConstraint CurrentIkConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRotationConstraint { get; private set; }
    public Transform CurrentIkTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public EBodySide CurrentBodySide { get; private set; }

    public void SetCurrentSide(Vector3 positionToCheck)
    {
        Vector3
            leftRootPosition =
                LeftBoneIkConstraint.data.root.transform
                    .position; // This here is the left shoulder holding the left arm
        Vector3
            RightRootPosition =
                RightBoneIkConstraint.data.root.transform
                    .position; // This here is the right shoulder holding the right arm

        bool isLeftSideCloser = Vector3.Distance(positionToCheck, leftRootPosition) <
                                Vector3.Distance(positionToCheck, RightRootPosition);

        if (isLeftSideCloser)
        {
            Debug.Log("Left side closer");
            CurrentBodySide = EBodySide.LEFT;
            CurrentIkConstraint = leftBoneIkConstraint;
            CurrentMultiRotationConstraint = leftMultiRotationConstraint;
        }
        else
        {
            Debug.Log("Right side closer");
            CurrentBodySide = EBodySide.RIGHT;
            CurrentIkConstraint = rightBoneIkConstraint;
            CurrentMultiRotationConstraint = rightMultiRotationConstraint;
        }

        CurrentIkTargetTransform = CurrentIkConstraint.data.target.transform;
        CurrentShoulderTransform = CurrentIkConstraint.data.root.transform;
    }
}