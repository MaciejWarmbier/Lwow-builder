using System;
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

    public Material GetMaterial(ColorType type)
    {
        return colorList.FirstOrDefault(r => r.type == type).material;
    }

    [Serializable]
    private class ColorConfiguration
    {
        public ColorType type;
        public Color color;
        public Material material;
    }

    public enum ColorType
    {
        Positive,
        Negative,
        Selected,
        Clicked,
        FontDefault,
        FontPositive,
        FontNegative,
    }
}
