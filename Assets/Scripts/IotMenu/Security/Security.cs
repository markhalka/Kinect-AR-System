using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security : MenuOption
{
    public Security() : base("Security")
    {
        submenus = new List<MenuOption>(new MenuOption[] { new CameraSettings(), new EditCameras(), new Recordings(), new ViewCameras() });
    }
    public override void handle()
    {

    }
}
