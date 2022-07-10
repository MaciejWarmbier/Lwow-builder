using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BuildingsController : MonoBehaviour
{
    [SerializeField] private List<Building> buildingPrefabs;
    [SerializeField] BuildingSelectionCanvas buildingSelectionCanvas;

    private void Awake()
    {
        Assert.IsNotNull(buildingPrefabs);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            var canvas = Instantiate(buildingSelectionCanvas);
            canvas.OnCanvasClosed += OnBuildingSelection;
        }
    }

    private void OnBuildingSelection(bool isBought, Building boughtBuilding)
    {
        if (isBought)
        {
            Vector3 buildingPosition = transform.position;
            buildingPosition.y += 2;
            GameObject createdBuilding = boughtBuilding.CreateBuilding(boughtBuilding, buildingPosition);
            //if (createdBuilding != null)
            //{
            //    building = createdBuilding.GetComponent<Building>();
            //    building.HoveredOver += Hover;
            //    building.StoppedHover += StopHover;
            //    gridManager.BlockNode(coordinates);
            //    StopHover();
            //    VillageResources.villageResources.ChangeBuildingScore(boughtBuilding.BuildingScore);
            //    VillageResources.villageResources.ChangeFoodProduction(boughtBuilding.FoodProduction);
            //    VillageResources.villageResources.ChangeResourcesProduction(boughtBuilding.ResourcesProduction);
            //    VillageResources.villageResources.ChangeMoraleProduction(boughtBuilding.MoraleProduction);
            //    WorldController.worldController.CheckEvent();
            //}
        }
        else
        {
            //StopHover();
        }
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
