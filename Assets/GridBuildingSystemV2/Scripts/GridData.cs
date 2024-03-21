using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex){
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, id, placedObjectIndex);
        foreach (var position in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(position))
            {
                throw new Exception($"Dictionary already contains this cell position {position}");
            } else
            {
                placedObjects[position] = data;
            }
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> values = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                values.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return values;
    }

    public bool canPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }


    public int RemoveObjectAt(Vector3Int gridPosition)
    {
        PlacementData placedObject = placedObjects[gridPosition];
        if (placedObject.occupiedPositions.Count > 0)
        {
            foreach (Vector3Int pos in placedObject.occupiedPositions)
            {
                placedObjects.Remove(pos);
            }
            return placedObject.placeObjectIndex;
        }
        return -1;
    }


    //internal void RemoveObjectAt(Vector3Int gridPosition)
    //{
    //    foreach (var position in placedObjects[gridPosition].occupiedPositions)
    //    {
    //        placedObjects.Remove(position);
    //    }
    //}

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition))
        {
            return -1;
        } else
        {
            return placedObjects[gridPosition].placeObjectIndex;
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int placeObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placeObjectIndex){
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        this.placeObjectIndex = placeObjectIndex;
    }
}