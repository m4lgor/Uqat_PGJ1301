using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    [SerializeField] bool _LogCollisions;
    [SerializeField] bool _LogVelocity;
    void OnCollisionEnter(Collision c)
    {
        if (!_LogCollisions)
        {
            return;
        }

        // Impulsion appliquée à CE rigidbody pour résoudre le contact (N·s)
        Vector3 J = c.impulse;
        Vector3 n = c.GetContact(0).normal;
        HudConsole.Log($"{name}  ReceivedImpulse={J:F3}   Contact Normal={n:F3}");
    }

    private void FixedUpdate()
    {
        if (_LogVelocity)
        {
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                HudConsole.Log($"{name}  LinearVelocity={rb.linearVelocity:F3}   AngularVelocity={rb.angularVelocity:F3}");
            }
        }
    }
}
