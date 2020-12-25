using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public static bool safeMode = false;
    public static GameObject currentWindow;

    [SerializeField]
    public static GameObject windowList; //not sure if that would work...

    public GameObject handObject;

    static Vector2 windowVec = new Vector2(0, 0);
    static Vector3 previousHand = new Vector3(0, 0, 0);
    static Vector2 nullVector = new Vector2(0, 0);

    public enum safeWindowState { HIDDEN, SHOWN, PARTIAL };
    public static safeWindowState safeState = safeWindowState.SHOWN;

 //   static List<Window> windows;


    #region maximize Windows

    public static void MaximizeWindow(int pid)
    {
        IDictionary<IntPtr, string> windows = List_Windows_By_PID(pid);
        foreach (KeyValuePair<IntPtr, string> pair in windows)
        {
            var placement = new WINDOWPLACEMENT();
            GetWindowPlacement(pair.Key, ref placement);
            if (placement.showCmd == SW_SHOWMINIMIZED)
            {
                Debug.LogError("maxamizing: " + pair.Value);
                ShowWindowAsync(pair.Key, SW_SHOWMAXIMIZED);
            }
        }
    }

    public static void MinimizeWindow(int pid)
    {
        IDictionary<IntPtr, string> windows = List_Windows_By_PID(pid);
        foreach (KeyValuePair<IntPtr, string> pair in windows)
        {
            var placement = new WINDOWPLACEMENT();
            GetWindowPlacement(pair.Key, ref placement);
            if (placement.showCmd == SW_SHOWMAXIMIZED)
            {
                Debug.LogError("minimizing: " + pair.Value);
                ShowWindowAsync(pair.Key, SW_SHOWMINIMIZED);
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


    public static void hideWindow(Window window)
    {
        window.gameObject.SetActive(false);
        //  WindowManager.MinimizeWindow(processId);
    }

    public static void showWindow(Window window)
    {
        window.gameObject.SetActive(true);
        // WindowManager.MaximizeWindow(processId);
    }

    public static void destroyWindow(GameObject window)
    {
        if (window != null)
        {
            if (window == currentWindow)
            {
                currentWindow = null;
            }
            Destroy(window);
        }
    }


    public static void moveCurrentWindow(Hand hand)
    {
        if (currentWindow != null)
        {
            if (hand.currentState == P_HandState.CLOSED)
            {
                moveWindow(hand.position);
            }
            else
            {
                currentWindow = null;
            }
        }
    }

    const int MIN_THRESH = 10; // how many ticks are neccessary before you are no longer resizing 
    static float resizeTickCount = 0;

    public static void checkResizing(Hand right, Hand left) //check this code...
    {
        if ((mode.currentMode == modes.RESIZING && resizeTickCount < MIN_THRESH) || left.currentState == P_HandState.CLOSED && right.currentState == P_HandState.CLOSED && currentWindow!=null) //than you can resize the window
        {
            resizeTickCount += 1;
            if(left.currentState == P_HandState.CLOSED && right.currentState == P_HandState.CLOSED)
            {
                resizeTickCount = 0;
            }

            resizeWindowDistance(currentWindow.GetComponent<Window>(), 20 * Vector3.Distance(right.position, left.position));
            mode.currentMode = modes.RESIZING;
        }
        else if(mode.currentMode == modes.RESIZING)
        {
            resizeTickCount = 0;
            left.updateState();
            right.updateState();
            mode.currentMode = modes.NONE;
        }
    }


    public static void showSafeWindows()
    {
        if (safeState == safeWindowState.SHOWN)
        {
            return;
        }

        foreach (Window w in windowList.GetComponentsInChildren<Window>())
        {
            if (w.getSafeMode())
            {
                showWindow(w);
            }
        }

        safeState = safeWindowState.SHOWN;
    }

    public static void hideSafeWindows()
    {
        if (safeState == safeWindowState.HIDDEN)
        {
            return;
        }

        foreach (Window w in windowList.GetComponentsInChildren<Window>())
        {
            if (w.getSafeMode())
            {
                hideWindow(w);
            }
        }
        safeState = safeWindowState.HIDDEN;
    }

    public static void destroyCurrentWindow()
    {
        if (currentWindow != null)
        {
            destroyWindow(currentWindow);
        }
    }

    static float previous = 0;
    public static void resizeWindow(Window window, float amount)
    {
        if (window == null)
        {
            return;
        }
        window.GetComponent<uWindowCapture.UwcWindowTexture>().scalePer1000Pixel += amount;
    }

    public static void resizeWindowDistance(Window window, float current)
    {
        if (window == null)
        {
            return;
        }

        if (previous == 0)
            previous = current;

        float delta = previous - current;

        resizeWindow(window, delta);

        previous = current;
    }


    public static void moveWindow(Vector3 current)
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
}


