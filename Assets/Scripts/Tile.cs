using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] BuildingSelectionCanvas buildingSelectionCanvas;
    [SerializeField] GameObject hoverMesh;

    private Building building;
    private Renderer tileRenderer;
    private GridManager gridManager;
    private Vector2Int coordinates = new Vector2Int();

    public bool IsPlaceable { get { return isPlaceable; } }
    private bool isSelected = false;

    private void Awake()
    {
        tileRenderer = GetComponentInChildren<Renderer>();
        gridManager = FindObjectOfType<GridManager>();
    }
    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosotion(transform.position);

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }
    void OnMouseDown()
    {
        //TODO -=
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (gridManager.GetNode(coordinates).isUsable)
            {
                var canvas = Instantiate(buildingSelectionCanvas);
                canvas.OnCanvasClosed += OnBuildingSelection;
            }
        }
    }

    private void OnBuildingSelection(bool isBought, Building boughtBuilding)
    {
        if (isBought)
        {
            Vector3 buildingPosition = transform.position;
            buildingPosition.y += 2;
            GameObject createdBuilding = boughtBuilding.CreateBuilding(boughtBuilding, buildingPosition);
            if (createdBuilding != null)
            {
                building = createdBuilding.GetComponent<Building>();
                building.HoveredOver += Hover;
                building.StoppedHover += StopHover;
                gridManager.BlockNode(coordinates);
                StopHover();
            }
        }
        else
        {
            StopHover();
        }
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Hover();
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            StopHover();
        }
    }

    private void Hover()
    {
        if (!isSelected)
        {
            isSelected = true;
            hoverMesh.active = true;
            tileRenderer.material.SetColor("_Color", Color.red);
        }
    }

    private void StopHover()
    {
        if (isSelected)
        {
            isSelected = false;
            hoverMesh.active = false;
            tileRenderer.material.SetColor("_Color", Color.green);
        }
    }
}
