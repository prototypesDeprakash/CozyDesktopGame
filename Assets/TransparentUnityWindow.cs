using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentUnityWindow : MonoBehaviour
{
    // Import necessary WinAPI functions
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    // Struct for extending the frame into the client area
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    // Window styles
    private const int GWL_EXSTYLE = -20;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TRANSPARENT = 0x00000020;

    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private const uint LWA_COLORKEY = 0x00000001; // Transparency effect

    void Start()
    {
        // Get handle to the current Unity window
        IntPtr hWnd = GetActiveWindow();

        // Extend the frame to enable transparency
        MARGINS margins = new MARGINS
        {
            cxLeftWidth = -1,
            cxRightWidth = -1,
            cyTopHeight = -1,
            cyBottomHeight = -1
        };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);

        // Set the window to be transparent
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED );
        SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);

        // Make the window always on top (optional)
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0x0001);

        // Ensure the Unity game runs in the background
        Application.runInBackground = true;
    }
}
