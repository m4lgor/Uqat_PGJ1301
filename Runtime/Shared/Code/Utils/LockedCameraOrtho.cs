using UnityEngine;
using UnityEngine.InputSystem;

public class LockedCameraOrtho : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform _Target;

    [Header("Smoothing")]
    public float _SmoothSpeed = 5f;

    [SerializeField] float _Size = 5;

        
    private Vector3 _offset = new Vector3(0f, 0f, -10f); // Default 2D-style offset

    void FixedUpdate()
    {
        if (_Target == null) return;

        // Desired position based on target + offset
        Vector3 desiredPosition = _Target.position + _offset;

        // Smoothly interpolate from current to desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _SmoothSpeed * Time.deltaTime);

        // Always look forward along the Z axis
        transform.rotation = Quaternion.LookRotation(Vector3.forward);

        var cameraComponent = GetComponent<Camera>();
        if(cameraComponent)
        {
            cameraComponent.orthographicSize = Mathf.Lerp(cameraComponent.orthographicSize, _Size, Time.deltaTime);
        }
    }
}
