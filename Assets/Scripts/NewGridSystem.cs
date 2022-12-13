using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSizeSnap = 10;
    Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();
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
                grid.Add(coordinates, null);
            }
        }
    }

    public void AddTile(Tile tile)
    {
        var tilePosition = tile.gameObject.transform.position;
        var coordinates = GetCoordinatesFromPosition(tilePosition);
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates] = tile;
        }
        else
        {
            grid.Add(coordinates, tile);
        }
    }

    public Tile GetTile(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }

    public List<Tile> GetNeighbors(Tile tile)
    {
        var tilePosition = tile.gameObject.transform.position;
        var coordinates = GetCoordinatesFromPosition(tilePosition);

        List<Tile> neighbors = new List<Tile>();

        if (grid.ContainsKey(coordinates))
        {
            for(int i = -1; i < 2; i++)
            {
                for(int j=-1; j<2;j++)
                {
                    if(i != 0 && j != 0)
                    {
                        neighbors.Add(grid[new Vector2(coordinates.x - i, coordinates.y - j)]);
                    }
                }
            }
        }

        return neighbors;
    }

    public List<Tile> GetBigTileNeighbors(List<Tile> tiles)
    {
        List<Tile> bigBuildingNeighbors = new List<Tile>();
        foreach (var tile in tiles)
        {
            bigBuildingNeighbors.AddRange(GetNeighbors(tile));
        }

        foreach(var tile in tiles)
        {
            bigBuildingNeighbors.RemoveAll(item => item.gameObject.transform.position == tile.gameObject.transform.position);
        }

        return bigBuildingNeighbors;
    }

    public List<Tile> GetBigBuildingTiles(Tile tile)
    {
        var tilePosition = tile.gameObject.transform.position;
        var coordinates = GetCoordinatesFromPosition(tilePosition);

        List<Tile> bigBuildingList = new List<Tile>();

        if (grid.ContainsKey(coordinates))
        {
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    bigBuildingList.Add(grid[new Vector2(coordinates.x - i, coordinates.y - j)]);
                }
            }
        }

        return bigBuildingList;
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();


        coordinates.x = Mathf.RoundToInt(position.x / unityGridSizeSnap);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSizeSnap);

        return coordinates;
    }
}
