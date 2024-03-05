using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////this class needs to be on the Gps Camera
///make sure you have the proper POI Setup as shown in the Example Scene
///The POI GameObject, where the collider lies, need the Tag "POI" for the code to work.
///Also it's important that the Collider on the POI is not Trigger (isTrigger = false)
public class TriggerChecker : MonoBehaviour
{
    private GameObject currentPOI;

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("POI"))
        {
            currentPOI = other.gameObject;
            Debug.Log("We hit a poi");

            //On the PoiManager of each POI you can write what happens
            //if you enter the poi
            currentPOI.GetComponent<Poi>().StartCoroutine("DoStuffInPoi");   

        }
    }

    //for Windows and WebGL add On TriggerExit that only applys to Windows/WebGL
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("POI"))
        {
            currentPOI = other.gameObject;
            //Debug.Log("We left Poi" + other.name);

            if (!Application.isMobilePlatform)
            {
                currentPOI.GetComponent<Poi>().StartCoroutine("LeavingPoi");
            }

        }
    }
}
