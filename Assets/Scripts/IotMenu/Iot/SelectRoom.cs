using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoom : MenuOption
{
 
    public SelectRoom() : base("Select Room")
    {

    }
    public override void handle()
    {
        //this will call open menu, and show all rooms
        List<IOTRoom> rooms = new List<IOTRoom>(); //tihs will initialize the rooms (probably from a text file or something)

    }
}
