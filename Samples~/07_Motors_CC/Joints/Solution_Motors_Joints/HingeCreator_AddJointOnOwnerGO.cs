using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HingeCreator_AddJointOnOwnerGO : MonoBehaviour
{
    List<GameObject> GOHits = new List<GameObject>();
    Vector3 rayAxis = new Vector3(1.0f, 0.0f, 0.0f); 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void FixedUpdate()
    {
        var hits = Physics.RaycastAll(transform.position, this.rayAxis, 100);
        Quaternion rot = Quaternion.AngleAxis(2, new Vector3(0, 0, 1));
        rayAxis = rot * rayAxis;
        for (int i = 0; i < hits.Length; i++) 
        {
            var hit = hits[i];
            var hitGO = hit.collider.gameObject;
            if (GOHits.Contains(hitGO))
            {
                continue;
            }

            // Add hinge component on hit gameobject.
            var hingeJoint = gameObject.AddComponent<HingeJoint>();

            var hitRB = hitGO.GetComponent<Rigidbody>();

            if (!hitRB || !GetComponent<Rigidbody>())
            {
                continue;
            }

            // Set the connected body.
            hingeJoint.connectedBody = hitRB;

            // For the exemple this is turned to false, but if you set this to true, you won't need to set the connectedAnchor.
            // The joint will calculate the connected anchor for you, assuming that both anchors are at the same position
            // (which is what we want here)
            hingeJoint.autoConfigureConnectedAnchor = false;

            // Set the anchor position from the owner's perspective, in local space. (Which is likely to be zero)
            hingeJoint.anchor = transform.InverseTransformPoint(transform.position);
            // Set the anchor from the hit bodie's perspective, in local space.
            hingeJoint.connectedAnchor = hit.transform.InverseTransformPoint(transform.position);

            // Set the axis to local space too.
            hingeJoint.axis = transform.InverseTransformPoint(Vector3.forward);

            // Motor hardcode
            JointMotor motor = new JointMotor();
            motor.force = 10000;
            motor.targetVelocity = -50;
            hingeJoint.motor = motor;
            hingeJoint.useMotor = true;

            // Add GO to the list.
            GOHits.Add(hit.collider.gameObject);
            Debug.Log("Hit: " + hit.collider.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        if(!enabled)
        {
            return;
        }

        Gizmos.DrawLine(transform.position, transform.position + rayAxis * 100);
    }
}
