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
    private Vector3 leftOriginalTargetPosition, rightOriginalTargetPosition;

    public float CharacterShoulderHeight;
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
        this.leftOriginalTargetPosition = leftBoneIkConstraint.data.target.transform.localPosition;
        this.rightOriginalTargetPosition = rightBoneIkConstraint.data.target.transform.localPosition;
        OriginalTargetRotation = leftBoneIkConstraint.data.target.rotation;
        
        CharacterShoulderHeight = leftBoneIkConstraint.data.root.transform.position.y;
        SetCurrentSide(Vector3.positiveInfinity);
    }

    // Read-Only Properties
    public TwoBoneIKConstraint LeftBoneIkConstraint => leftBoneIkConstraint;
    public TwoBoneIKConstraint RightBoneIkConstraint => rightBoneIkConstraint;
    public MultiRotationConstraint LeftMultiRotationConstraint => leftMultiRotationConstraint;
    public MultiRotationConstraint RightMultiRotationConstraint => rightMultiRotationConstraint;
    public Rigidbody Rigidbody => rigidbody;

    public CapsuleCollider RootCollider => rootCollider;
    public Transform RootTransform => rootTransform;

    public Collider CurrentIntersectingCollider {get; set;}
    public TwoBoneIKConstraint CurrentIkConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRotationConstraint { get; private set; }
    public Transform CurrentIkTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public EBodySide CurrentBodySide { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    
    public float ColliderCenterY { get; set; }
    public float InteractionPointYOffset { get; set; } = 0;
    public Vector3 CurrentOriginalTargetPosition { get; private set; }
    public Quaternion OriginalTargetRotation { get; private set; }
    public float LowestDistance { get; set; } = Mathf.Infinity;
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
            CurrentOriginalTargetPosition = leftOriginalTargetPosition;
        }
        else
        {
            Debug.Log("Right side closer");
            CurrentBodySide = EBodySide.RIGHT;
            CurrentIkConstraint = rightBoneIkConstraint;
            CurrentMultiRotationConstraint = rightMultiRotationConstraint;
            CurrentOriginalTargetPosition = rightOriginalTargetPosition;
        }

        CurrentIkTargetTransform = CurrentIkConstraint.data.target.transform;
        CurrentShoulderTransform = CurrentIkConstraint.data.root.transform;
    }
}