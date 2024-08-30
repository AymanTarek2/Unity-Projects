using UnityEngine;
using UnityEngine.UI;

public class RawImageManager : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas
    public Texture[] textures; // Array to hold the images
    public GameObject[] gameObjects; // Array to hold the GameObjects
    public Vector2 imageSize = new Vector2(100, 100); // Size of each image
    public float spacing = 10f; // Space between images

    void Start()
    {
        CreateImages();
    }

    void CreateImages()
    {
        float startX = -((textures.Length - 1) * (imageSize.x + spacing)) / 2; // Center images

        for (int i = 0; i < textures.Length; i++)
        {
            GameObject newImage = new GameObject("RawImage");
            newImage.transform.SetParent(canvas.transform, false);

            RawImage rawImageComponent = newImage.AddComponent<RawImage>();
            rawImageComponent.texture = textures[i];

            RectTransform rectTransform = newImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = imageSize;

            // Set the pivot and anchor to the top center
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);

            // Set position to be at the top, centered horizontally
            rectTransform.anchoredPosition = new Vector2(startX + i * (imageSize.x + spacing), 0);

            // Add the DragHandler script and assign the corresponding GameObject
            DragHandler dragHandler = newImage.AddComponent<DragHandler>();
            dragHandler.associatedGameObject = gameObjects[i];
        }
    }
}
