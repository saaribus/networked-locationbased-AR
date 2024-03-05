using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Processes the string, the position of the player to instantiate a note across on every player.
/// The note is positioned infront of the player that writes the note, of course this can be
/// changed individually. The note stores its owner, so it can only be deleted by its original poster.
/// </summary>
public class NoteSpawnManager : NetworkBehaviour
{
    [SyncVar]
    public string myInputText;

    [SerializeField] GameObject notePrefab;

    private Vector3 spawnPos;
    [SerializeField] TMP_InputField _inputField;

    public void ProcessInput(string receivedString)
    {
        ClientInstance ci = ClientInstance.instance;

        //we need to find out where the player that just inputted a text is standing, so we can
        //spawn the note infront of them
        Vector3 playerPos = ci.transform.position;
        Vector3 playerDir = ci.transform.forward.normalized;
        Quaternion spawnRot = ci.transform.rotation;

        //we then want to spawn it in front of us, and just above the floor
        //adjust offsettY depending on how tall your NotePrefab is
        Vector3 offsetY = new Vector3(0, 0.8f, 0);
        float offsetZ = 3f;

        //calculating the spawnposition with the above gathered information
        spawnPos = playerPos + playerDir * offsetZ + offsetY;

        //preparing the networkconnection of the writing player
        NetworkConnectionToClient _conn = ci.connectionToClient;

        //Calling the method on the server
        CmdSetVar(receivedString, spawnPos, spawnRot, _conn);

    }

    [Command(requiresAuthority = false)]
    public void CmdSetVar(string stringToWrite, Vector3 spawnPos, Quaternion spawnRot, NetworkConnectionToClient conn)
    {
        myInputText = stringToWrite;

        GameObject newNote = Instantiate(notePrefab, spawnPos, spawnRot);
        newNote.GetComponent<InteractableNote>().stringToKeep = myInputText;
        newNote.GetComponentInChildren<TextMeshPro>().text = myInputText;

        //Server should spawn this new Object and define the right authority (only the player that
        //wrote the note, is able to delete it)
        NetworkServer.Spawn(newNote, conn);

        //also tell the server to call the ClientRpc so every client can see the new note
        RpcUpdateText(myInputText, newNote);
    }

    [ClientRpc]
    public void RpcUpdateText(string stringForNote, GameObject go)
    {
        go.GetComponent<InteractableNote>().stringToKeep = stringForNote;
        go.GetComponentInChildren<TextMeshPro>().text = go.GetComponent<InteractableNote>().stringToKeep;
    }

    //we delete the string in the input field, once we have instantiated it on the server
    public void ClearField()
    {
        _inputField.GetComponent<TMP_InputField>().text = "Enter Text...";
        var eventSystem = EventSystem.current;
        if (!eventSystem.alreadySelecting) eventSystem.SetSelectedGameObject(null);

    }

}
