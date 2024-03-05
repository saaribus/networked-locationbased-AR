using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

/// <summary>
/// This clas if for saving the string to the specific note GameObject, so if a client connects
/// later or reconnects, the notes know what their string/message is. Also it handles the deleting
/// of itself whenever a owner is pressing the "button" to delete a note GameObject.
/// </summary>
public class InteractableNote : NetworkBehaviour
{
    [SyncVar]
    public string stringToKeep;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (stringToKeep != "")
        {
            Debug.Log(stringToKeep);
            gameObject.GetComponentInChildren<TextMeshPro>().text = stringToKeep;
            
        }

    }

    [Command]
    public void CmdObjektVomServerLoeschen()
    {
        NetworkServer.Destroy(gameObject);
        Debug.Log("Note " + gameObject.GetComponent<NetworkIdentity>().netId + "is gone");
    }
}
