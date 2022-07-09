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

    public int Resources { get { return resources; } }
    public int Food { get { return food; } }
    public int Morale { get { return morale; } }
    public int Level { get { return level; } }

    private void Awake()
    {
        Assert.IsNotNull(resourcesLabel);
        Assert.IsNotNull(foodLabel);
        Assert.IsNotNull(moraleLabel);
    }

    private void Start()
    {
        UpdateLabels();
    }

    public void AddResources(int addValue)
    {
        resources += addValue;
        UpdateLabels();
    }

    public void AddFood(int addValue)
    {
        food += addValue;
        UpdateLabels();
    }

    public void AddMorale(int addValue)
    {
        morale += addValue;
        UpdateLabels();
    }

    public bool WithdrawResources(int minusValue)
    {
        if(resources - minusValue < 0)
        {
            return false;
        }
        resources -= minusValue;
        UpdateLabels();
        return true;
    }

    public bool WithdrawFood(int minusValue)
    {
        if (food - minusValue < 0)
        {
            return false;
        }
        food -= minusValue;
        UpdateLabels();
        return true;
    }

    public bool WithdrawMorale(int minusValue)
    {
        if (morale - minusValue < 0)
        {
            return false;
        }
        morale -= minusValue;
        UpdateLabels();
        return true;
    }
    
    public void UpdateLabels()
    {
        foodLabel.text = food.ToString();
        moraleLabel.text = morale.ToString();
        resourcesLabel.text = resources.ToString();
    }
}
