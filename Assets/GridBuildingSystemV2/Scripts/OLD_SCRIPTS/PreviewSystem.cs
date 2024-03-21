
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOfset = 0.02f;

    [SerializeField] private GameObject highlightedCellIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private Transform rotatorTransform;

    // Start is called before the first frame update
    void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        highlightedCellIndicator.SetActive(false);
        cellIndicatorRenderer = highlightedCellIndicator.GetComponentInChildren<Renderer>();
        rotatorTransform = FindObjectOfType<PlatformRotator>().gameObject.transform;
    }

    internal void startShowingRemovePreview()
    {
        PrepareIndicator(Vector2Int.one);
    }

    public void startShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        previewObject.transform.parent = this.transform;
        foreach (MonoBehaviour a in previewObject.GetComponents<MonoBehaviour>())
        {
            a.enabled = false;
        }

        PreparePreview(prefab);
        PrepareIndicator(size);
        highlightedCellIndicator.SetActive(true);
    }

    private void PrepareIndicator(Vector2Int size)
    {
        if (size.x > 0 && size.y > 0)
        {
            highlightedCellIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
        }
        cellIndicatorRenderer.material.mainTextureScale = size;
    }

    private void PreparePreview(GameObject prefab)
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

    public void stopShowingPreview()
    {
        highlightedCellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void UpdatePositionAndColor(Vector3 position, bool validity)
    {
        highlightedCellIndicator.transform.position = position;
        highlightedCellIndicator.transform.localRotation = Quaternion.Euler(0, rotatorTransform.localEulerAngles.y, 0);

        previewObject.transform.position = new Vector3(position.x, position.y + previewYOfset, position.z);
        previewObject.transform.localRotation = Quaternion.Euler(0, rotatorTransform.localEulerAngles.y, 0);


        Color c = !validity ? Color.red : Color.white;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
        previewMaterialInstance.color = c;
    }

    public void setHighlitedCellState(bool state)
    {
        highlightedCellIndicator.SetActive(state);
    }

    public void setHighlightedCellColor(bool validity)
    {
        highlightedCellIndicator.GetComponentInChildren<SpriteRenderer>().color = validity ? Color.green : Color.red;

    }
}


