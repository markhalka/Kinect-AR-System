using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MenuOption
{
    public Effects() : base("Effects")
    {
        submenus = new List<MenuOption>(new MenuOption[] { new SelectEffects(), new EditEffects(), new CreateEffects() });       
    }

    public override void handle()
    {
        base.handle();
    }
}
