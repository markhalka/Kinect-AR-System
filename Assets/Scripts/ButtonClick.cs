using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public void onButtonClick()
    {
        IotMenu.buttonCall = transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text;
    }
}
