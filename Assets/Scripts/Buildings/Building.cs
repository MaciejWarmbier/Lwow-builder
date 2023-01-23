using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using static BuildingConfig;
using static Tile;

public class Building : MonoBehaviour
{
    public Action<bool> OnDestruction;
    [SerializeField] BuildingData buildingData;
    [SerializeField] BuildingCanvas buildingCanvas;
    [SerializeField] GameObject constructionObject;

    private bool _isInConstruction = false;
    public bool IsBeingDestroyed = false;

    public BuildingData Data { get { return buildingData; } }
    public BuildingType Type;

    private List<Tile> neighbors;

    public void Awake()
    {
        Assert.IsNotNull(buildingData);
        Assert.IsNotNull(buildingCanvas);
        Assert.IsNotNull(constructionObject);
        buildingCanvas.Setup(this);
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

    public List<Building> GetNeighborsOfType(BuildingType buildingType)
    {
        List<Building> buildings = new List<Building>();
        foreach (var neighbor in neighbors)
        {
            if (neighbor.PlacedBuilding?.Type == buildingType && !_isInConstruction && !neighbor.PlacedBuilding.IsBeingDestroyed)
            {
                buildings.Add(neighbor.PlacedBuilding);
            }
        }

        return buildings;
    }

    public int CheckForNeighbor(BuildingType buildingType)
    {
        int buildingTiles = 0;
        foreach(var neighbor in neighbors)
        {
            if(neighbor.PlacedBuilding?.Type == buildingType && !_isInConstruction && !neighbor.PlacedBuilding.IsBeingDestroyed)
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

    public virtual void ActivateEffect()
    {

    }

    public virtual string Description()
    {
        string description = "";
        if (buildingData.ResourcesProduction != 0) description = description + $"{{r}}{buildingData.ResourcesProduction} ";
        if (buildingData.FoodProduction != 0) description = description + $"{{f}}{buildingData.FoodProduction} ";
        if (buildingData.MoraleProduction != 0) description = description + $"{{m}}{buildingData.MoraleProduction} ";


        return description;
    }

    public GameObject CreateBuilding(Building building, Vector3 position)
    {
        var instantiatedBuilding = Instantiate(building.gameObject, new Vector3(position.x, 5, position.z) , Quaternion.identity);
        
        return instantiatedBuilding;
    }

    public virtual bool CheckIfUnlocked()
    {
        return true;
    }

    public bool CheckIfCanBeDestroyed()
    {
        return !_isInConstruction && !IsBeingDestroyed;
    }

    public void ShowOnTile(Vector3 position, bool canBePlaced)
    {
       gameObject.transform.position = new Vector3(position.x, 4, position.z);
    }

    public void ShowDescriptionCanvas(bool isShown)
    {
        if (!_isInConstruction)
        {
            if (isShown)
                buildingCanvas.ShowBuildingDescription();
            else
                buildingCanvas.HideBuildingDescription();
        }
    }

    public async void PlaceOnTile(Vector3 position, List<Tile> listOfTiles)
    {
        if (CheckCosts())
        {
            neighbors = listOfTiles;
            gameObject.transform.position = new Vector3(position.x, 4, position.z);
            
            gameObject.transform.parent = BuildingsController.buildingsController.gameObject.transform;
            BuildingsController.buildingsController.buildingInProgress = null;
            BuildingsController.buildingsController.AddBuildingToList(this);
            VillageResources.villageResources.ChangeFood(-buildingData.FoodCost);
            VillageResources.villageResources.ChangeResources(-buildingData.ResourcesCost);

            await StartConstructionTimer();

            this.PassiveEffect();

            VillageResources.villageResources.ChangeBuildingScore(buildingData.BuildingScore);
            VillageResources.villageResources.ChangeFoodProduction(buildingData.FoodProduction);
            VillageResources.villageResources.ChangeResourcesProduction(buildingData.ResourcesProduction);
            VillageResources.villageResources.ChangeMoraleProduction(buildingData.MoraleProduction);

            WorldController.worldController.CheckEvent();
        }
    }

    public async Task StartConstructionTimer()
    {
        _isInConstruction = true;
        constructionObject.SetActive(true);
        var part = (int)(((float)buildingData.TimeToBuild / 9) * 1000);
        for (int i=0; i<9; i++)
        {
            buildingCanvas.UpdateConstructionTimer(i);
            await Task.Delay(part);
        }
        buildingCanvas.HideConstructionTimer();
        constructionObject.SetActive(false);
        _isInConstruction= false;
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


    public virtual void DestroyBuilding(bool isDestroyedByStorm)
    {
        IsBeingDestroyed = true;
        VillageResources.villageResources.ChangeResourcesProduction(-Data.ResourcesProduction);
        VillageResources.villageResources.ChangeFoodProduction(-Data.FoodProduction);
        VillageResources.villageResources.ChangeMoraleProduction(-Data.MoraleProduction);

        OnDestruction?.Invoke(isDestroyedByStorm);
        BuildingsController.buildingsController.RemoveBuildingFromList(this);
        
        Destroy(this.gameObject);
        IsBeingDestroyed = false;
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
        public int TimeToBuild;
        public bool IsBig;
        public bool IsOneTimeBuilt;
    }
}


