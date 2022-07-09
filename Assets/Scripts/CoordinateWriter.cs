using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateWriter : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.red;
    [SerializeField] Color pathColor = Color.blue;
    [SerializeField] Color exploredColor = Color.yellow;

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

        ObjectNode node = gridManager.GetNode(coordinates);
        if (node == null) return;

        if (node.isClickable)
        {
            coordinateLabel.color = defaultColor;
        }
        else if (node.isTree || node.isRock)
        {
            coordinateLabel.color = exploredColor;
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
