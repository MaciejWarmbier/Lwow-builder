using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNode
{
    public Vector2Int coordinates;
    public bool isClickable;
    public bool isRock;
    public bool isTree;

    public ObjectNode(Vector2Int coordinates, bool isClickable)
    {
        this.coordinates = coordinates;
        this.isClickable = isClickable;
    }
}
