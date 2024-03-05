using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// This class makes a Quad with a Collider behave like a deletion-button. Whenever its hit,
/// it checks if the clicker has authority over the note Game Object and if yes, deletes it.
/// </summary>
public class PseudoButtonOnClick : MonoBehaviour
{
    private InteractableNote interactNote;

    private void Start()
    {
        interactNote = gameObject.GetComponentInParent<InteractableNote>();
        if (!interactNote)
        {
            Debug.LogWarning("Didn't find the interactablenot, please use the NotePrefab for this");
        }
    }

    private void OnMouseDown()
    {
        print("just hit the Pseudobutton with my mouse!");

        //Debug.Log(interactNote.GetComponent<NetworkIdentity>().netId.ToString());
        //Debug.Log("Do I Have Authority? " + interactNote.GetComponent<NetworkIdentity>().isOwned);

        //we check if I have authority over the note, if I was the original poster.
        if (interactNote.GetComponent<NetworkIdentity>().isOwned)
        {
            Debug.Log("I have authority over this Note and can delete it");

            interactNote.CmdObjektVomServerLoeschen();
        }
        else if (!interactNote.GetComponent<NetworkIdentity>().isOwned)
        {
            
            Debug.Log("I CANNOT delete this note");
        }
        else { Debug.Log("schrödingers Authority"); }


    }
}