using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandObject : MonoBehaviour
{
    float speed = 10;
    float handSens = 2.5f;

    Vector3 zeroVector = new Vector3(0, 0, 0);
    Vector3 lastPosition;
    void Start()
    {
        lastPosition = zeroVector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        transform.position = zeroVector;
        lastPosition = zeroVector;
    }

 
    public void updateHandObject(Vector3 position)
    {
        transform.position += new Vector3((-lastPosition.x + position.x) * speed * handSens, (-lastPosition.y + position.y) * speed * handSens, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        lastPosition = position;
    }

  /*  public GameObject select()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        // If it hits something...
        if (hit.collider != null)
        {
            return hit.transform.gameObject;
        } else
        {
            return null;
        }
    }*/

    public void selectWindow()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            selectWindow(hit.collider.gameObject);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
        }
    }

    void selectWindow(GameObject curr)
    {
        if (curr.GetComponent<Image>() != null)
            curr.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        WindowManager.currentWindow = curr;
    }
}
