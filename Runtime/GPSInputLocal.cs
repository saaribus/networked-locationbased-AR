using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GPSInputLocal : MonoBehaviour
{

#if UNITY_ANDROID

    /* [Tooltip("Set y-Position")]
     [SerializeField] float baseOffset = 2.0f;*/

    [Header("This Script is only used on MOBILE")]
    [Space(7)]
    [SerializeField] TextMeshProUGUI debugLatitude;
    [SerializeField] TextMeshProUGUI debugLongitude;

    private float gpsAccuracy = 3f;

    //the minimum distance in meters the device must move laterally before Input.location gets Updated!   
    private float gpsIntervall = 0.1f;

    //this is for debugging reasons, if you want to display the GPS Coordinates in e.g. a TextMeshPro
    private double lat,lon;
    
    //[SerializeField]
    //Vector3 startLatLonDegrees;
    [Header("Latitude and Longitude of Point of Reference")]
    [Space(7)]
    [Tooltip("Set the float to the Latitude of your chosen Point of Reference")]
    public double referenceLatitude;
    [Tooltip("Set the float to the Longitude of your chosen Point of Reference")]
    public double referenceLongitude;

    Vector3 worldPosition;

    private void Start()
    {
        if (referenceLatitude == 0 || referenceLongitude == 0)
        {
            Debug.LogError("The Latitude and Longitude of the Point of Reference can " +
                "not be Zero. Please set the float of ReferenceLatitude and ReferenceLongitude");

        }

        //We only want to update the movement of the GameObject on the owner Client, not on the other clients
        //if (isOwned)
        //{
            Input.location.Start(gpsAccuracy, gpsIntervall);
        //}
    }

    private void Update()
    {
        //if (isOwned)
        //{
            if (!Application.isEditor)
            {
                //if we're not in the Editor, and on Android, get our Data from Sensors
                GetSensorData();

                //transform our GPS position to Unity Coordinates
                TranslateGPSToUnity();

                //Apply the calculated Transform.Position to the NavMeshMoveTarget. Because the GPS Camera
                //has a NavMeshAgent Component, it will follow the target every frame
                ApplyLocation();

            }
        //}
    }


    void GetSensorData()
    {
        
        lat = Input.location.lastData.latitude;
        lon = Input.location.lastData.longitude;

        debugLatitude.text = lat.ToString();
        debugLongitude.text = lon.ToString();
        
    }

    void TranslateGPSToUnity()
    {
        double dlat = referenceLatitude - lat;
        double dlon = referenceLongitude - lon;

        //double dlat = startLatLonDegrees.x - lat;
        //double dlon = startLatLonDegrees.z - lon;

        //Calculate lat und lon to planar xy Coordinates - central europe
        //Basel: dlon * 75118 dlat * 111300
        //Berlin: dlon * 67000 dlat * 111300
        double xPosition = dlon * 67000;
        double zPosition = dlat * 111300;

        //Apply to Absolute Unity Location using initalHeight
        worldPosition = new Vector3((float)xPosition, 0, (float)zPosition);

    }

    void ApplyLocation()
    {
        //Move the Target (myself) in the unity world according to the GPS Data
        transform.position = worldPosition;
    }


#endif
}