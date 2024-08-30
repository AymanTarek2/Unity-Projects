using UnityEngine;
using System.Collections.Generic;

public class GlobalHeightBasedGravity : MonoBehaviour
{
    public List<GameObject> monitoredObjects;  // List to store the GameObjects
    public float gravityThresholdY = 1.0f;     // The y value threshold
    public List<int> disableGravityIndices;    // Indices of objects to monitor for falling below the threshold

    private List<Rigidbody> rbList;            // List to store the Rigidbodies of the monitored objects

    void Start()
    {
        rbList = new List<Rigidbody>();

        // Initialize the Rigidbody list
        foreach (var obj in monitoredObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rbList.Add(rb);
            }
            else
            {
                Debug.LogWarning("GameObject " + obj.name + " does not have a Rigidbody component.");
            }
        }
    }

    void FixedUpdate()
    {
        foreach (int index in disableGravityIndices)
        {
            if (monitoredObjects[index].transform.position.y <= gravityThresholdY)
            {
                DisableAllGravity();
                return;
            }
        }

        EnableAllGravity();
    }

    private void DisableAllGravity()
    {
        foreach (var rb in rbList)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }

    private void EnableAllGravity()
    {
        foreach (var rb in rbList)
        {
            rb.useGravity = true;
        }
    }
}
