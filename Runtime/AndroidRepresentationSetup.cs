using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

//we use this to connect to the MoveTarget, if we are on mobile and when we connect to server
public class AndroidRepresentationSetup : NetworkBehaviour
{
    NavMeshAgent agent;
    GameObject navMeshTarget;

    private Transform gpsCameraTransform;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Debug.Log("Android Authority started NOW!");
        agent = GetComponent<NavMeshAgent>();
        Debug.Log("WE do have a agent" + agent);
        
        navMeshTarget = FindObjectOfType<GPSInputLocal>().gameObject;

        SetupCameraReference();
    }

    public void Update()
    {
        //this only happens after the client came into existence
        //the agent is the NavMeshAgent Component on the MobilePlayer Prefab. It only exists after On Start Authority
        if (navMeshTarget && agent)
        {
            agent.destination = navMeshTarget.transform.position;

            UpdateRotationOfRepresentation();
            MakeCamFollowPlayer();
        }
        
    }

    private void SetupCameraReference()
    {
        Camera[] allCameras = Camera.allCameras;
        foreach (Camera cam in allCameras)
        {
            if (cam.gameObject.CompareTag("gpsCam"))
            {
                gpsCameraTransform = cam.gameObject.transform;
            }

        }

        gpsCameraTransform.localPosition = new Vector3(0, 0, 0);

        if (gpsCameraTransform)
        {
            Debug.Log("We found the GPS Cam ON ANDROID");
        }
        else
        {
            Debug.LogWarning("ANDROID: We dont have a Camera");
        }

    }

    private void UpdateRotationOfRepresentation()
    {
        //UPDATE HORIZONTAL ROTATION to the Representation if we look around with the phone
        //only apply the rotation xz, the representation only rotates around the y axis
        float yRotation = gpsCameraTransform.eulerAngles.y;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
    }

    //we make the Camera have the same position as the player, without childing it.
    //Childing is not an option, because we need to move the camera through the gyroscope and then update the
    //rotation of the player accordingly. If we child the camera it results in an infite regress, which makes
    //the camera movement look very jittery.
    private void MakeCamFollowPlayer()
    {
        if (isOwned)
        {
            Vector3 calculatedCamPos = new Vector3(transform.position.x, 1.6f, transform.position.z);
            gpsCameraTransform.position = calculatedCamPos;
            //Debug.Log("GPS Cam Pos ist: " + gpsCameraTransform.position);
        }
    }
}
