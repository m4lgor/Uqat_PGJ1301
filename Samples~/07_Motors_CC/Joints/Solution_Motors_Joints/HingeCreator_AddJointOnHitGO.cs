using UnityEngine;

public class HingeCreator_AddJointOnHitGO : MonoBehaviour
{
    Vector3 rayAxis = new Vector3(1.0f, 0.0f, 0.0f); 

    // Update is called once per frame
    void FixedUpdate()
    {
        var hits = Physics.RaycastAll(transform.position, rayAxis, 100);
        Quaternion rot = Quaternion.AngleAxis(2, new Vector3(0, 0, 1));
        rayAxis = rot * rayAxis;
        for (int i = 0; i < hits.Length; i++) 
        {
            var hit = hits[i];
            var hitGO = hit.collider.gameObject;
            if (hitGO.GetComponent<HingeJoint>())
            {
                // Continue if this hit already have a constraint.
                continue;
            }

            var myRB = gameObject.GetComponent<Rigidbody>();
            if (!myRB || !hit.rigidbody)
            {
                // Continue to next hit if there is any missing rigidbody.
                continue;
            }

            // Hinge Component
            var hingeJoint = hitGO.AddComponent<HingeJoint>();
            if(myRB)
            {
                // /!\ /!\ /!\ Here is a big trick. To prevent the physics solver from doing some complicated resolutions,
                // we need to disable all the Constraints that we've set on the hit rigid body to allow the
                // joint to create the constraints itself.
                hit.rigidbody.constraints = RigidbodyConstraints.None;
                myRB.constraints = RigidbodyConstraints.None;

                // The main body is the joint's script owner.(HitGO)
                hingeJoint.connectedBody = myRB;
                hingeJoint.autoConfigureConnectedAnchor = false;

                // Transform the script owner's transform in HitGO's local space for the Anchor.
                hingeJoint.anchor = hitGO.transform.InverseTransformPoint(transform.position);

                // Transform the script owner's transform in its own local space for the Anchor.
                hingeJoint.connectedAnchor = myRB.transform.InverseTransformPoint(transform.position); 

                // Also transform the axis according to the joint main body.
                hingeJoint.axis = hitGO.transform.InverseTransformDirection(Vector3.forward);

                // Setup the motor.
                JointMotor motor = new JointMotor();
                motor.force = 10000;
                motor.targetVelocity = -50;
                hingeJoint.motor = motor;
                hingeJoint.useMotor = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!enabled)
        {
            return;
        }

        Gizmos.DrawLine(transform.position, transform.position + rayAxis * 100);
    }
}
