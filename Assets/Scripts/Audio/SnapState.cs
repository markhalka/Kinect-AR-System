using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SnapState
{
    float SNAP_THRESH = 2;
    float snapTime;
    public bool detectedSnap;
    Audio audio;

    public SnapState()
    {
        audio = new Audio();
        snapTime = 0;
        detectedSnap = false;
    }

    public void checkSnap()
    {
        if (audio.didSnap())
        {
            detectedSnap = true;
            snapTime = 0;
        }

        if (detectedSnap)
        {
            snapTime += Time.deltaTime;

            if (snapTime > SNAP_THRESH)
            {
                detectedSnap = false;
                snapTime = 0;
            }
        }
    }
}
