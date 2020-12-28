using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JulianSchoenbaechler.MicDecode;
public class Audio : MonoBehaviour
{

    public void Start()
    {
        microphone.StartRecording();
        StartCoroutine(getBackground());
    }

    public void Update()
    {
      //  didSnap();   
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            microphone.StartRecording();
            Debug.LogError("stopped.");

        }
    }

    public MicDecode microphone;
    int decibalMin = 8;
    int decibalMax = 35;
    public bool didSnap()
    {
        if (!acquiredBackground)
            return false;

        float value = microphone.VolumeDecibel;
        float difference = value - background;
        if (difference > decibalMin && difference < decibalMax)
        {
            Debug.LogError("snap detected");
            return true;
        }
        return false;
    }

    int count = 0;
    float background = 0;
    float delay = 0.1f;
    float backgroundTime = 5;
    bool acquiredBackground = false;
    IEnumerator getBackground()
    {
        while(count < backgroundTime / delay)
        {
            count++;
            background += microphone.VolumeDecibel;
            yield return new WaitForSeconds(delay);
        }
        Debug.LogError("done getting background");
        background /= count;
        acquiredBackground = true;

    }
}


