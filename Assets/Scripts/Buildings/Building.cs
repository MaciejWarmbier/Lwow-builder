using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using static BuildingConfig;
using static Tile;

public class Building : MonoBehaviour
{
    public Action<bool> OnDestruction;
    [SerializeField] BuildingData buildingData;
    [SerializeField] BuildingDescriptionCanvas buildingDesciptionCanvas;
    [SerializeField] GameObject constructionObject;

    private bool _isInConstruction = false;
    public bool IsBeingDestroyed = false;
    private ResourcesController _resourcesController;
    protected PlotController _plotController;
    protected GameEventsController _gameEventsController;
    protected BuildingsController _buildingsController;

    public BuildingData Data { get { return buildingData; } }
    public BuildingType Type;

    private List<Tile> neighbors;

    public void Start()
    {
        Assert.IsNotNull(buildingData);
        Assert.IsNotNull(buildingDesciptionCanvas);
        Assert.IsNotNull(constructionObject);

        buildingDesciptionCanvas.Setup(this);

        Initialize();
    }

    public void Initialize()
    {
        _resourcesController = GameController.Game.GetController<ResourcesController>();
        _plotController = GameController.Game.GetController<PlotController>();
        _gameEventsController = GameController.Game.GetController<GameEventsController>();
        _buildingsController = GameController.Game.GetController<BuildingsController>();
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
                buildingDesciptionCanvas.ShowBuildingDescription();
            else
                buildingDesciptionCanvas.HideBuildingDescription();
        }
    }

    public void PlaceOnTile(Vector3 position, List<Tile> listOfTiles)
    {
        if (CheckCosts())
        {
            neighbors = listOfTiles;
            gameObject.transform.position = new Vector3(position.x, 4, position.z);
            
            gameObject.transform.parent = _buildingsController.gameObject.transform;
            _buildingsController.buildingInProgress = null;
            _buildingsController.AddBuildingToList(this);
            _resourcesController.ChangeFood(-buildingData.FoodCost);
            _resourcesController.ChangeResources(-buildingData.ResourcesCost);

            StartCoroutine(StartConstructionTimer());
        }
    }

    private void FinishConstruction()
    {
        this.PassiveEffect();

        _resourcesController.ChangeBuildingScore(buildingData.BuildingScore);
        _resourcesController.ChangeFoodProduction(buildingData.FoodProduction);
        _resourcesController.ChangeResourcesProduction(buildingData.ResourcesProduction);
        _resourcesController.ChangeMoraleProduction(buildingData.MoraleProduction);

        _gameEventsController.CheckEvent();
    }

    public IEnumerator StartConstructionTimer()
    {
        _isInConstruction = true;
        constructionObject.SetActive(true);
        var part = (((float)buildingData.TimeToBuild / 9));
        for (int i=0; i<9; i++)
        {
            buildingDesciptionCanvas.UpdateConstructionTimer(i);
            yield return new WaitForSeconds(part);
        }
        buildingDesciptionCanvas.HideConstructionTimer();
        constructionObject.SetActive(false);
        _isInConstruction= false;

        FinishConstruction();
    }

    

    private bool CheckCosts()
    {
        if (_resourcesController.Food < buildingData.FoodCost)
        {
            Debug.LogError("Not enough food");
            return false;
        }
        if(_resourcesController.Resources < buildingData.ResourcesCost)
        {
            Debug.LogError("Not enough resources");
            return false;
        }

        return true;
    }


    public virtual void DestroyBuilding(bool isDestroyedByStorm)
    {
        IsBeingDestroyed = true;
        _resourcesController.ChangeResourcesProduction(-Data.ResourcesProduction);
        _resourcesController.ChangeFoodProduction(-Data.FoodProduction);
        _resourcesController.ChangeMoraleProduction(-Data.MoraleProduction);

        OnDestruction?.Invoke(isDestroyedByStorm);
        _buildingsController.RemoveBuildingFromList(this);
        
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


