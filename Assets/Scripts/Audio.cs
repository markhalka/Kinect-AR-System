using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JulianSchoenbaechler.MicDecode;
public class Audio : MonoBehaviour
{


    //ok, so for this one, after a snap, wait 2 seconds for the gesture to be recognized, than that counts 



    //this will use watson text to speech api (or another similar service) to turn the speech into text, and search for keywords (such as: hey Project-tron)
    public IEnumerator speechToText() { yield break; }


    //this function will handle all speech to text functionalities, and will work of keywords 
    public void handleSpeechToText() { }


    public void Start()
    {
        microphone.StartRecording();
        Debug.LogError("microphone started");
        StartCoroutine(getBackground());
    }



    public void Update()
    {
        didSnap();   
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            microphone.StartRecording();
            Debug.LogError("stopped.");

        }
    }

    //ok, so here, just spend hte first 5 seconds collecting the background audio info 
    //then, according to the sensitivity, just look for short spikes in the dbell value, and return true if they exist (has to be between lets say 10 and 15 dbells for a snap)

    public MicDecode microphone;
    int decibalMin = 8;
    int decibalMax = 35;
  
   // int timeMin = 
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


