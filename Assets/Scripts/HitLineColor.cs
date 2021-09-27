using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitLineEnum
{
    RED,
    GREEN,
    BLUE,
    YELLOW,
    COUNT
}

public struct HitLineColor
{
    public static Color32 red = new Color32(255, 114, 114, 255);
    public static Color32 green = new Color32(114, 255, 114, 255);
    public static Color32 blue = new Color32(114, 114, 255, 255);
    public static Color32 yellow = new Color32(255, 255, 114, 255);
};
