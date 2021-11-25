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

public struct HitLineColor
{
    public static Vector4 red = new Vector4(1.0f, 0.44706f, 0.44706f, 1.0f);
    public static Vector4 green = new Vector4(0.44706f, 1f, 0.44706f, 1.0f);
    public static Vector4 blue = new Vector4(0.44706f, 0.44706f, 1f, 1.0f);
    public static Vector4 yellow = new Vector4(1f, 1f, 0.44706f, 1.0f);
};

public struct PlayerLineColor
{
    public static Vector4 red = new Vector4(11.31371f, 0, 0, 1.0f);
    public static Vector4 green = new Vector4(0, 11.31371f, 0, 1.0f);
    public static Vector4 blue = new Vector4(0, 0, 11.31371f, 1.0f);
    public static Vector4 yellow = new Vector4(11.31371f, 11.31371f, 0, 1.0f);
}
