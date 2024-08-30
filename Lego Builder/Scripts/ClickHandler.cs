using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject associatedGameObject;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Spawn a copy of the associated GameObject
        GameObject spawnedInstance = Instantiate(associatedGameObject, associatedGameObject.transform.position, associatedGameObject.transform.rotation);

        // Optionally set the spawnedInstance's parent to canvas or any other desired transform
        spawnedInstance.transform.SetParent(canvas.transform, false);
    }
}
