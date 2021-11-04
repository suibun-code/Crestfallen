using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class FileDialogPlugin : MonoBehaviour
{
    const string dll = "UnityFileDialog";

    [DllImport(dll)]
    private static extern IntPtr GetFilePath();

    public static string OpenFileDialog()
    {
        return Marshal.PtrToStringAnsi(GetFilePath());
    }
}
