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
    [SerializeField] int levelRequirement;
    [SerializeField] int foodCost;
    [SerializeField] int resourcesCost;
    [SerializeField] Sprite buildingImage; 
    [SerializeField] string description; 
    public int LevelRequirement { get { return levelRequirement; } }
    public int FoodCost { get { return foodCost; } }
    public int ResourcesCost { get { return resourcesCost; } }
    public Sprite BuildingImage { get { return buildingImage; } }
    public string Description { get { return description; } }


    public Action HoveredOver;
    public Action StoppedHover;

    public void Awake()
    {
        Assert.IsFalse(string.IsNullOrEmpty(buildingName));
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log(buildingName);
            HoveredOver?.Invoke();
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Exit" + buildingName);
            StoppedHover?.Invoke();
        }
    }

    public GameObject CreateBuilding(Building building, Vector3 position)
    {
        //Bank bank = FindObjectOfType<Bank>();
        //if (bank == null)
        //{
        //    return false;
        //}

        //if (bank.CurrentBalance >= cost)
        //{
            var instantiatedBuilding = Instantiate(building.gameObject, position, Quaternion.identity);
            //bank.Withdrawal(cost);
            return instantiatedBuilding;
        //}

        //return false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
