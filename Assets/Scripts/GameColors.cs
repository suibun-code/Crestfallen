using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LineColorEnum
{
    RED,
    GREEN,
    BLUE,
    YELLOW,
    Count
}

public struct AccuracyColor
{
    public static Vector4 miss = new Vector4(0.66f, 0.18f, 0.85f, 1.0f);
    public static Vector4 bad = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    public static Vector4 great = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
    public static Vector4 perfect = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
}

public struct HitLineColor
{
    public static Vector4 red = new Vector4(1.0f, 0.44706f, 0.44706f, 1.0f);
    public static Vector4 green = new Vector4(0.44706f, 1f, 0.44706f, 1.0f);
    public static Vector4 blue = new Vector4(0.44706f, 0.44706f, 1f, 1.0f);
    public static Vector4 yellow = new Vector4(1f, 1f, 0.44706f, 1.0f);
};

public struct PlayerLineColor
{
    public static Vector4 red = new Vector4(32f, 0, 0, 1.0f);
    public static Vector4 green = new Vector4(0, 32f, 0, 1.0f);
    public static Vector4 blue = new Vector4(0, 8f, 32f, 1.0f);
    public static Vector4 yellow = new Vector4(32f, 32f, 0, 1.0f);
}
