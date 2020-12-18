using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuOption : MonoBehaviour
{
    public string name;
    public List<MenuOption> submenus;

    public MenuOption(string name)
    {
        this.name = name;
    }
    public virtual void handle()
    {

    }


}
