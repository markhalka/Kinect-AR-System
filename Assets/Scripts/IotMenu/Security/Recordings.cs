using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recordings : MenuOption
{

    public Recordings() : base("Recordings")
    {

    }

    public override void handle()
    {
        if (currentWindow != null)
        {
            //that recording is selected, so just open the settings than done 
            //that camera is selected, now open the settings options for that camera, and then done 
            int index = 0;
            for (; index < recordingPanel.transform.childCount; index++)
            {
                if (recordingPanel.transform.GetChild(index).gameObject == currentWindow)
                {
                    break;
                }
            }
            currentRecording = recordings[index];
            currentWindow = null;
            openMenu("Recording settings", new string[] { }); //add the values here, and functions to deal with them
        }
    }

    public void openRecordings()
    {
        recordings = new List<Recording>(); //this is where you would list the recordings (same thing here)
        GameObject newRecordingPanel;
        foreach (var r in recordings)
        {
            if (r.seen)
            {
                newRecordingPanel = Instantiate(recordingPanel, seenContainer.transform);
            }
            else
            {
                newRecordingPanel = Instantiate(recordingPanel, unseenContainer.transform);
            }
            newRecordingPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = r.date;
            newRecordingPanel.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = r.location;

        }
    }

}
