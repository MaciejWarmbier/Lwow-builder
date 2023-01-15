using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static string AddSpriteTextToStrings(this string text)
    {
        string newText = text.Replace("{f}", "<sprite=1>");
        newText = newText.Replace("{m}", "<sprite=2>");
        newText = newText.Replace("{r}", "<sprite=0>");

        return newText;
    }
}
