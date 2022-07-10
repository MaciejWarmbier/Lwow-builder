using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [SerializeField] string buildingName;
    [SerializeField] bool isBig;
    [SerializeField] int foodCost;
    [SerializeField] int resourcesCost;
    [SerializeField] int buildingScore;
    [SerializeField] int moraleProduction;
    [SerializeField] int resourcesProduction;
    [SerializeField] int foodProduction;
    [SerializeField] Sprite buildingImage; 
    [SerializeField] string description; 

    public int FoodCost { get { return foodCost; } }
    public int ResourcesCost { get { return resourcesCost; } }
    public Sprite BuildingImage { get { return buildingImage; } }
    public string Description { get { return description; } }
    public int BuildingScore { get { return buildingScore; } }
    public int MoraleProduction { get { return moraleProduction; } }
    public int ResourcesProduction { get { return resourcesProduction; } }
    public int FoodProduction { get { return foodProduction; } }
    public bool IsBig { get { return isBig; } }

    public void Awake()
    {
        Assert.IsFalse(string.IsNullOrEmpty(buildingName));
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log(buildingName);
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Exit" + buildingName);
        }
    }

    public GameObject CreateBuilding(Building building, Vector3 position)
    {
        var instantiatedBuilding = Instantiate(building.gameObject, position, Quaternion.identity);
        return instantiatedBuilding;
    }

    public virtual bool CheckIfPossibleToBuild()
    {
        return true;
    }

    public void ShowOnTile(Vector3 position, bool canBePlaced)
    {
        gameObject.transform.position = position;
    }

    public void PlaceOnTile(Vector3 position)
    {
        //TODO check costs
        gameObject.transform.position = position;
        gameObject.transform.parent = BuildingsController.buildingsController.buildingsObject.transform;
        BuildingsController.buildingsController.buildingInProgress = null;

        VillageResources.villageResources.ChangeBuildingScore(BuildingScore);
        VillageResources.villageResources.ChangeFoodProduction(FoodProduction);
        VillageResources.villageResources.ChangeResourcesProduction(ResourcesProduction);
        VillageResources.villageResources.ChangeMoraleProduction(MoraleProduction);
        VillageResources.villageResources.ChangeFood(-FoodCost);
        VillageResources.villageResources.ChangeResources(-ResourcesCost);
        WorldController.worldController.CheckEvent();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

    }
}
