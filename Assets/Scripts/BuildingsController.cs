using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BuildingsController : MonoBehaviour
{
    [SerializeField] private List<Building> buildingPrefabs;
    [SerializeField] BuildingSelectionCanvas buildingSelectionCanvas;
    [SerializeField] public GameObject buildingsObject;

    public static BuildingsController buildingsController;

    public Building buildingInProgress = null;
    private bool isCanvasActive = false;

    private void Awake()
    {
        Assert.IsNotNull(buildingPrefabs);
        buildingsController = GetComponent<BuildingsController>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B) && !isCanvasActive)
        {
            if (!FindObjectOfType<BuildingSelectionCanvas>())
            {
                isCanvasActive = true;
                var canvas = Instantiate(buildingSelectionCanvas);
                canvas.OnCanvasClosed += OnBuildingSelection;
            }
        }
    }

    private void OnBuildingSelection(bool isBought, Building boughtBuilding)
    {
        if (isBought)
        {
            Vector3 buildingPosition = WorldController.worldController.lastTile.transform.position;
            WorldController.worldController.lastTile.UnHoveredByBuilding();
            buildingPosition.y += 2;
            GameObject createdBuilding = boughtBuilding.CreateBuilding(boughtBuilding, buildingPosition);
            if (createdBuilding != null)
            {
                var building = createdBuilding.GetComponent<Building>();
                buildingInProgress = building;
            }
        }

        isCanvasActive = false;
    }

    public List<Building> GetAvailableBuildings()
    {
        List<Building> availableBuildings = new List<Building>();
        foreach(var building in buildingPrefabs)
        {
            if(building.CheckIfPossibleToBuild())
            {
                availableBuildings.Add(building);
            }
        }

        return availableBuildings;
    }



}
