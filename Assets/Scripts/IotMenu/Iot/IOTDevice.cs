using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOTDevice 
{

    public enum IOTType { door, window, blind, light, other }
    public enum IOTValues { open, closed, locked, unlocked }

    public string name;
    public string room;
    public IOTType type;
    public IOTValues value;

}
