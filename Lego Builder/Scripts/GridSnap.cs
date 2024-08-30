using UnityEngine;

public class GridSnap : MonoBehaviour
{
    public float spacing = 2f;  // Spacing between grid points
    public Vector3 origin = Vector3.zero;  // Origin of the grid

    void Start()
    {
        SnapToGrid();
    }

    void SnapToGrid()
    {
        // Round position to nearest grid point
        Vector3 snappedPosition = RoundToNearestGrid(transform.position);
        transform.position = snappedPosition;
    }

    public Vector3 RoundToNearestGrid(Vector3 position)
    {
        // Calculate the position in grid coordinates
        Vector3 relativePosition = position - origin;
        Vector3 gridPosition = new Vector3(
            Mathf.Round(relativePosition.x / spacing) * spacing,
            position.y,  // Keep the y-coordinate unchanged
            Mathf.Round(relativePosition.z / spacing) * spacing
        );

        // Offset back to world coordinates
        return gridPosition + origin;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Example: Snap when colliding with objects having a box collider
        if (collision.collider is BoxCollider)
        {
            // Get the GameObject of the collider
            GameObject collidedObject = collision.gameObject;
            Debug.Log("Collided with: " + collidedObject.name);

            // Round x and z coordinates of the collided object
            //Vector3 roundedPosition = RoundToNearestGrid(collidedObject.transform.position);
            //collidedObject.transform.position = roundedPosition;
        }
    }
}
