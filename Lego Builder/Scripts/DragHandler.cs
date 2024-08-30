using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject associatedGameObject;
    public float spawnDistance = 70f; // Public variable to control the spawn distance

    public KeyCode keyToDetect = KeyCode.Space;

    //public float cameraHeightIncreaseFactor = 20f; // Factor to increase camera height on drop
    public GameObject draggedInstance;
    private Camera mainCamera;
    private MeshProcessor meshProcessor;
    private Rigidbody prevDraggedRb;
    private CompatibilityChecker compatibilityChecker;

    private MeshStackManager meshStackManager;

    private void Start()
    {
        meshStackManager = MeshStackManager.Instance;
    }
    private void Awake()
    {
        mainCamera = Camera.main;
        meshProcessor = GetComponent<MeshProcessor>(); // Assuming MeshProcessor is attached to the same GameObject
        compatibilityChecker = GetComponent<CompatibilityChecker>();
    }
    private void Update()
    {
        if (draggedInstance != null && Input.GetKeyDown(KeyCode.R))
        {
            draggedInstance.transform.Rotate(Vector3.down, 90f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prevDraggedRb != null)
            prevDraggedRb.constraints |= RigidbodyConstraints.FreezePositionY;
        
        // Create a copy of the associated GameObject to drag
        draggedInstance = Instantiate(associatedGameObject);
        draggedInstance.transform.SetParent(null);  // Ensure it's not parented to the canvas

        // Position the dragged instance in front of the main camera at the specified distance
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = spawnDistance; // Use spawnDistance to set the z position
        draggedInstance.transform.position = mainCamera.ScreenToWorldPoint(mousePosition);

        // Assign a random color to the renderer
        Renderer renderer = draggedInstance.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color randomColor = GetRandomColor();
            renderer.material.color = randomColor;
        }

        // Add the ColliderTriggerHandler to handle collision events
        ColliHandler triggerHandler = draggedInstance.AddComponent<ColliHandler>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedInstance != null)
        {
            // Update the position of the dragged instance to follow the mouse
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = spawnDistance; // Use spawnDistance to set the z position
            draggedInstance.transform.position = mainCamera.ScreenToWorldPoint(mousePosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        Rigidbody draggedRb = draggedInstance.GetComponent<Rigidbody>();

        if (draggedRb != null)
        {
            draggedRb.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            draggedRb.freezeRotation = true;
            prevDraggedRb = draggedRb;
            Debug.Log("e7m");
        }
        

        // Calculate face info using MeshProcessor attached to draggedInstance
        MeshProcessor processor = draggedInstance.GetComponent<MeshProcessor>();
        if (processor != null)
        {
            processor.ProcessFace(Vector3.up); // Assuming positive y-axis direction
        }

        meshStackManager.addGameObjectsToList(draggedInstance);
        draggedInstance = null;
    }

    private Color GetRandomColor()
    {
        Color[] colors = { Color.red, Color.blue, Color.green };
        return colors[Random.Range(0, colors.Length)];
    }
}
