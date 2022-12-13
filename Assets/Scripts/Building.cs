using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static BuildingConfig;
using static Tile;

public class Building : MonoBehaviour
{
    [SerializeField] string buildingName;
    [SerializeField] bool isBig;
    [SerializeField] int foodCost;
    [SerializeField] int resourcesCost;
    [SerializeField] int buildingScore;
    [SerializeField] int moraleProduction;
    [SerializeField] int resourcesProduction;
    [SerializeField] int foodProduction;
    [SerializeField] Sprite buildingImage; 
    [SerializeField] string description;

    public BuildingType Type;
    public int FoodCost { get { return foodCost; } }
    public int ResourcesCost { get { return resourcesCost; } }
    public Sprite BuildingImage { get { return buildingImage; } }
    public string Description { get { return description; } }
    public int BuildingScore { get { return buildingScore; } }
    public int MoraleProduction { get { return moraleProduction; } }
    public int ResourcesProduction { get { return resourcesProduction; } }
    public int FoodProduction { get { return foodProduction; } }
    public bool IsBig { get { return isBig; } }

    private List<Tile> neighbors;

    public void Awake()
    {
        Assert.IsFalse(string.IsNullOrEmpty(buildingName));
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log(buildingName);
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Exit" + buildingName);
        }
    }

    public int CheckForNeighbor(BuildingType buildingType)
    {
        int buildingTiles = 0;
        foreach(var neighbor in neighbors)
        {
            if(neighbor.PlacedBuilding.Type == buildingType)
            {
                buildingTiles++;
            }
        }

        return buildingTiles;
    }

    public int CheckForNeighbor(TileType tileType)
    {
        int neighborTiles = 0;
        foreach (var neighbor in neighbors)
        {
            if (neighbor.Type == tileType)
            {
                neighborTiles++;
            }
        }

        return neighborTiles;
    }

    public virtual void PassiveEffect()
    {

    }

    public GameObject CreateBuilding(Building building, Vector3 position)
    {
        var instantiatedBuilding = Instantiate(building.gameObject, position, Quaternion.identity);
        return instantiatedBuilding;
    }

    public virtual bool CheckIfPossibleToBuild()
    {
        return true;
    }

    public void ShowOnTile(Vector3 position, bool canBePlaced)
    {
        gameObject.transform.position = position;
    }

    public void PlaceOnTile(Vector3 position, List<Tile> listOfTiles)
    {
        //TODO check costs
        neighbors = listOfTiles;
        gameObject.transform.position = position;
        gameObject.transform.parent = BuildingsController.buildingsController.buildingsObject.transform;
        BuildingsController.buildingsController.buildingInProgress = null;
        this.PassiveEffect();

        VillageResources.villageResources.ChangeBuildingScore(BuildingScore);
        VillageResources.villageResources.ChangeFoodProduction(FoodProduction);
        VillageResources.villageResources.ChangeResourcesProduction(ResourcesProduction);
        VillageResources.villageResources.ChangeMoraleProduction(MoraleProduction);
        VillageResources.villageResources.ChangeFood(-FoodCost);
        VillageResources.villageResources.ChangeResources(-ResourcesCost);
        WorldController.worldController.CheckEvent();
    }
}
