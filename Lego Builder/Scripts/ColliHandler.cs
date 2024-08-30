using UnityEngine;

public class ColliHandler : MonoBehaviour
{
    private MeshStackManager meshStackManager;

    public static bool collisionHandled = false;

    private void Start()
    {
        meshStackManager = MeshStackManager.Instance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        Debug.Log($"Collided with {collidedObject.name}");

        
        // Perform custom actions based on collision with specific tag
        Debug.Log($"Custom collision condition met with {collidedObject.name}");

        if (!isCompatible() && collidedObject.tag != "Plane" && !collisionHandled)
        {
            meshStackManager.DestroyLastObject(this.gameObject, collidedObject);
            collisionHandled = true;
        }
        if(collisionHandled)
        {
            collisionHandled = false;
        }
    }

    public bool isCompatible()
    {
        //should be implemented
        return true;
    }


}
