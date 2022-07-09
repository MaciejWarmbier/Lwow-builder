using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isClickable;
    [SerializeField] bool isTree;
    [SerializeField] bool isRock;
    [SerializeField] BuildingSelectionCanvas buildingSelectionCanvas;
    [SerializeField] GameObject hoverMesh;
    

    private Building building;
    private Renderer tileRenderer;
    private GridManager gridManager;
    private Vector2Int coordinates = new Vector2Int();

    private bool isSelected = false;
    private bool isRockSmashed = false;
    private bool isTreeSmashed = false;

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

            if (!isClickable)
            {
                gridManager.BlockNode(coordinates);
            }
            if (isTree)
            {
                gridManager.CreateTreeResource(coordinates);
            }
            if (isRock)
            {
                gridManager.CreateRockResource(coordinates);
            }
        }
    }
    void OnMouseDown()
    {
        //TODO -=
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (gridManager.GetNode(coordinates).isClickable)
            {
                var canvas = Instantiate(buildingSelectionCanvas);
                canvas.OnCanvasClosed += OnBuildingSelection;
            }
            else if (gridManager.GetNode(coordinates).isTree && !isTreeSmashed)
            {
                StartCoroutine("SmashThatTree");
            }
            else if (gridManager.GetNode(coordinates).isRock && !isRockSmashed)
            {
                StartCoroutine("SmashThatRock");
            }
        }
    }

    private IEnumerator SmashThatTree()
    {
        isTreeSmashed = true;
        int number = Random.Range(1, 2);
        VillageResources.villageResources.ChangeResources(number);

        yield return new WaitForSeconds(1);
        isTreeSmashed = false;
    }

    private IEnumerator SmashThatRock()
    {
        isRockSmashed = true;
        int number = Random.Range(0, 3);
        VillageResources.villageResources.ChangeResources(number);

        yield return new WaitForSeconds(1);
        isRockSmashed = false;
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
                VillageResources.villageResources.ChangeBuildingScore(boughtBuilding.BuildingScore);
                VillageResources.villageResources.ChangeFoodProduction(boughtBuilding.FoodProduction);
                VillageResources.villageResources.ChangeResourcesProduction(boughtBuilding.ResourcesProduction);
                VillageResources.villageResources.ChangeMoraleProduction(boughtBuilding.MoraleProduction);
                WorldController.worldController.CheckEvent();
            }
        }
        else
        {
            StopHover();
        }
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && (isClickable || isTree || isRock))
        {
            Hover();
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && (isClickable || isTree || isRock))
        {
            StopHover();
        }
    }

    private void Hover()
    {
        if (!isSelected)
        {
            isSelected = true;
            hoverMesh.SetActive(true);
            tileRenderer.material.SetColor("_Color", Color.red);
        }
    }

    private void StopHover()
    {
        if (isSelected)
        {
            isSelected = false;
            hoverMesh.SetActive(false);
            tileRenderer.material.SetColor("_Color", Color.green);
        }
    }
}
