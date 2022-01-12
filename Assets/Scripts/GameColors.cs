using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameColors : Singleton<GameColors>
{
    public Vector4 hitlineColorOne { get; private set; } = new Vector4(1.0f, 0.44706f, 0.44706f, 1.0f);
    public Vector4 hitlineColorTwo { get; private set; } = new Vector4(0.44706f, 1f, 0.44706f, 1.0f);
    public Vector4 hitlineColorThree { get; private set; } = new Vector4(0.44706f, 0.44706f, 1f, 1.0f);
    public Vector4 hitlineColorFour { get; private set; } = new Vector4(1f, 1f, 0.44706f, 1.0f);

    public Vector4 laneColor_Left1 { get; private set; } = new Vector4(32f, 0, 0, 1.0f);
    public Vector4 laneColor_Left2 { get; private set; } = new Vector4(0, 32f, 0, 1.0f);
    public Vector4 laneColor_Right1 { get; private set; } = new Vector4(0, 8f, 32f, 1.0f);
    public Vector4 laneColor_Right2 { get; private set; } = new Vector4(32f, 32f, 0, 1.0f);
};
