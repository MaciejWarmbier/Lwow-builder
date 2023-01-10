using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class BuildingsController : MonoBehaviour
{
    [SerializeField] BuildingSelectionCanvas buildingSelectionCanvas;

    public static BuildingsController buildingsController;

    public Building buildingInProgress = null;
    public List<Building> listOfBuiltBuildings;

    private bool isCanvasActive = false;
    private BuildingConfig buildingConfig;

    private void Awake()
    {
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
            WorldController.worldController.lastTile.StopHover();
            GameObject createdBuilding = boughtBuilding.CreateBuilding(boughtBuilding, buildingPosition);
            if (createdBuilding != null)
            {
                buildingInProgress = createdBuilding.GetComponent<Building>();
            }
        }

        isCanvasActive = false;
    }

    public void AddBuildingToList()
    {
        listOfBuiltBuildings.Add(buildingInProgress);
    }

    public List<Building> GetAvailableBuildings()
    {
        List<Building> availableBuildings = buildingConfig.GetBuildingPrefabList();

        for(int i = 0; i< availableBuildings.Count; i++)
        {
            if (!availableBuildings[i].CheckIfUnlocked())
            {
                availableBuildings.RemoveAt(i);
                i--;
            }
        }

        return availableBuildings;
    }
}
