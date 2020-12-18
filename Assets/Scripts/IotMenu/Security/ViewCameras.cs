using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCameras : MenuOption
{
    public ViewCameras() : base("View Cameras")
    {

    }

    public void openCamera()
    {
        cameras = new List<Camera>(); //this is where you would init cameras, these could be from a text file or list somewhere (maybe xml)


        foreach (var c in cameras)
        {
            GameObject newPanel = Instantiate(cameraPanel, cameraPanel.transform.parent);
            newPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = c.name;
            newPanel.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "status: " + c.status;
            newPanel.transform.GetChild(2).GetComponent<TMPro.TMP_Text>().text = "location: " + c.location;

        }
    }

    public override void handle()
    {
        if (currentWindow != null)
        {
            //that camera is selected, now open the settings options for that camera, and then done 
            int index = 0;
            for (; index < cameraPanel.transform.childCount; index++)
            {
                if (cameraPanel.transform.GetChild(index).gameObject == currentWindow)
                {
                    break;
                }
            }
            currentCamera = cameras[index];
            currentWindow = null;
            openMenu("Camera settings", new string[] { }); //add the values here, and functions to deal with them
        }
    }
}
