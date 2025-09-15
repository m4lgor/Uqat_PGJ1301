using UnityEngine;
using UnityEngine.UIElements;

public class Satellite : MonoBehaviour
{

    [Tooltip("The first Transform for the axis")]
    [SerializeField] Transform Transform0;

    [Tooltip("The common transform or the 2 axis product")]
    [SerializeField] Transform CenterTransform;

    [Tooltip("The second in Transform for the axis")]
    [SerializeField] Transform Transform1;

    [Tooltip("Rotation speed")]
    [SerializeField] float AngularSpeedDeg = 45f;

    [Tooltip("Enable debug draws")]
    [SerializeField] bool EnableDebugDraw = false;

    Vector3 _RotationAxis = Vector3.right;
    Vector3 _PositionOffset;

    float _Angle = 0.0f;

    void Start()
    {
        // Don't continue if any data is invalid.
        if (!CenterTransform || !Transform0 || !Transform1)
        {
            Debug.LogError("Invalid Center or transform0 or transform2. Fix to continue");
            enabled = false; 

            return;
        }

        // Store the initial position offset
        _PositionOffset = transform.position - CenterTransform.position; 
    }

    void Update() 
    {
        // Increase angle
        _Angle += AngularSpeedDeg * Time.deltaTime;

        // Calculate the rotation axis every frame as it could change.
        if (!CalculateRotationAxis(out _RotationAxis))
        {
            Debug.LogError("Invalid rotation axis calculation");
            return;
        }

        // Create rotation quaternion
        Quaternion rot = Quaternion.AngleAxis(_Angle, _RotationAxis);

        // Rotate the position offset using the newly calculated rotation axis
        Vector3 newPosition = rot * _PositionOffset;

        // Add the centerTransform offset to return.
        // We add this after applying the rotaiton as this was not supposed to have any rotation applied.
        newPosition += CenterTransform.position;

        // Update the transform's position.
        transform.position = newPosition;
    }

    bool CalculateRotationAxis(out Vector3 OutRotationAxis)
    {
        OutRotationAxis = Vector3.zero;
        if (Transform0 == null || CenterTransform == null || Transform1 == null)
        {
            return false;
        }
        
        Vector3 V0 = (Transform0.position - CenterTransform.position).normalized;
        Vector3 V1 = (CenterTransform.position - Transform1.position).normalized;

        // Return false if both vectors are in the same direction
        if(Mathf.Approximately(Mathf.Abs(Vector3.Dot(V0, V1)), 1.0f))
        {
            return false;
        }

        // Rotation axis is the orthogonal vector to both V0 and V1
        OutRotationAxis = Vector3.Cross(V0, V1);

        return true;
    }

    void OnDrawGizmos()
    {
        if (!EnableDebugDraw)
        {
            return;
        }

        // We shouldn't recalculate this here but its useful to display the debug in stop mode.
        if(!CalculateRotationAxis(out _RotationAxis))
        {
            return;
        }

        Gizmos.color = Color.black;
        if (Transform0 == null || CenterTransform == null || Transform1 == null)
            return;

        float size = Mathf.Max(5.0f, _PositionOffset.magnitude);

        Gizmos.DrawLine(CenterTransform.position, CenterTransform.position + _RotationAxis * size);
        Gizmos.DrawLine(CenterTransform.position, CenterTransform.position - _RotationAxis * size);

        Gizmos.color = new Color(0.0f, 0.0f, 0.0f, 0.4f);
        Gizmos.DrawLine(Transform0.position, CenterTransform.position);
        Gizmos.DrawLine(Transform1.position, CenterTransform.position);


    }
}