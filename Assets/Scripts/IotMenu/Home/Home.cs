using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MenuOption
{
    public Home() : base("Home")
    {
        submenus = new List<MenuOption>(new MenuOption[] { new Settings(), new IOT(), new Effects(), new Security() });
    }
    public override void handle()
    {
        base.handle();
    }
}
