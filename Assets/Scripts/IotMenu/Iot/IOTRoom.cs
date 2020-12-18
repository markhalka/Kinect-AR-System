using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOTRoom
{
    public List<IOTDevice> devices;
    public string name;


    List<IOTRoom> rooms;
    //initialize the room and all connected devices
    void openRoom()
    {
        //this will just call open menu
        List<string> deviceNames = new List<string>();
        foreach (var d in currentRoom.devices)
        {
            deviceNames.Add(d.name);
        }
        openMenu(currentRoom.name, deviceNames.ToArray());
    }
}

    
