using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using static BuildingConfig;
using static ColorConfig;
using static Tile;

public class Building : MonoBehaviour
{
    [SerializeField] GameObject OverlayObject;
    [SerializeField] List<MeshRenderer> OverlayMeshList;
    [SerializeField] BuildingData buildingData;

    public BuildingData Data { get { return buildingData; } }
    public BuildingType Type;

    private List<Tile> neighbors;
    private ColorConfig _colorConfig;

    public void Awake()
    {
        Assert.IsNotNull(buildingData);
        // TODO uncomment
       // Assert.IsNotNull(overlay);
       // Assert.IsNotNull(overlayMaterial);

        _colorConfig = ConfigController.GetConfig<ColorConfig>();
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log(buildingData.Name);
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Exit" + buildingData.Name);
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

    public virtual bool CheckIfUnlocked()
    {
        return true;
    }

    public void ShowOnTile(Vector3 position, bool canBePlaced)
    {
        // TODO uncomment
        if (canBePlaced)
        {
            gameObject.transform.position = position;
            //SetOverlay(true, ColorType.Positive);
        }
        else
        {
            //SetOverlay(true, ColorType.Negative);
        }
    }

    public void SetOverlay(bool isActive, ColorType colorType)
    {
        OverlayObject.SetActive(isActive);
        foreach(var mesh in OverlayMeshList)
        {
            //TODO Material Config
           // mesh.material = 
        }
    }

    public void PlaceOnTile(Vector3 position, List<Tile> listOfTiles)
    {
        if (CheckCosts())
        {
            neighbors = listOfTiles;
            gameObject.transform.position = position;
            //TODO think if not under tiles

            gameObject.transform.parent = BuildingsController.buildingsController.gameObject.transform;
            BuildingsController.buildingsController.buildingInProgress = null;
            this.PassiveEffect();

            VillageResources.villageResources.ChangeBuildingScore(buildingData.BuildingScore);
            VillageResources.villageResources.ChangeFoodProduction(buildingData.FoodProduction);
            VillageResources.villageResources.ChangeResourcesProduction(buildingData.ResourcesProduction);
            VillageResources.villageResources.ChangeMoraleProduction(buildingData.MoraleProduction);
            VillageResources.villageResources.ChangeFood(-buildingData.FoodCost);
            VillageResources.villageResources.ChangeResources(-buildingData.ResourcesCost);

            WorldController.worldController.CheckEvent();
        }
    }

    private bool CheckCosts()
    {
        if (VillageResources.villageResources.Food < buildingData.FoodCost)
        {
            Debug.LogError("Not enough food");
            return false;
        }
        if(VillageResources.villageResources.Resources < buildingData.ResourcesCost)
        {
            Debug.LogError("Not enough resources");
            return false;
        }

        return true;
    }

    public void DestroyBuilding(bool isDestroyedByStorm)
    {
        //TODO
        if (isDestroyedByStorm)
        {
            //TODO add lighting effect
        }
        BuildingsController.buildingsController.listOfBuiltBuildings.Remove(this);
        Destroy(this);
    }

    [Serializable]
    public class BuildingData
    {
        public string Name;
        public int FoodCost;
        public int ResourcesCost;
        public Sprite BuildingImage;
        public string Description;
        public int BuildingScore;
        public int MoraleProduction;
        public int ResourcesProduction;
        public int FoodProduction;
        public bool IsBig;
    }
}


