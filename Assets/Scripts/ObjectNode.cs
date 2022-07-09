using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNode
{
    public Vector2Int coordinates;
    public bool isUsable;
    public bool isCleared;
    public bool isTaken;

    public ObjectNode(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isUsable = isWalkable;
    }
}
