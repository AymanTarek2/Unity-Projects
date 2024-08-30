using UnityEngine;

public class ServoArmController : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation
    private float currentRotation = 0f; // Current rotation angle

    void Update()
    {
        // Check for up arrow key press
        if (Input.GetKey(KeyCode.UpArrow))
        {
            RotateArm(rotationSpeed * Time.deltaTime);
        }

        // Check for down arrow key press
        if (Input.GetKey(KeyCode.DownArrow))
        {
            RotateArm(-rotationSpeed * Time.deltaTime);
        }
    }

    void RotateArm(float rotationAmount)
    {
        // Calculate new rotation
        float newRotation = currentRotation + rotationAmount;

        // Clamp the rotation between -90 and 90 degrees
        newRotation = Mathf.Clamp(newRotation, -90f, 90f);

        // Apply the rotation around the z-axis
        transform.localRotation = Quaternion.Euler(newRotation, -90, -90);

        // Update the current rotation
        currentRotation = newRotation;
    }
    
    public float getCurrentRotation()
    {
        return this.currentRotation;
    }
}
