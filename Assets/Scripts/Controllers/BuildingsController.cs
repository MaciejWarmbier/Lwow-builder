using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using static BuildingConfig;
using static Tile;

public class BuildingsController : MonoBehaviour, IController
{
    public Building buildingInProgress = null;
    public List<Building> listOfBuiltBuildings;
    public List<BuildingType> typesOfBuiltBuildings;

    private BuildingConfig buildingConfig;
    private GridManager _gridController;

    public void OnDestroyPlacedBuilding()
    {
        Destroy(buildingInProgress.gameObject);
        buildingInProgress = null;
    }

    public void OnDestroyBuildingCanvasClosed(bool isDestroyed)
    {
        if (isDestroyed)
        {
            _gridController.lastChosenTile.PlacedBuilding.DestroyBuilding(false);
        }
    }

    public void OnBuildingSelection(bool isBought, Building boughtBuilding)
    {
        if (isBought)
        {
            Vector3 buildingPosition = _gridController.lastChosenTile.transform.position;
            _gridController.lastChosenTile.StopHover();
            GameObject createdBuilding = boughtBuilding.CreateBuilding(boughtBuilding, buildingPosition);
            if (createdBuilding != null)
            {
                buildingInProgress = createdBuilding.GetComponent<Building>();
            }
        }
    }

    public void AddBuildingToList(Building building)
    {
        listOfBuiltBuildings.Add(building);
        if (!typesOfBuiltBuildings.Contains(building.Type))
            typesOfBuiltBuildings.Add(building.Type);
    }

    public void RemoveBuildingFromList(Building building)
    {
        listOfBuiltBuildings.Remove(building);
        var type = listOfBuiltBuildings.FirstOrDefault(x => x.Type == building.Type);
        if (type != null)
            typesOfBuiltBuildings.Remove(building.Type);
    }

    public List<Building> GetAvailableBuildings()
    {
        List<Building> availableBuildings = buildingConfig.GetBuildingPrefabList();

        for(int i = 0; i< availableBuildings.Count; i++)
        {
            if (!availableBuildings[i].CheckIfUnlocked() || (availableBuildings[i].Data.IsOneTimeBuilt && typesOfBuiltBuildings.Contains(availableBuildings[i].Type)))
            {
                availableBuildings.RemoveAt(i);
                i--;
            }
        }

        return availableBuildings;
    }

    public async Task<bool> Initialize()
    {
        await Task.Yield();
        _gridController = GameController.Game.GetController<GridManager>();
        buildingConfig = ConfigController.GetConfig<BuildingConfig>();

        List<Building> Buildings = buildingConfig.GetBuildingPrefabList();
        foreach(Building building in Buildings)
        {
            building.Initialize();
        }

        return true;
    }
}
