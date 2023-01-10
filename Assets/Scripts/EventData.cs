using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameEvent 
{
    public EventType type;
    public Building script;
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

[Serializable]
public enum EventType
{
    GrowlingInTheWoods,
    BreakOfWinter,
    StormOfPerun,
    AltarOfPerun,
    TheNightKiller,
    ForestNymph,
    KupalaNight,
    SlayerOfTheBeast,
    SlayerOfTheBeast2,
    TheGreatHunt,
    DevilishWell
}