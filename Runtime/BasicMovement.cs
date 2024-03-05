using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
/// <summary>
/// this is the movment class, so the player can move with mouse and keyboard
/// </summary>
public class BasicMovement : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float sensitivity = 100f;

    public enum LookMode { LookWhenRightButtonDown, LookWhenMouseMove }; 
    public LookMode lookMode;

    private Transform cameraTransform;
    [SerializeField] private GameObject representation;

    public override void OnStartClient()
    {
        base.OnStartClient();
        //only do anything on the local player. We dont want to be able to move other players around
        if (!isLocalPlayer) return;

        Camera[] allCameras = Camera.allCameras;
        foreach (Camera cam in allCameras)
        {
            if (cam.gameObject.CompareTag("gpsCam"))
            {
                cameraTransform = cam.gameObject.transform;
                
            }

        }

        cameraTransform.localPosition = new Vector3(0, 1.1f, 0);
     
        if (cameraTransform)
        {
            //we want to child the GpsCamera, so it moves with the player
            cameraTransform.parent = gameObject.transform;
            Debug.Log("GPSCam should be Child of the player now");
            cameraTransform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            Debug.LogWarning("We dont have a Camera");
        }

    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (!isLocalPlayer) return;

        cameraTransform.parent = null;
        Debug.Log("Saved Camera from being destroyed");
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 3f;
        }

        // Handle movement
        HandleMovement();

        // Handle rotation based on mouse input
        switch (lookMode)
        {
            case LookMode.LookWhenRightButtonDown:
                if (Input.GetKey(KeyCode.Mouse1)) HandleMouseLook();
                break;

            case LookMode.LookWhenMouseMove:
                HandleMouseLook();
                break;
        }

    }

    void HandleMovement()
    {
        //Get the value from -1 (left) to 1 (right) from the currently configured Unity Input System Horizontal Axis
        float xValue = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;

        //Get the value from -1 (down) to 1 (up) from the currently configured Unity Input System Vertical Axis
        float zValue = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        //Add the values to a Vector3 and leave the y-Value to 0
        Vector3 movement = new Vector3(0, 0, 0);

        movement = Vector3.right * xValue + Vector3.forward * zValue;
          
        //Apply the new Vector to the Object we want to move
        gameObject.transform.Translate(movement);

    }

    void HandleMouseLook()
    {
        //Get the value from -1 (left) to 1 (right) from the currently configured Unity Input System Mouse Delta
        float hRot = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        //Get the value from -1 (down) to 1 (up) from the currently configured Unity Input System Mouse Delta
        float vRot = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //HORIZONTAL ROTATION
        //We want the camera to rotate horizontaly around the World's y-Vector 
        
        //Apply the Horizontal Rotation to the whole GameObject (and therefore also to the representation and the camera) in world space
        gameObject.transform.Rotate(Vector3.up, hRot, Space.World);
       
        //VERTICAL ROTATION
        //We want to rotate vertically around the GameObject's "right" Vector 

        //we want to make sure people can't "overrotate", so we need to limit the amount of up- or down-rotation
        //we can check the amount of up or down rotation by comparing against the y-part of the GameObject's forward Vector
        //and adjust our vertical rotation accordingly
        if (cameraTransform.forward.y > 0.2f)
        {
            vRot = Mathf.Min(vRot, 0.0f);
        }
        else if (cameraTransform.forward.y < -0.2f)
        {
            vRot = Mathf.Max(vRot, 0.0f);
        }

        //apply Vertical Rotation to the Camera, relative to the Camera (Space.Self)
        cameraTransform.Rotate(Vector3.left, vRot);

    }

}
