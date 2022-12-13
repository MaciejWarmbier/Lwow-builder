using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Config", menuName = "Configs/Building Config", order = 1)]
public class BuildingConfig : ScriptableObject
{
    [SerializeField] List<BuildingConfiguration> buildingList;

    public Building GetBuilding(BuildingType type)
    {
        return buildingList.FirstOrDefault(r => r.type == type).buildingPrefab;
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
        Well
    }
}
