using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Config", menuName = "Configs/Building Config", order = 1)]
public class BuildingConfig : ScriptableObject
{
    [SerializeField] List<BuildingConfiguration> buildingList;

    public Building GetBuildingPrefab(BuildingType type)
    {
        return buildingList.FirstOrDefault(r => r.type == type).buildingPrefab;
    }

    public List<Building> GetBuildingPrefabList()
    {
        var buildings = new List<Building>();
        foreach(var building in buildingList)
        {
            buildings.Add(building.buildingPrefab);
        }

        return buildings;
    }

    [Serializable]
    private class BuildingConfiguration
    {
        [SerializeField] public BuildingType type;
        [SerializeField] public Building buildingPrefab;
    }

    public enum BuildingType
    {
        Mill,
        Wheat,
        Well,
        Animal_Pen,
        Armory,
        City_Hall,
        House,
        Peruns_Altar,
        Sawmill,
        Tavern,
        Temple
    }
}
