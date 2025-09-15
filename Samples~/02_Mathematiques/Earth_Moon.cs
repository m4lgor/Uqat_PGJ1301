using System;
using UnityEditor.Build;
using UnityEngine;

public class Solution : MonoBehaviour
{
    [Tooltip("The Transform in the Center of our rotation")]
    [SerializeField] Transform CenterTransform;

    [Tooltip("The rotation speed around the Center")]
    [SerializeField] float AngularSpeedDeg = 45.0f;

    [Tooltip("Enable look at will make our object rotate to always look at the Center")]
    [SerializeField] bool EnableLookAt = false;

    [SerializeField] bool EnableDebugDraw = false;

    // Orbit related
    Vector3 _PositionOffset; // Stored position offset
    Vector3 _RotationAxis; // Stored the rotation axis around the Center
    float _AngleDeg = 0.0f; // Incremented Angle for our rotation around the Center

    // LookAt related
    Quaternion _RotationOffset;

    void Start()
    {
        // Store the position offset
        _PositionOffset = transform.position - CenterTransform.position;

        // To find the orthogonal vector, we need a second vector that it not the same as the position offset. 
        // Any other vector works. We'll use Vector3.up in this case, but anything would have worked
        // as long as its not the same as the offset.
        Vector3 refDirection = Vector3.up;
        if (Mathf.Abs(Vector3.Dot(_PositionOffset.normalized, refDirection)) > 0.99f)
            refDirection = Vector3.right;

        // We can then calculate the axis around which we want to rotate
        _RotationAxis = Vector3.Cross(_PositionOffset, refDirection).normalized;

        // Loot at code
        if (EnableLookAt)
        {
            // Later on, we'll use the LookRotation to look at the Center transform.
            // Same as for the position offset, we want to keep our rotation offset to then add it on top of the lookat operation
            // This way our object will allways look at the Center, with the initial offset added.

            // Compute the "pure LookAt" at t=0
            // We use _RotationAxis as the upward vector, to prevent any usecase of our look at direction being the same as the upward vector. 
            Quaternion look0 = Quaternion.LookRotation((CenterTransform.position - transform.position).normalized, _RotationAxis);

            // Add the inverse lookat rotation to our current rotation and keep it.
            _RotationOffset = Quaternion.Inverse(look0) * transform.rotation;
        }

    }

    void Update()
    {
        // Increment angle every frame
        _AngleDeg += AngularSpeedDeg * Time.deltaTime;

        // Apply the rotate around to our position offset.
        // We end up with an offset vector, rotated around our axis. 
        // This is just an offset vector, so it still needs to be added to the Center's position.
        Vector3 positionOffsetRotateAround = Quaternion.AngleAxis(_AngleDeg, _RotationAxis) * _PositionOffset;

        // Add the rotated offset to the Center's position.
        // We have obtained the new position for our object.
        transform.position = CenterTransform.position + positionOffsetRotateAround;

        // Loot at code
        if (EnableLookAt)
        {
            // Same process as for the position offset.
            // We apply the look at rotation, then we add the Rotation offset calculated in the Start() to it.
            Quaternion look = Quaternion.LookRotation(
                (CenterTransform.position - transform.position).normalized,
                _RotationAxis
            );

            // Add the rotation offset to the lookat rotation.
            transform.rotation = look * _RotationOffset;
        }
    }

    private void OnDrawGizmos()
    {
        if(!EnableDebugDraw || CenterTransform == null || Mathf.Approximately(_PositionOffset.magnitude, 0.0f))
            return;

        Gizmos.color = Color.white;
        Vector3 drawOffset = _RotationAxis * _PositionOffset.magnitude;
        Gizmos.DrawLine(CenterTransform.position - drawOffset, CenterTransform.position + drawOffset);

        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        Gizmos.DrawLine(CenterTransform.position - drawOffset, transform.position);
        Gizmos.DrawLine(CenterTransform.position + drawOffset, transform.position);

        Vector3 test1 = CenterTransform.position - drawOffset - transform.position;
        Vector3 test2 = CenterTransform.position + drawOffset - transform.position;

        if (!Mathf.Approximately(test1.magnitude, test2.magnitude))
        {
            Debug.LogError(test1.magnitude - test2.magnitude);
        }
    }
}