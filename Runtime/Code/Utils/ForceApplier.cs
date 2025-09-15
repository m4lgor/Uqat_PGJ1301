using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    [SerializeField] Vector3 Force = new Vector3(0, 100f, 0);
    [SerializeField] ForceMode ForceMode = ForceMode.Impulse;
    [SerializeField] bool ApplyOnStart = false;
    [SerializeField] bool ApplyOnUpdate = false;

    Vector3 _StartPosition = Vector3.zero;

    [ContextMenu("Apply Impulse")]
    void ApplyImpulse()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Force, ForceMode);
        }
    }

    [ContextMenu("Reset")]
    void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = _StartPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _StartPosition = transform.position;

        if (ApplyOnStart)
        {
            ApplyImpulse();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ApplyOnUpdate)
        {
            ApplyImpulse();
        }
    }
}
