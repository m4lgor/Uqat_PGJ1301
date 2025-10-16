using UnityEngine;

public class SimpleThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;      // The transform to follow
    [SerializeField] private Vector3 _offset = new Vector3(0, 3, -5); // Default offset from the target
    [SerializeField] private bool _smoothFollow = true;
    [SerializeField] private float _smoothSpeed = 5f;

    void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 desiredPosition = _target.position + _offset;

        if (_smoothFollow)
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        else
            transform.position = desiredPosition;

        transform.LookAt(_target.position);

        // Rotation never changes — camera keeps its initial orientation
    }

    // Optional helper to set target and offset at runtime
    public void SetTarget(Transform newTarget, Vector3 newOffset)
    {
        _target = newTarget;
        _offset = newOffset;
    }
}
