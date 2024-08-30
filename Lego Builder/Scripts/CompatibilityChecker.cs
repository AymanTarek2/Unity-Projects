using UnityEngine;

public class CompatibilityChecker : MonoBehaviour
{
    public bool CheckCompatibility(GameObject sender, GameObject receiver)
    {
        Debug.Log("Processing Compatibility here");
        return true; // Example: Always return true for now
    }

    public void ProcessCompatibility(GameObject sender)
    {
        // Placeholder for further processing after placement
        // Replace with your processing logic
        Debug.Log($"Placed {sender.name} successfully.");
    }
}
