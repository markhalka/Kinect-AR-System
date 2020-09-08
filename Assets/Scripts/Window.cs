using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Window : MonoBehaviour
{

    public GameObject currentWindow = null;
    public GameObject handObject;

    #region minimizeWindows
    public static void CloseFirefoxWindow()
    {
        string processName = "PaintDotNet";
        CloseWindow(processName);
    }


    public static void CloseWindow(string processName)
    {
        Process[] processes = Process.GetProcessesByName(processName);
        Debug.LogError(processes.Length + " length");
        if (processes.Length > 0)
        {
            foreach (var process in processes)
            {
                IDictionary<IntPtr, string> windows = List_Windows_By_PID(process.Id);
                foreach (KeyValuePair<IntPtr, string> pair in windows)
                {
                    var placement = new WINDOWPLACEMENT();
                    GetWindowPlacement(pair.Key, ref placement);

                    if (placement.showCmd == SW_SHOWMINIMIZED)
                    {
                        Debug.LogError("maxamizing");
                        //if minimized, show maximized
                        ShowWindowAsync(pair.Key, SW_SHOWMAXIMIZED);
                    }
                    else
                    {
                        Debug.LogError("minimizing");
                        //default to minimize
                        ShowWindowAsync(pair.Key, SW_SHOWMINIMIZED);
                    }
                }
            }
        }
    }


    private const int SW_SHOWNORMAL = 1;
    private const int SW_SHOWMINIMIZED = 2;
    private const int SW_SHOWMAXIMIZED = 3;

    private struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public System.Drawing.Point ptMinPosition;
        public System.Drawing.Point ptMaxPosition;
        public System.Drawing.Rectangle rcNormalPosition;
    }

    [DllImport("user32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("USER32.DLL")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    private static extern IntPtr GetShellWindow();

    [DllImport("USER32.DLL")]
    private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);


    public static IDictionary<IntPtr, string> List_Windows_By_PID(int processID)
    {
        IntPtr hShellWindow = GetShellWindow();
        Dictionary<IntPtr, string> dictWindows = new Dictionary<IntPtr, string>();

        EnumWindows(delegate (IntPtr hWnd, int lParam)
        {
            //ignore the shell window
            if (hWnd == hShellWindow)
            {
                return true;
            }

            //ignore non-visible windows
            if (!IsWindowVisible(hWnd))
            {
                return true;
            }

            //ignore windows with no text
            int length = GetWindowTextLength(hWnd);
            if (length == 0)
            {
                return true;
            }

            uint windowPid;
            GetWindowThreadProcessId(hWnd, out windowPid);

            //ignore windows from a different process
            if (windowPid != processID)
            {
                return true;
            }

            StringBuilder stringBuilder = new StringBuilder(length);
            GetWindowText(hWnd, stringBuilder, length + 1);
            dictWindows.Add(hWnd, stringBuilder.ToString());

            return true;

        }, 0);

        return dictWindows;
    }


    #endregion

 

    Vector2 windowVec = new Vector2(0, 0);
    Vector3 previousHand = new Vector3(0, 0, 0);
    Vector2 nullVector = new Vector2(0, 0);

    public void destroyWindow(GameObject window)
    {      
        if (currentWindow != null && window == currentWindow)
        {
            currentWindow = null;
            handObject.SetActive(false);
            Destroy(window);
        }
    }
   
    float previous = 0;
    public void resizeWindow(float current)
    {
        if (currentWindow == null)
            return;

        if (previous == 0)
            previous = current;

        float delta = previous - current;

        currentWindow.GetComponent<uWindowCapture.UwcWindowTexture>().scalePer1000Pixel -= delta;

        previous = current;
    }


    public void moveWindow(Vector3 current)
    {
        if (currentWindow == null)
            return;

        if (previousHand == new Vector3(0, 0, 0))
        {
            previousHand = current;
        }

        Vector3 delta = previousHand - current;

        currentWindow.transform.Translate(-delta.x * 50, -delta.y * 50, 0);
        previousHand = current;
    }

    public void selectWindow()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(handObject.transform.position, handObject.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(handObject.transform.position, handObject.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            selectWindow(hit.collider.gameObject);
        }
        else
        {
            Debug.DrawRay(handObject.transform.position, handObject.transform.TransformDirection(Vector3.down) * 1000, Color.white);
        }
    }

    void selectWindow(GameObject curr)
    {
        if (curr.GetComponent<Image>() != null)
            curr.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        currentWindow = curr;
    }


}
