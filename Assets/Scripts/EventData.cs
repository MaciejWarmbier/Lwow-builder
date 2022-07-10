using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameEvent 
{
    public int eventLevel;
    public bool wasUsed = false;
    public string title;
    public string description;
    public string description2;
    public string description3;
    public string description4;
    public Choice rightChoice;
    public Choice leftChoice;
    public bool skipChoice;

    public bool isPerunEvent; //rightChoiceRozwala Mila i odblokowuje altar
    //left choice rozwala mila i 2 random budynki i odblokowuje altar
    public bool isNymphEvent;
    public bool isNocKupaly;
    public bool isSlayerOfTheBeast; 
    public bool isGreatHunt; 
    public bool isDevilishWell;
    public bool isPerunSword;
    public bool isSlayerOfTheBeast2;
}

[Serializable]
public class Choice
{
    public string choiceText;
    public int resourcesChange;
    public int foodChange;
    public int moraleChange;
    public string choiceResultText;
    public string choiceResultText2;
    public string choiceResultText3;
    public string choiceResultText4;
}
