using UnityEngine;

public class Link1Rotation : MonoBehaviour
{
    public float forceAmount = 10f; // Force to apply for rotation
    public KeyCode leftKey = KeyCode.LeftArrow; // Key to rotate left
    public KeyCode rightKey = KeyCode.RightArrow; // Key to rotate right

    private HingeJoint hinge;
    private JointLimits limits;
    private Rigidbody rb;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        limits = hinge.limits;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(leftKey))
        {
            ApplyForce(-forceAmount);
        }
        else if (Input.GetKey(rightKey))
        {
            ApplyForce(forceAmount);
        }
        else
        {
            NullifyForce();
        }
    }

    void ApplyForce(float force)
    {
        // Ensure the current angle does not exceed the hinge limits before applying force
        if ((hinge.angle <= limits.min && force < 0) || (hinge.angle >= limits.max && force > 0))
        {
            return;
        }

        // Apply force to the rigidbody
        rb.AddForceAtPosition(transform.right * force, hinge.transform.position);
    }

    void NullifyForce()
    {
        // Nullify the acting force to stop the rotation
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
