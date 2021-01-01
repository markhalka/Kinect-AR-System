using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOT : MenuOption
{
    public IOT() : base("IOT")
    {
        submenus = new List<MenuOption>(new MenuOption[] { new Temperature(), new IOTWindows(), new Lights(), new SelectRoom(), new IOTSettings() });
    }
    public override void handle()
    {
        base.handle();
    }
}
