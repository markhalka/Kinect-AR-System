using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MenuOption
{
   public Settings() : base("Settings")
    {
        submenus = new List<MenuOption>(new MenuOption[] { new Brightness(), new Recalibration(), new DeviceInfo(), new GestureSettings() });
    }

    public override void handle()
    {
        base.handle();
    }
}
