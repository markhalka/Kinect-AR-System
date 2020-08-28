using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{

    public void onButtonClick()
    {
        Main.currentWindow = gameObject;
        Debug.LogError("it was clicked!!!");
    }

/*
    public string name;

    public GameObject window;

    */

}
