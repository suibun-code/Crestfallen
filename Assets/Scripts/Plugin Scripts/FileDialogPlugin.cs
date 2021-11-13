using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class FileDialogPlugin : MonoBehaviour
{
    const string dll = "UnityFileDialog";

    [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr open_file_dialog(string title, string msg, string filter);
    [DllImport(dll)]
    private static extern IntPtr get_file_name();
    [DllImport(dll)]
    private static extern void open_explorer_with_path(string path);

    public static string OpenFileDialog(string title, string msg, string filter)
    {
        return Marshal.PtrToStringAnsi(open_file_dialog(title, msg, filter));
    }
    public static string GetFileName()
    {
        return Marshal.PtrToStringAnsi(get_file_name());
    }
    public static void OpenExplorerWithPath(string path)
    {
        open_explorer_with_path(path);
    }
}
