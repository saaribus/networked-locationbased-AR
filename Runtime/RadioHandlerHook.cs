using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
/// <summary>
/// This class handles a networked interactive radio. It is played/stopped simultaneosly on all
/// clients. This makes a shared listening experience possible.
/// It works with a simple OnMouseDown, whenever a client clicks on the radio (which needs to have
/// a collider component and a rigidbody. To synchronize the playing/pausing and the playbacktime
/// over the network we use Syncvars and hooks.
/// </summary>
public class RadioHandlerHook : NetworkBehaviour
{
    [SyncVar(hook = nameof(ChangeAudioState))]
    public bool bShouldPlay = false;

    [SyncVar(hook = nameof(UpdatePlaybackTime))]
    public float serverPlaybackTime;

    private AudioSource radioSound;
  
    private void Start()
    {
        radioSound = GetComponentInChildren<AudioSource>();
        if (!radioSound)
        {
            Debug.LogWarning("No AudioSource detected. Make sure you have an AudioSource as a" +
                "child of the radio");
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //Get updated PlaybackTime this is done through syncvars when I come in later.
    }

    private void OnMouseDown()
    {
        ClientInstance ci = ClientInstance.instance;
        //if I am not the local player, then do nothing
        if (!ci.isLocalPlayer) return;


        // if I am the localplayer I tell the Server to Call the ChangeBoolean Method
        CmdChangeBoolean(!bShouldPlay);
        
    }

    //on the server toggle the boolean
    [Command(requiresAuthority = false)]
    public void CmdChangeBoolean(bool newState)
    {
        bShouldPlay = newState;

        //this change gets the ChangeAudioState called, because we have a hook there
    }

    //this gets called whenever the AudioState is changed
    public void ChangeAudioState(bool oldState, bool newState)
    {
        bShouldPlay = newState;

        if (bShouldPlay)
        {
            //Start Audio on the server
            CmdPlayAudioOnServer();
            return;
        }
        if (!bShouldPlay)
        {
            //Stop Audio on the server
            CmdPauseAudioOnServer();
            return;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayAudioOnServer()
    {
        radioSound.Play();
        radioSound.time = serverPlaybackTime;

        //make Audio Start on all Clients (also on local player)
        RpcPlayAudioOnClients();
    }

    [Command(requiresAuthority = false)]
    public void CmdPauseAudioOnServer()
    {
        serverPlaybackTime = radioSound.time;
        radioSound.Pause();

        //Make Audio PAUSE on all Clients (also on local Player)
        RpcPauseAudioOnClients();
    }

    //On all clients we need to store at what time we started playing the audio so it doesn't get
    //out of sync.
    [ClientRpc]
    public void RpcPlayAudioOnClients()
    {
        radioSound.Play();
        radioSound.time = serverPlaybackTime;
    }

    [ClientRpc]
    public void RpcPauseAudioOnClients()
    {
        radioSound.Pause();
    }

    //this gets called whenever we have saved a new playbacktime, because we have a hook there
    //this makes sure that after pausing the audio on all clients, it gets played from the same
    //spot again on all clients.
    public void UpdatePlaybackTime(float oldTime, float newTime)
    {
        serverPlaybackTime = newTime;
        Debug.Log("Time is updated now and is until next play: " + serverPlaybackTime);
    }

}
