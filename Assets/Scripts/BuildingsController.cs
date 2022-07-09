using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BuildingsController : MonoBehaviour
{
    [SerializeField] private List<Building> buildingPrefabs;
    private VillageResources village;

    private void Awake()
    {
        Assert.IsNotNull(buildingPrefabs);

        village = GetComponent<VillageResources>();
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

        return availableBuildinga;
    }

}
