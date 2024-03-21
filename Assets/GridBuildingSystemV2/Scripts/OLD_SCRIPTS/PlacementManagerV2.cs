using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManagerV2 : MonoBehaviour
{
    [SerializeField] private GameObject mouseCursorIndicator;
    [SerializeField] private InputManagerV2 inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDatabase database;
    [SerializeField] private PreviewSystem preview;

    private int currentlySelectedObjectID = -1;
    private List<GameObject> placedGameObjects = new();
    private GridData objectData, floorData;

    private Transform rotatorTransform;

    private void Start()
    {
        rotatorTransform = FindObjectOfType<PlatformRotator>().gameObject.transform;
        objectData = new GridData();
        floorData = new GridData();
    }

    private void Update()
    {
        Vector3 mousePosition = getMousePosition();
        if (mousePosition == Vector3.zero)
        {
            IncrementObjectID(0);
            return;
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            IncrementObjectID(currentlySelectedObjectID + 1);
        }
        if (currentlySelectedObjectID < 0)
        {
            return;
        }

       
        mouseCursorIndicator.transform.position = new Vector3(mousePosition.x, grid.transform.position.y, mousePosition.z);



        if (Input.GetKeyDown(KeyCode.X))
        {
            PlaceObject(currentlySelectedObjectID);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            RemoveObject();
        }
        Vector3Int gridPosition = getGridPosition(mousePosition);

        bool validity = CheckPlacementValidity(currentlySelectedObjectID, gridPosition);

        preview.UpdatePositionAndColor(grid.CellToWorld(gridPosition), validity);
        preview.setHighlightedCellColor(validity);
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
        return selectedData.canPlaceObjectAt(gridPosition,  database.objects[id].Size);
    }

    private void IncrementObjectID(int newID)
    {
        preview.stopShowingPreview();
        currentlySelectedObjectID = newID;
        if (currentlySelectedObjectID >= database.objects.Count)
        {
            currentlySelectedObjectID = -1;
            preview.setHighlitedCellState(false);

        } else
        {
            print("am i ending up here?");
            preview.startShowingPlacementPreview(database.objects[currentlySelectedObjectID].Prefab, database.objects[currentlySelectedObjectID].Size);
            preview.setHighlitedCellState(true);
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