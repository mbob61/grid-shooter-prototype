using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManagerV3 : MonoBehaviour
{
    [SerializeField] private GameObject mouseCursorIndicator;
    [SerializeField] private InputManagerV2 inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDatabase database;
    [SerializeField] private PreviewSystemV2 preview;

    private int currentlySelectedObjectID = -1;
    private List<GameObject> placedGameObjects = new();
    private GridData objectData, floorData;

    private Transform rotatorTransform;
    private ObjectData previewObject = null;

    private Vector3Int placementGridPosition = Vector3Int.one;
    private bool currentGridPositionPlacementValidity = false;

    private void Start()
    {
        rotatorTransform = FindObjectOfType<PlatformRotator>().gameObject.transform;
        objectData = new GridData();
        floorData = new GridData();
    }

    private void Update()
    {
        Vector3 mousePosition = getMousePosition();

        // Infinity returned if the mouse isn't on the grid so we have something constant to compare to.
        // If we DO NOT have valid mouse position, kill the previews.
        // If we DO have a valid mouse position, create the previews if they don't exist.
        if (mousePosition.x == Mathf.Infinity)
        {
            preview.stopShowingPreview();
            return;
        }
        preview.UpdateCursorIndicatorPosition(mousePosition, grid.transform.position.y);


        if (Input.GetKeyDown(KeyCode.Z))
        {
            IncrementObjectID();
        }

        if (currentlySelectedObjectID < 0)
        {
            //Stop showing the preview if we don't have an object to spawn
            preview.stopShowingPreview();
            return;
        } else
        {
            // Display the preview if we have something to preview
            preview.startShowingPreview(previewObject.Prefab, previewObject.Size);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            PlaceObject(currentlySelectedObjectID);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            RemoveObject();
        }

        placementGridPosition = getGridPosition(mousePosition);
        currentGridPositionPlacementValidity = CheckPlacementValidity(currentlySelectedObjectID, placementGridPosition);

        preview.UpdatePositionAndColor(grid.CellToWorld(placementGridPosition), currentGridPositionPlacementValidity);
    }

    private void RemoveFloor()
    {

    }

    private void RemoveObject()
    {

        // Add a second method (on a different key), for removing floor as well
        // When we remove floor, also then call this to check for removing the object on top

        Vector3Int gridPosition = getGridPosition();
        if (!objectData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            int index = objectData.RemoveObjectAt(gridPosition);
            Destroy(placedGameObjects[index]);


            // Investigate using array remove instead of index = null.
            // We would need to go through each object inside the GridData objects
            // and potentially update their `placedObjectIndex` if appropriate (decrement if bigger than this index, otherwise leave alone)
            placedGameObjects[index] = null;
        }
    }

    private void PlaceObject(int objectID)
    {
        if (objectID > -1)
        {
            Vector3Int gridPosition = getGridPosition();

            if (CheckPlacementValidity(objectID, gridPosition))
            {
                ObjectData objectToPlace = database.objects[objectID];

                GameObject newObject = Instantiate(objectToPlace.Prefab);
                newObject.transform.position = grid.CellToWorld(gridPosition);
                newObject.transform.parent = grid.transform;
                newObject.transform.rotation = rotatorTransform.localRotation;

                placedGameObjects.Add(newObject);

                GetCorrectGridDataForObject().AddObjectAt(gridPosition,
                    objectToPlace.Size,
                    objectToPlace.ID,
                    placedGameObjects.Count - 1);
            }
        }
    }

    private bool CheckPlacementValidity(int id, Vector3Int gridPosition)
    {
        GridData selectedData = GetCorrectGridDataForObject();
        return selectedData.canPlaceObjectAt(gridPosition, database.objects[id].Size);
    }

    private void IncrementObjectID()
    {
        preview.stopShowingPreview();
        currentlySelectedObjectID++;
        if (currentlySelectedObjectID >= database.objects.Count)
        {
            currentlySelectedObjectID = -1;
        }
        else
        {
            previewObject = database.objects[currentlySelectedObjectID];
        }
    }

    private Vector3 getMousePosition()
    {
        return inputManager.GetHoveredMousePosition();
    }

    private Vector3Int getGridPosition()
    {
        Vector3 mousePosition = getMousePosition();
        return grid.WorldToCell(mousePosition);
    }

    private Vector3Int getGridPosition(Vector3 mousePosition)
    {
        return grid.WorldToCell(mousePosition);
    }

    private GridData GetCorrectGridDataForObject()
    {
        return database.objects[currentlySelectedObjectID].ID == 0 ? floorData : objectData;
    }

}