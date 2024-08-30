using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeTorque : MonoBehaviour
{
    public KeyCode counterclockwiseKey = KeyCode.RightArrow;
    public KeyCode clockwiseKey = KeyCode.LeftArrow;
    public float rotationSpeed = 100f; // Speed of rotation
    public float motorForce = 1000f; // Force to resist external forces like gravity
    public float springForce = 10000f; // Spring force to counteract gravity
    private HingeJoint hingeJoint;
    private Rigidbody rb;
    private JointLimits jointLimits;

    private float currentVelocity;
    void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();

        if (hingeJoint != null && rb != null)
        {
            rb.useGravity = false; // Disable gravity for the Rigidbody

            JointMotor motor = hingeJoint.motor;
            motor.force = motorForce;
            motor.targetVelocity = 0;
            hingeJoint.motor = motor;
            hingeJoint.useMotor = true;

            JointSpring spring = hingeJoint.spring;
            spring.spring = springForce;
            spring.damper = 0;
            hingeJoint.spring = spring;
            hingeJoint.useSpring = true;

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
            JointMotor motor = hingeJoint.motor;

            if (Input.GetKey(clockwiseKey) && hingeJoint.angle > jointLimits.min + 1f)
            {
                motor.targetVelocity = -rotationSpeed;
                currentVelocity = motor.targetVelocity;
                hingeJoint.useMotor = true;
            }
            else if (Input.GetKey(counterclockwiseKey) && hingeJoint.angle < jointLimits.max - 1f)
            {
                motor.targetVelocity = rotationSpeed;
                currentVelocity = motor.targetVelocity;
                hingeJoint.useMotor = true;
            }
            else
            {
                motor.targetVelocity = - currentVelocity;
                hingeJoint.useMotor = false; // Disable motor to use spring
            }

            hingeJoint.motor = motor;
        }
    }
}
