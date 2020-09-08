using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windowClick : MonoBehaviour
{
    //assign this in the script instead of window
    public void onButtonClick()
    {
        Main.window.currentWindow = gameObject;
    }
}
