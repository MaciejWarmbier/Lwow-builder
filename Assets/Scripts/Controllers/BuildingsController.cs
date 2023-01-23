using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using static BuildingConfig;
using static Tile;

public class BuildingsController : MonoBehaviour
{
    [SerializeField] BuildingSelectionCanvas buildingSelectionCanvas;
    [SerializeField] DestroyBuildingCanvas destroyBuildingCanvas;

    public static BuildingsController buildingsController;

    public Building buildingInProgress = null;
    public List<Building> listOfBuiltBuildings;
    public List<BuildingType> typesOfBuiltBuildings;

    public bool isCanvasActive = false;
    private BuildingConfig buildingConfig;

    private void Awake()
    {
        Assert.IsNotNull(buildingSelectionCanvas);
        Assert.IsNotNull(destroyBuildingCanvas);

        buildingConfig = ConfigController.GetConfig<BuildingConfig>();
        buildingsController = GetComponent<BuildingsController>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B) && !isCanvasActive && buildingInProgress == null)
        {
            if (!FindObjectOfType<BuildingSelectionCanvas>())
            {
                isCanvasActive = true;
                WorldController.worldController.lastTile.StopHover();
                var canvas = Instantiate(buildingSelectionCanvas);
                canvas.OnCanvasClosed += OnBuildingSelection;
            }
        }

        if (Input.GetKeyUp(KeyCode.D) && !isCanvasActive && buildingInProgress == null)
        {
            if (WorldController.worldController.lastTile.Type == TileType.Built && WorldController.worldController.lastTile.PlacedBuilding.CheckIfCanBeDestroyed())
            {
                if (!FindObjectOfType<DestroyBuildingCanvas>())
                {
                    isCanvasActive = true;
                    var canvas = Instantiate(destroyBuildingCanvas);
                    canvas.OnCanvasClosed += OnDestroyBuildingCanvasClosed;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && !isCanvasActive && buildingInProgress != null)
        {
            WorldController.worldController.lastTile.HideOverlayWithCornerTiles();
            Destroy(buildingInProgress.gameObject);
            buildingInProgress = null;
        }
    }

    private void OnDestroyBuildingCanvasClosed(bool isDestroyed)
    {
        if (isDestroyed)
        {
            WorldController.worldController.lastTile.PlacedBuilding.DestroyBuilding(false);
        }

        isCanvasActive = false;
    }

    private void OnBuildingSelection(bool isBought, Building boughtBuilding)
    {
        if (isBought)
        {
            Vector3 buildingPosition = WorldController.worldController.lastTile.transform.position;
            WorldController.worldController.lastTile.StopHover();
            GameObject createdBuilding = boughtBuilding.CreateBuilding(boughtBuilding, buildingPosition);
            if (createdBuilding != null)
            {
                buildingInProgress = createdBuilding.GetComponent<Building>();
            }
        }

        isCanvasActive = false;
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
}
