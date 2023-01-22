using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateWriter : MonoBehaviour
{
    [SerializeField] Color freeColor = Color.white;
    [SerializeField] Color blockedColor = Color.red;
    [SerializeField] Color usedColor = Color.yellow;

    TextMeshPro coordinateLabel;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;


    void Awake()
    {
        coordinateLabel = GetComponent<TextMeshPro>();
        coordinateLabel.enabled = false;
        gridManager = FindObjectOfType<GridManager>();
        DisplayCoordinates();
    }
    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            coordinateLabel.enabled = true;
        }

        SetLabelColor();
        ToggleLabels();
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            coordinateLabel.enabled = !coordinateLabel.IsActive();
        }
    }

    void SetLabelColor()
    {
        if (gridManager == null)
        {
            return;
        }

        Tile tile = gridManager.GetTile(coordinates);
        if (tile == null) return;

        if (tile.Type == Tile.TileType.Free)
        {
            coordinateLabel.color = freeColor;
        }
        else if (tile.Type == Tile.TileType.Tree || tile.Type == Tile.TileType.Rock || tile.Type == Tile.TileType.Built)
        {
            coordinateLabel.color = usedColor;
        }
        else 
        {
            coordinateLabel.color = blockedColor;
        }
    }

    void DisplayCoordinates()
    {
        if (gridManager == null) return;

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSizeSnap);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSizeSnap);

        coordinateLabel.text = coordinates.x + "," + coordinates.y;
    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}
