using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroRotation : MonoBehaviour
{
#if UNITY_ANDROID

    [Header("This Script is only used on MOBILE")]
    [Space(7)]
    [SerializeField] string test = "";
   
    Gyroscope gyro;
    Compass compass;

    //Eine zusätzliche Drehung um die Gyro Rotation mit der Kamera in Unity kompatibel zu machen.
    Quaternion gyroRotation;
    float compassHeading;

    private void Start()
    {
        SetupGyro();
    }

    void SetupGyro()
    {
        gyro = Input.gyro;
        gyro.enabled = true;

        compass = Input.compass;
        compass.enabled = true;

        //gyroRotation = Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(0, 1, 0));

        gyro.updateInterval = 0.0f;
    }

    private void Update()
    {
        if (!ClientInstance.instance) return;

        if (!Application.isEditor)
        {
            GetSensorData();
            OrientVirtualCamera();
        }
    }

    void GetSensorData()
    {
        gyroRotation = gyro.attitude;
        compassHeading = compass.magneticHeading;
    }

    void OrientVirtualCamera()
    {
        //Unity Camera rotates according to Gyro
        transform.rotation = gyroRotation;


        if (!Application.isEditor)
        {
            transform.Rotate(0, 0, 180, Space.Self);
            transform.Rotate(90, 0, 0, Space.World);
        }
    }




#endif
}