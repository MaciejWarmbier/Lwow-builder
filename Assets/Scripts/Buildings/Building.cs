using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] BuildingCanvas buildingCanvas;

    [SerializeField] GameObject constructionObject;

    public BuildingData Data { get { return buildingData; } }
    public BuildingType Type;

    private List<Tile> neighbors;
    private ColorConfig _colorConfig;

    public void Awake()
    {
        Assert.IsNotNull(buildingData);
        Assert.IsNotNull(OverlayObject);
        Assert.IsNotNull(buildingCanvas);
        Assert.IsTrue(OverlayMeshList.Count>0);

        _colorConfig = ConfigController.GetConfig<ColorConfig>();
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

    public int CheckForNeighbor(BuildingType buildingType)
    {
        int buildingTiles = 0;
        foreach(var neighbor in neighbors)
        {
            if(neighbor.PlacedBuilding?.Type == buildingType)
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

    public virtual string Description()
    {
        string description = "";
        if (buildingData.ResourcesProduction != 0) description = description + $"<r>{buildingData.ResourcesProduction} ";
        if (buildingData.FoodProduction != 0) description = description + $"<f>{buildingData.FoodProduction} ";
        if (buildingData.MoraleProduction != 0) description = description + $"<m>{buildingData.MoraleProduction} ";


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

    public void ShowOnTile(Vector3 position, bool canBePlaced)
    {
        if (canBePlaced)
        {
            gameObject.transform.position = new Vector3(position.x, 5, position.z);
            SetOverlay(true, ColorType.Positive);
        }
        else
        {
            gameObject.transform.position = new Vector3(position.x, 5, position.z);
            SetOverlay(true, ColorType.Negative);
        }
    }

    public void SetOverlay(bool isActive, ColorType colorType)
    {
        if (isActive)
        {
            var material = _colorConfig.GetMaterial(colorType);
            foreach (var mesh in OverlayMeshList)
            {
                mesh.material = material;
            }
        }
        OverlayObject.SetActive(isActive);
    }

    public async void PlaceOnTile(Vector3 position, List<Tile> listOfTiles)
    {
        if (CheckCosts())
        {
            neighbors = listOfTiles;
            gameObject.transform.position = new Vector3(position.x, 5, position.z);
            //TODO think if not under tiles
            
            gameObject.transform.parent = BuildingsController.buildingsController.gameObject.transform;
            SetOverlay(false, ColorType.Positive);
            BuildingsController.buildingsController.buildingInProgress = null;
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
        var part = (int)(((float)buildingData.TimeToBuild / 9) * 1000);
        for (int i=0; i<9; i++)
        {
            buildingCanvas.UpdateConstructionTimer(i);
            await Task.Delay(part);
        }
        buildingCanvas.HideConstructionTimer();
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
        public int TimeToBuild;
        public bool IsBig;
    }
}


