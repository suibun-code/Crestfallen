using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class PluginTest : MonoBehaviour
{
    const string dll = "UnityFileDialog";

    [DllImport(dll)]
    private static extern void SeedRandomizer();

    [DllImport(dll)]
    private static extern int DieRoll(int sides);

    [DllImport(dll)]
    private static extern float Add(float a, float b);

    [DllImport(dll)]
    private static extern float Random();

    [DllImport(dll)]
    private static extern IntPtr HelloWorld();

    void Start() {
        SeedRandomizer();

        Debug.Log(Add(3.2f, 4.5f));
        Debug.Log(DieRoll(20));
        Debug.Log(Random());
        Debug.Log(Marshal.PtrToStringAnsi(HelloWorld()));
    }
}
