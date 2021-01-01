using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD1 : MonoBehaviour {
     GameObject outerCircle;
     GameObject cntrCircle;
     GameObject innerCircle;
     GameObject vLine;
     GameObject vLineB;
     GameObject bLine;

    public TMPro.TMP_Text Logo;
    FadeText fadeText;

    // Use this for initialization
    void Start () {
        outerCircle = transform.GetChild(0).gameObject;
        cntrCircle = transform.GetChild(1).gameObject;
        innerCircle = transform.GetChild(2).gameObject;
        vLine = transform.GetChild(3).gameObject;
        vLineB = transform.GetChild(4).gameObject;
        bLine = transform.GetChild(5).gameObject;

        fadeText = new FadeText();
    }

    float start_count = 0;
    float time_count = 0;

    const float TIME_DELAY_START = 2;
    const float DURATION = 2;
    const float TEXT_DURATION = 1;
    const float TIME_DELAY_END = 1;
    const float END_DURATION = 2;


    Vector2 speed = new Vector2(1, 1);
    bool startedText = false;
    bool startedFadeOut = false;


	void Update () {
        start_count+=Time.deltaTime;
        if(start_count > TIME_DELAY_START)
        {
            time_count+=Time.deltaTime;
            speed = Vector2.Lerp(speed, new Vector2(0,0), time_count / DURATION);
        }

        if(time_count >= DURATION)
        {
            speed.x = 0;
            if (!startedText)
            {
                startedText = true;
                StartCoroutine(fadeText.FadeTextToFullAlpha(TEXT_DURATION, Logo));
                Debug.LogError("showing text");
            }
        }
        foreach(Animator a in transform.GetComponentsInChildren<Animator>())
        {
            a.speed = speed.x;
        }

        vLine.transform.Rotate(new Vector3(vLine.transform.rotation.x, vLine.transform.rotation.y, vLine.transform.rotation.z + 5f));
        vLineB.transform.Rotate(new Vector3(vLineB.transform.rotation.x, vLineB.transform.rotation.y, vLineB.transform.rotation.z - 5f));
        bLine.transform.Rotate(new Vector3(bLine.transform.rotation.x, bLine.transform.rotation.y, bLine.transform.rotation.z - 2f));
        innerCircle.transform.Rotate(new Vector3(innerCircle.transform.rotation.x, innerCircle.transform.rotation.y, innerCircle.transform.rotation.z + 2f));

        if(time_count >= DURATION + TIME_DELAY_END)
        {
            if (!startedFadeOut)
            {
                startedFadeOut = true;
                foreach (Image i in transform.parent.GetComponentsInChildren<Image>())
                {
                    StartCoroutine(fadeOut(i, END_DURATION));
                }
            }        
        }
    }

    IEnumerator fadeOut(Image i, float t)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);
    }

}
