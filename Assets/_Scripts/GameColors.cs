using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameColors : Singleton<GameColors>
{
    public Color hitlineColor1;
    public Color hitlineColor2;
    public Color hitlineColor3;
    public Color hitlineColor4;

    [ColorUsage(true, true)] public Color laneColor1;
    [ColorUsage(true, true)] public Color laneColor2;
    [ColorUsage(true, true)] public Color laneColor3;
    [ColorUsage(true, true)] public Color laneColor4;

    //Difficulty text colors in SongSelect
    public Color Tier0;
    public Color Tier1;
    public Color Tier2;
    public Color Tier3;
    public Color Tier4;

    //public Vector4 hitlineColorOne { get; private set; } = new Vector4(1.0f, 0.44706f, 0.44706f, 1.0f);
    //public Vector4 hitlineColorTwo { get; private set; } = new Vector4(0.44706f, 1f, 0.44706f, 1.0f);
    //public Vector4 hitlineColorThree { get; private set; } = new Vector4(0.44706f, 0.44706f, 1f, 1.0f);
    //public Vector4 hitlineColorFour { get; private set; } = new Vector4(1f, 1f, 0.44706f, 1.0f);

    //public Vector4 laneColorOne { get; private set; } = new Vector4(32f, 0, 0, 1.0f);
    //public Vector4 laneColorTwo { get; private set; } = new Vector4(0, 32f, 0, 1.0f);
    //public Vector4 laneColorThree { get; private set; } = new Vector4(0, 8f, 32f, 1.0f);
    //public Vector4 laneColorFour { get; private set; } = new Vector4(32f, 32f, 0, 1.0f);

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //hitlineColorOne = new Vector4(hitlineColor1.r, hitlineColor1.g, hitlineColor1.b, hitlineColor1.a);
        //hitlineColorTwo = new Vector4(hitlineColor2.r, hitlineColor2.g, hitlineColor2.b, hitlineColor2.a);
        //hitlineColorThree = new Vector4(hitlineColor3.r, hitlineColor3.g, hitlineColor3.b, hitlineColor3.a);
        //hitlineColorFour = new Vector4(hitlineColor4.r, hitlineColor4.g, hitlineColor4.b, hitlineColor4.a);

        //laneColorOne = new Vector4(laneColor1.r, laneColor1.g, laneColor1.b, laneColor1.a);
        //laneColorTwo = new Vector4(laneColor2.r, laneColor2.g, laneColor2.b, laneColor2.a);
        //laneColorThree = new Vector4(laneColor3.r, laneColor3.g, laneColor3.b, laneColor3.a);
        //laneColorFour = new Vector4(laneColor4.r, laneColor4.g, laneColor4.b, laneColor4.a);
    }
};
