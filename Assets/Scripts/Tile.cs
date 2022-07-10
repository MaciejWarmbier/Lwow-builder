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
    [SerializeField] AudioSource soundEffect;


    private Building building;
    private Renderer tileRenderer;
    private GridManager gridManager;
    private Vector2Int coordinates = new Vector2Int();
    private Vector3 corner = new Vector3();
    private List<Tile> neighbors = new List<Tile>();

    private bool isSelected = false;
    private bool isRockSmashed = false;
    private bool isTreeSmashed = false;
    private bool isPossibleToPlace = true;

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
                if(BuildingsController.buildingsController.buildingInProgress != null)
                {
                    if (BuildingsController.buildingsController.buildingInProgress.IsBig)
                    {
                        if(isPossibleToPlace)
                        {
                            building = BuildingsController.buildingsController.buildingInProgress;
                            foreach(Tile neighbor in neighbors)
                            {
                                gridManager.BlockNode(neighbor.coordinates);
                                neighbor.NonClickable();
                            }
                            StopHover();
                            BuildingsController.buildingsController.buildingInProgress.PlaceOnTile(corner);
                            NonClickable();
                            gridManager.BlockNode(coordinates);
                        }
                    }
                    else
                    {
                        building = BuildingsController.buildingsController.buildingInProgress;
                        BuildingsController.buildingsController.buildingInProgress.PlaceOnTile(gameObject.transform.position);
                        isClickable = false;
                    }
                }
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
        soundEffect.Play();
        yield return new WaitForSeconds(1);
        isTreeSmashed = false;
    }

    private IEnumerator SmashThatRock()
    {
        isRockSmashed = true;
        int number = Random.Range(0, 3);
        VillageResources.villageResources.ChangeResources(number);
        soundEffect.Play();
        yield return new WaitForSeconds(1);
        isRockSmashed = false;
    }

    //private void OnBuildingSelection(bool isBought, Building boughtBuilding)
    //{
    //    if (isBought)
    //    {
    //        Vector3 buildingPosition = transform.position;
    //        buildingPosition.y += 2;
    //        GameObject createdBuilding = boughtBuilding.CreateBuilding(boughtBuilding, buildingPosition);
    //        if (createdBuilding != null)
    //        {
    //            building = createdBuilding.GetComponent<Building>();
    //            building.HoveredOver += Hover;
    //            building.StoppedHover += StopHover;
    //            gridManager.BlockNode(coordinates);
    //            StopHover();
    //            VillageResources.villageResources.ChangeBuildingScore(boughtBuilding.BuildingScore);
    //            VillageResources.villageResources.ChangeFoodProduction(boughtBuilding.FoodProduction);
    //            VillageResources.villageResources.ChangeResourcesProduction(boughtBuilding.ResourcesProduction);
    //            VillageResources.villageResources.ChangeMoraleProduction(boughtBuilding.MoraleProduction);
    //            WorldController.worldController.CheckEvent();
    //        }
    //    }
    //    else
    //    {
    //        StopHover();
    //    }
    //}

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && (isClickable || isTree || isRock))
        {
            Hover();
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && hoverMesh.activeSelf)
        {
            StopHover();
        }
    }

    public bool HoveredByBuilding()
    {
        hoverMesh.SetActive(true);
        tileRenderer.material.SetColor("_Color", Color.red);

        if (!isClickable || isRock || isTree)
        {
            return false;
        }

        return true;
    }

    public void UnHoveredByBuilding()
    {
        hoverMesh.SetActive(false);
        tileRenderer.material.SetColor("_Color", Color.green);
        isSelected = false;
    }

    public void NonClickable()
    {
        isClickable = false;
        hoverMesh.SetActive(false);
        tileRenderer.material.SetColor("_Color", Color.green);
    }

    private void Hover()
    {
        if (!isSelected)
        {
            isSelected = true;
            hoverMesh.SetActive(true);
            tileRenderer.material.SetColor("_Color", Color.red);
            WorldController.worldController.lastTile = this;

            if(BuildingsController.buildingsController.buildingInProgress != null)
            {
                if (BuildingsController.buildingsController.buildingInProgress.IsBig)
                {
                    isPossibleToPlace = true;
                    var tile = GameObject.Find($"({coordinates.x - 1}, {coordinates.y})").GetComponent<Tile>();
                    neighbors.Add(tile);
                    if (!tile.HoveredByBuilding()) isPossibleToPlace = false;
                    tile = GameObject.Find($"({coordinates.x - 1}, {coordinates.y + 1})").GetComponent<Tile>();
                    neighbors.Add(tile);
                    if (!tile.HoveredByBuilding()) isPossibleToPlace = false;
                    tile = GameObject.Find($"({coordinates.x}, {coordinates.y + 1})").GetComponent<Tile>();
                    neighbors.Add(tile);
                    if (!tile.HoveredByBuilding()) isPossibleToPlace = false;

                    corner = new Vector3(gameObject.transform.position.x - 5, 0, gameObject.transform.position.z + 5);
                    BuildingsController.buildingsController.buildingInProgress.ShowOnTile(corner, isClickable);
                }
                else
                {
                    BuildingsController.buildingsController.buildingInProgress.ShowOnTile(gameObject.transform.position, isClickable);
                }
            }
        }
    }

    private void StopHover()
    {
        if (isSelected)
        {
            if (BuildingsController.buildingsController.buildingInProgress != null)
            {
                if (BuildingsController.buildingsController.buildingInProgress.IsBig)
                {
                    foreach(Tile neighbor in neighbors)
                    {
                        neighbor.UnHoveredByBuilding();
                    }
                }
            }

                    isSelected = false;
            hoverMesh.SetActive(false);
            tileRenderer.material.SetColor("_Color", Color.green);
        }
    }
}
