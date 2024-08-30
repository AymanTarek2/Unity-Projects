using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of camera movement
    public bool shouldClampY = true;  // Whether to clamp Y position or not
    public float maxY = 10f;      // Maximum Y position
    public float minY = 1f;       // Minimum Y position

    void Update()
    {
        // Move camera up when holding 'W'
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        }

        // Move camera down when holding 'S'
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
        }

        // Optionally clamp camera's Y position
        if (shouldClampY)
        {
            Vector3 clampedPosition = transform.position;
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
            transform.position = clampedPosition;
        }
    }
}
