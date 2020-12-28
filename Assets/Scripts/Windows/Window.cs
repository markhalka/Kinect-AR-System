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

    int processId = 0;
    public int id { get; set; }
    bool safeMode = false;

    public void Start()
    {
        id = WindowMenu.windowIds++;
        gameObject.transform.GetChild(0).GetComponentInChildren<TMPro.TMP_Text>().text = id.ToString();
    }

    public void setSafeMode(bool mode)
    {
        safeMode = mode;
    }

    public bool getSafeMode()
    {
        return safeMode;
    }

    public void setProcessId(int id)
    {
        processId = id;
    }

    public void onButtonClick()
    {
        WindowManager.currentWindow = gameObject;
    }
}
