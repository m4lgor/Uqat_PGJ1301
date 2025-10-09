using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private float _RotationSpeed = 100.0f; // Degrees per second

    // Update is called once per frame
    void Update()
    {
        var rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0.0f, 0.0f, _RotationSpeed * Time.deltaTime));
        }
        else
        {
            transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f) * _RotationSpeed * Time.deltaTime);
        }
    }
}
