using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

//we are using the ClientInstance.cs instead of the Basic Player Script.
//The ClientInstance.cs manages Player Network Stuff.

//This needs to be on all Prefabs you want to spawn as Playerprefabs.
//For this example we will put it on the Windows and WebGL and the AndroidPlayer-Prefab. 
//Although there is different behaviour depending from which platform the client spawns,
// is the same for all platforms.
public class ClientInstance : NetworkBehaviour
{
    public static ClientInstance instance;

    //this is an Event that gets called on all ClientInstances, whenever a new ClientInstance is joining
    //public static event Action OnNewClientOnServer;

    #region Setting up Event for Keeping up to date with client information
   /* public override void OnStartServer()
    {
        base.OnStartServer();
        OnNewClientOnServer += ClientInstance_OnNewClientOnServer;
        //OnNewClientOnServer?.Invoke();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        OnNewClientOnServer -= ClientInstance_OnNewClientOnServer;
    }

    public void ClientInstance_OnNewClientOnServer()
    {
        Debug.Log("This is called now, on " + gameObject.name);
    }*/

    #endregion

    //#region Spawn Client as a Set of GameObjects with individual NetworkIdentities
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        instance = this;
        
    }


}
