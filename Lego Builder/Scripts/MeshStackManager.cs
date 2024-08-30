using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MeshStackManager : MonoBehaviour
{
    private static MeshStackManager instance;
    public static MeshStackManager Instance { get { return instance; } }
    private Stack<FaceInfo> faceInfoStack = new Stack<FaceInfo>();

    private static List<GameObject> legoGameObjects = new List<GameObject>();

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void AddFaceInfo(FaceInfo faceInfo)
    {
        faceInfoStack.Push(faceInfo);
        Debug.Log("Added face information to stack.");
        Debug.Log($"Angles: [{string.Join(", ", faceInfo.Angles)}]");
        Debug.Log($"Lengths: [{string.Join(", ", faceInfo.Lengths)}]");
        Debug.Log($"Position: {faceInfo.Position}");

    }

    public void addGameObjectsToList(GameObject gameObject)
    {
        legoGameObjects.Add(gameObject);
        Debug.Log("object added");
    }

    public void DestroyLastObject(GameObject obj1, GameObject obj2) {

        Debug.Log("access to destroying");

        if (obj1 == null || obj2 == null)
        {
            Debug.Log("something is nullinggggg");
            return;
        }
        for (int i = legoGameObjects.Count - 1; i >= 0; i--)
        {
            if (obj1 == legoGameObjects[i].gameObject || obj2 == legoGameObjects[i].gameObject)
            {
                Destroy(legoGameObjects[i]);
                legoGameObjects.Remove(legoGameObjects[i]);
                Debug.Log("Destroying Object zzzzzzzzzzzzz");
                return;
            }
        }
    }
}
