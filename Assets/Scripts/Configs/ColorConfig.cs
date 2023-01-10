using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorConfig", menuName = "Configs/ColorConfig", order = 1)]
public class ColorConfig : ScriptableObject
{
    [SerializeField] List<ColorConfiguration> colorList;

    public Color GetColor(ColorType type)
    {
        return colorList.FirstOrDefault(r => r.type == type).color;
    }

    [Serializable]
    private class ColorConfiguration
    {
        [SerializeField] public ColorType type;
        [SerializeField] public Color color;
    }

    public enum ColorType
    {
        Positive,
        Negative,
        Selected,
        Clicked,
        FontDefault
    }
}
