using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystemV2 : MonoBehaviour
{
    [SerializeField] private float previewOfsetY = 0.02f;

    [SerializeField] private GameObject highlightedCellIndicator, mouseCursorIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer highlightedCellIndicatorRenderer;

    private Transform rotatorTransform;

    // Start is called before the first frame update
    void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        highlightedCellIndicatorRenderer = highlightedCellIndicator.GetComponentInChildren<Renderer>();
        rotatorTransform = FindObjectOfType<PlatformRotator>().gameObject.transform;
    }

    public void UpdateCursorIndicatorPosition(Vector3 mousePosition, float gridYPosition)
    {
        mouseCursorIndicator.transform.position = new Vector3(mousePosition.x, gridYPosition, mousePosition.z);
    }

    public void startShowingPreview(GameObject prefab, Vector2Int size)
    {
        highlightedCellIndicator.SetActive(true);
        if (previewObject == null)
        {
            previewObject = Instantiate(prefab);
            previewObject.transform.parent = this.transform;

            // Disable all the scripts on the previews, we're only interested in the models
            foreach (MonoBehaviour a in previewObject.GetComponents<MonoBehaviour>())
            {
                a.enabled = false;
            }

            PreparePrefabPreview();
            PrepareHighlightedCellPreview(size);
        }
    }

    public void stopShowingPreview()
    {
        highlightedCellIndicator.SetActive(false);
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }

    public void startShowingRemovePreview()
    {
        highlightedCellIndicator.SetActive(true);
        PrepareHighlightedCellPreview(Vector2Int.one);
    }

    private void PreparePrefabPreview()
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    private void PrepareHighlightedCellPreview(Vector2Int size)
    {
        // Set the size of the highlighted cell indicator to match the size of the desired prefab to spawn
        if (size.x > 0 && size.y > 0)
        {
            highlightedCellIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
        }
    }

    public void UpdatePositionAndColor(Vector3 position, bool validity)
    {
        highlightedCellIndicator.transform.position = position;
        highlightedCellIndicator.transform.localRotation = Quaternion.Euler(0, rotatorTransform.localEulerAngles.y, 0);

        previewObject.transform.position = new Vector3(position.x, position.y + previewOfsetY, position.z);
        previewObject.transform.localRotation = Quaternion.Euler(0, rotatorTransform.localEulerAngles.y, 0);


        Color previewColor = !validity ? Color.red : Color.white;
        previewColor.a = 0.5f;
        previewMaterialInstance.color = previewColor;

        Color highlightedCellColor = !validity ? Color.red : Color.green;
        highlightedCellColor.a = 0.5f;
        highlightedCellIndicatorRenderer.material.color = highlightedCellColor;
    }
}


