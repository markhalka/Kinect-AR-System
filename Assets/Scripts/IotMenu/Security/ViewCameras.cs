using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCameras : MenuOption
{
    public ViewCameras() : base("View Cameras")
    {
        submenus = new List<MenuOption>(new MenuOption[] { new BackDoorCam(), new KitchenCam(), new FrontDoorCam(), new BasementCam(), new MasterBedroom() });
    }

    public void openCamera()
    {
        List<SecurityCamera> cameras = new List<SecurityCamera>(); 
    }

    public override void handle()
    {

    }
}
