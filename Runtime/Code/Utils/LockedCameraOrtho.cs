using UnityEngine;
using UnityEngine.InputSystem;

public class LockedCameraOrtho : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target;

    [Header("Smoothing")]
    public float smoothSpeed = 5f;

    [Header("Offset from Target")]
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Default 2D-style offset

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position based on target + offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate from current to desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Always look forward along the Z axis
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}
