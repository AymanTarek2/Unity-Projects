using UnityEngine;

public class HingeChainControl : MonoBehaviour
{
    public KeyCode counterclockwiseKey1 = KeyCode.RightArrow;
    public KeyCode clockwiseKey1 = KeyCode.LeftArrow;
    public KeyCode counterclockwiseKey2 = KeyCode.UpArrow;
    public KeyCode clockwiseKey2 = KeyCode.DownArrow;
    public KeyCode counterclockwiseKey3 = KeyCode.W;
    public KeyCode clockwiseKey3 = KeyCode.S;
    public float rotationSpeed = 100f; // Speed of rotation
    public HingeJoint hingeJoint1; // First hinge joint
    public HingeJoint hingeJoint2; // Second hinge joint
    public HingeJoint hingeJoint3; // Third hinge joint

    void Start()
    {
        InitializeHingeJoint(hingeJoint1);
        InitializeHingeJoint(hingeJoint2);
        InitializeHingeJoint(hingeJoint3);
    }

    void Update()
    {
        UpdateHingeJoint(hingeJoint1, clockwiseKey1, counterclockwiseKey1);
        UpdateHingeJoint(hingeJoint2, clockwiseKey2, counterclockwiseKey2);
        UpdateHingeJoint(hingeJoint3, clockwiseKey3, counterclockwiseKey3);
    }

    void InitializeHingeJoint(HingeJoint hingeJoint)
    {
        if (hingeJoint != null)
        {
            Rigidbody rb = hingeJoint.GetComponent<Rigidbody>();
            rb.useGravity = false; // Disable gravity for the Rigidbody
        }
        else
        {
            Debug.LogError("HingeJoint component not found.");
        }
    }

    void UpdateHingeJoint(HingeJoint hingeJoint, KeyCode clockwiseKey, KeyCode counterclockwiseKey)
    {
        if (hingeJoint != null)
        {
            Rigidbody rb = hingeJoint.GetComponent<Rigidbody>();
            Vector3 torque = hingeJoint.axis * rotationSpeed;

            if (Input.GetKey(clockwiseKey))
            {
                rb.AddTorque(-torque);
                ApplyOppositeTorqueToChild(hingeJoint, torque);
            }
            else if (Input.GetKey(counterclockwiseKey))
            {
                rb.AddTorque(torque);
                ApplyOppositeTorqueToChild(hingeJoint, -torque);
            }
        }
    }

    void ApplyOppositeTorqueToChild(HingeJoint parentJoint, Vector3 torque)
    {
        if (parentJoint != null && parentJoint.connectedBody != null)
        {
            HingeJoint childJoint = parentJoint.connectedBody.GetComponent<HingeJoint>();
            if (childJoint != null)
            {
                Rigidbody childRb = childJoint.GetComponent<Rigidbody>();
                if (childRb != null)
                {
                    childRb.AddTorque(-torque); // Apply opposite torque to child hinge joint
                }
                else
                {
                    Debug.LogError("Rigidbody component not found on child hingeJoint's connected body: " + parentJoint.connectedBody.name);
                }
            }
            else
            {
                Debug.LogWarning("Child HingeJoint component not found on connected body of hingeJoint: " + parentJoint.name);
            }
        }
        else
        {
            // No connected body (child) or parent joint is null
            Debug.LogWarning("Parent joint or connected body is null.");
        }
    }
}
