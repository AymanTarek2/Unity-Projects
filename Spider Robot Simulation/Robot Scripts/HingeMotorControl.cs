using UnityEngine;

public class HingeMotorControl : MonoBehaviour
{
    public KeyCode counterclockwiseKey = KeyCode.RightArrow;
    public KeyCode clockwiseKey = KeyCode.LeftArrow;
    public float maxMotorTorque = 1000f;
    private HingeJoint hingeJoint;
    private Rigidbody rb;
    private RigidbodyConstraints initialConstraints;
    private Vector3 currentForceApplied = Vector3.zero;
    private JointLimits jointLimits;

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();

        if (hingeJoint != null && rb != null)
        {
            JointMotor motor = hingeJoint.motor;
            motor.force = maxMotorTorque;
            motor.targetVelocity = 0;
            hingeJoint.motor = motor;
            hingeJoint.useMotor = true;

            initialConstraints = rb.constraints;
            jointLimits = hingeJoint.limits;
        }
        else
        {
            Debug.LogError("HingeJoint or Rigidbody component not found.");
        }
    }

    void Update()
    {
        if (hingeJoint != null && rb != null)
        {
            if (Input.GetKey(clockwiseKey) && hingeJoint.angle > jointLimits.min + 1f)
            {
                ApplyForce(transform.forward * 2);
                UnfreezeHingeAxis();
            }
            else if (Input.GetKey(counterclockwiseKey) && hingeJoint.angle < jointLimits.max - 1f)
            {
                ApplyForce(transform.forward * -5);
                UnfreezeHingeAxis();
            }
            else
            {
                RemoveAppliedForce();
                FreezeHingeAxis();
            }
        }
    }

    void ApplyForce(Vector3 force)
    {
        rb.AddForce(force);
        currentForceApplied = force;
    }

    void RemoveAppliedForce()
    {
        rb.AddForce(-currentForceApplied);
        currentForceApplied = Vector3.zero;
    }

    void FreezeHingeAxis()
    {
        Vector3 hingeAxis = hingeJoint.axis;
        RigidbodyConstraints freezeConstraints = initialConstraints;

        // Check hinge axis direction to freeze rotation on corresponding axis
        if (Mathf.Approximately(hingeAxis.x, 1f) || Mathf.Approximately(hingeAxis.x, -1f))
        {
            freezeConstraints |= RigidbodyConstraints.FreezeRotationX;
        }
        else if (Mathf.Approximately(hingeAxis.y, 1f) || Mathf.Approximately(hingeAxis.y, -1f))
        {
            freezeConstraints |= RigidbodyConstraints.FreezeRotationY;
        }
        else if (Mathf.Approximately(hingeAxis.z, 1f) || Mathf.Approximately(hingeAxis.z, -1f))
        {
            freezeConstraints |= RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            // For custom hinge axes, manually set constraints based on axis direction
            if (Mathf.Abs(hingeAxis.x) > 0.5f)
            {
                freezeConstraints |= RigidbodyConstraints.FreezeRotationX;
            }
            if (Mathf.Abs(hingeAxis.y) > 0.5f)
            {
                freezeConstraints |= RigidbodyConstraints.FreezeRotationY;
            }
            if (Mathf.Abs(hingeAxis.z) > 0.5f)
            {
                freezeConstraints |= RigidbodyConstraints.FreezeRotationZ;
            }
        }

        rb.constraints = freezeConstraints;
    }

    void UnfreezeHingeAxis()
    {
        rb.constraints = initialConstraints;
    }
}
