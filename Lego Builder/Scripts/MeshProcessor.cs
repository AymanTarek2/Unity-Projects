using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.UI.Image;

public class MeshProcessor : MonoBehaviour
{
    private BoxCollider boxCollider;
    private List<float> angles = new List<float>();
    private List<float> lengths = new List<float>();

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("No BoxCollider found! Ensure there is a BoxCollider component attached.");
            return;
        }

        // Process the upper face (positive y-axis direction)
        //ProcessFace(Vector3.up);
    }

    public void ProcessFace(Vector3 direction)
    {
        List<Vector3> faceVertices = GetFaceVertices(direction);

        if (faceVertices.Count == 0)
        {
            Debug.Log($"No vertices found for face in direction {direction}");
            return;
        }

        //Debug.Log($"Processing face in direction {direction}");
        //Debug.Log($"Number of vertices: {faceVertices.Count}");

        CalculateAnglesAndLengths(faceVertices);

        // Store the face information within the MeshStackManager
        MeshStackManager.Instance.AddFaceInfo(new FaceInfo(angles, lengths, GetCurrentPosition()));
    }

    private List<Vector3> GetFaceVertices(Vector3 direction)
    {
        List<Vector3> faceVertices = new List<Vector3>();

        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size / 2;

        // Calculate the vertices in local space
        Vector3[] localVertices = new Vector3[4];
        localVertices[0] = new Vector3(-size.x, size.y, -size.z);
        localVertices[1] = new Vector3(size.x, size.y, -size.z);
        localVertices[2] = new Vector3(size.x, size.y, size.z);
        localVertices[3] = new Vector3(-size.x, size.y, size.z);

        // Determine the vertices of the face based on the direction
        if (direction == Vector3.up)
        {
            faceVertices.Add(transform.TransformPoint(localVertices[0]));
            faceVertices.Add(transform.TransformPoint(localVertices[1]));
            faceVertices.Add(transform.TransformPoint(localVertices[2]));
            faceVertices.Add(transform.TransformPoint(localVertices[3]));
        }

        return faceVertices;
    }

    private void CalculateAnglesAndLengths(List<Vector3> faceVertices)
    {
        // Calculate angles and lengths between consecutive vertices on the face
        angles.Clear();
        lengths.Clear();

        for (int i = 0; i < faceVertices.Count; i++)
        {
            Vector3 currentVertex = faceVertices[i];
            Vector3 nextVertex = faceVertices[(i + 1) % faceVertices.Count];
            Vector3 prevVertex = faceVertices[(i - 1 + faceVertices.Count) % faceVertices.Count];

            Vector3 v1 = (nextVertex - currentVertex).normalized;
            Vector3 v2 = (prevVertex - currentVertex).normalized;

            float angle = Vector3.Angle(v1, v2);
            angles.Add(angle);
            //Debug.Log($"Angle at vertex {i}: {angle} degrees");

            float distance = Vector3.Distance(currentVertex, nextVertex);
            lengths.Add(distance);
            //Debug.Log($"Length between vertex {i} and vertex {(i + 1) % faceVertices.Count}: {distance}");
        }
    }

    public Vector3 RoundToNearestGrid(Vector3 position)
    {
        // Calculate the position in grid coordinates
        Vector3 relativePosition = position;
        Vector3 gridPosition = new Vector3(
            Mathf.Round(relativePosition.x / 8) * 8,
            position.y,  // Keep the y-coordinate unchanged
            Mathf.Round(relativePosition.z / 8) * 8
        );

        // Offset back to world coordinates
        return gridPosition;
    }

    void SnapToGrid()
    {
        // Round position to nearest grid point
        Vector3 snappedPosition = RoundToNearestGrid(transform.position);
        transform.position = snappedPosition;
    }
    private Vector3 GetCurrentPosition()
    {
        Vector3 newPosition =  RoundToNearestGrid(this.transform.position);
        this.transform.position = newPosition;
        return transform.position;
    }
}
