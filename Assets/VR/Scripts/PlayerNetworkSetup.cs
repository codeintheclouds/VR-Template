using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject _localXRRigGameObject;

    [SerializeField]
    private GameObject _avatarHead, _avatarbody;


    void Start()
    {
        //This will make sure there is only one XR Rig in the Room
        //Every player will control their XR Rig locally

        if(photonView.IsMine)
        {
            //Player is Local
            _localXRRigGameObject.SetActive(true);

            SetLayerRecursively(_avatarHead, 6);
            SetLayerRecursively(_avatarbody, 7);

            TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<TeleportationArea>();
            if(teleportationAreas.Length > 0)
            {
                Debug.Log("Found " + teleportationAreas.Length + " teleportation area");

                foreach(var item in teleportationAreas)
                {
                    item.teleportationProvider = _localXRRigGameObject.GetComponent<TeleportationProvider>();
                }
            }

        }
        else
        {
            //Player is remote
            _localXRRigGameObject.SetActive(false);

            SetLayerRecursively(_avatarHead, 0);
            SetLayerRecursively(_avatarbody, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
