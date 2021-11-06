using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class FileDialogPlugin : MonoBehaviour
{
    const string dll = "UnityFileDialog";

    [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr open_file_dialog();
    [DllImport(dll)]
    private static extern IntPtr get_file_name();

    public static string OpenFileDialog()
    {
        return Marshal.PtrToStringAnsi(open_file_dialog());
    }
    public static string GetFileName()
    {
        return Marshal.PtrToStringAnsi(get_file_name());
    }
}
