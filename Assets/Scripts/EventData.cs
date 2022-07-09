using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameEvent 
{
    public int eventLevel;
    public bool wasUsed = false;
    public string description;
    public Choice rightChoice;
    public Choice leftChoice;
    public Sprite image;
}

[Serializable]
public class Choice
{
    public string choiceText;
    public int resourcesChange;
    public int foodChange;
    public int moraleChange;
    public string choiceResult;
}
