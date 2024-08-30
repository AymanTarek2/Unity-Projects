using UnityEngine;
using System.Collections.Generic;

public class ReceiverHandler : MonoBehaviour
{
    private static List<GameObject> receivers = new List<GameObject>();
    private DragHandler dragHandler;

    public void Initialize(DragHandler handler)
    {
        dragHandler = handler;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == dragHandler.draggedInstance)
        {
            return; // Avoid self-collision
        }

        if (!receivers.Contains(gameObject))
        {
            receivers.Add(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        receivers.Remove(gameObject);
    }

    public static List<GameObject> GetReceivers()
    {
        return receivers;
    }
}
