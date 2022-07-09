using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    Dictionary<Vector2Int, ObjectNode> grid = new Dictionary<Vector2Int, ObjectNode>();
    public Dictionary<Vector2Int, ObjectNode> Grid { get { return grid; } }
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSizeSnap = 10;
    public int UnityGridSizeSnap { get { return unityGridSizeSnap; } }

    private void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new ObjectNode(coordinates, true));
            }
        }
    }

    public ObjectNode GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isClickable = false;
        }
    }
    
    public void CreateResource(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isResource = true;
        }
    }

    public Vector2Int GetCoordinatesFromPosotion(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();


        coordinates.x = Mathf.RoundToInt(position.x / unityGridSizeSnap);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSizeSnap);

        return coordinates;
    }
}
