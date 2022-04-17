using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{

    PhotonView _photonView;

    Rigidbody rb;

    bool _isBeingHeld = false;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isBeingHeld)
        {
            //object being grabbed
            rb.isKinematic = true;
            gameObject.layer = 11;
        }
        else
        {
            //Not grabbed
            rb.isKinematic = false;
            gameObject.layer = 9;
        }


    }

    private void TransferOwnership()
    {
        _photonView.RequestOwnership();
    }

    public void OnSelectEntered()
    {
        Debug.Log("Grabbed");

        // it will send RPC to everyone in the room and also to player who joined later
        _photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered); 

        if(_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("We do not request ownership. Already mine");
        }
        else
        {
            TransferOwnership();
        }

    }

    public void OnSelectedExited()
    {
        Debug.Log("Released");

        _photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if(targetView != _photonView)
        {
            return;
        }

        Debug.Log("Ownership Requested for " + targetView.name + " from " + requestingPlayer.NickName);
        _photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Ownership Transfered to " + targetView.name + " from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        
    }

    [PunRPC]
    public void StartNetworkGabbing()
    {
        _isBeingHeld = true;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        _isBeingHeld = false;
    }

}
