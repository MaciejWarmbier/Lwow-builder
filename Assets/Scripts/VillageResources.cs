using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class VillageResources : MonoBehaviour
{
    [SerializeField] int resources;
    [SerializeField] int food;
    [SerializeField] int morale;
    [SerializeField] int level;
    [SerializeField] TextMeshProUGUI resourcesLabel;
    [SerializeField] TextMeshProUGUI foodLabel;
    [SerializeField] TextMeshProUGUI moraleLabel;

    public static VillageResources villageResources;
    public int Resources { get { return resources; } }
    public int Food { get { return food; } }
    public int Morale { get { return morale; } }
    public int Level { get { return level; } }

    private void Awake()
    {
        Assert.IsNotNull(resourcesLabel);
        Assert.IsNotNull(foodLabel);
        Assert.IsNotNull(moraleLabel);

        villageResources = GetComponent<VillageResources>();
    }

    private void Start()
    {
        UpdateLabels();
    }

    public bool ChangeResources(int value)
    {
        if (value < 0)
        {
            if (resources + value < 0)
            {
                return false;
            }
            resources += value;
            UpdateLabels();
            return true;
        }
        else
        {
            resources += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeFood(int value)
    {
        if(value < 0)
        {
            if (food + value < 0)
            {
                return false;
            }
            food += value;
            UpdateLabels();
            return true;
        }
        else
        {
            food += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeMorale(int value)
    {
        if (value < 0)
        {
            if (morale + value < 0)
            {
                return false;
            }
            morale += value;
            UpdateLabels();
            return true;
        }
        else
        {
            morale += value;
            UpdateLabels();
            return true;
        }
    }
    
    public void UpdateLabels()
    {
        foodLabel.text = food.ToString();
        moraleLabel.text = morale.ToString();
        resourcesLabel.text = resources.ToString();
    }
}
