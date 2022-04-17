using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI _occupancyRateText_ForSchool;
    [SerializeField]
    private TextMeshProUGUI _occupancyRateText_ForOutdoor;
    private string _mapType;


    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if(!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }


    #region UI CallBack Methods

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnEnterButtonClicked_Outdoor()
    {

        _mapType = VRConstants.MAP_TYPE_VALUE_OUTDOOR;

        ExitGames.Client.Photon.Hashtable expectedCustomProperties = new ExitGames.Client.Photon.Hashtable()
        { {VRConstants.MAP_TYPE_KEY, _mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomProperties, 0);

    }

    public void OnEnterButtonClicked_School()
    {
        _mapType = VRConstants.MAP_TYPE_VALUE_SCHOOL;

        ExitGames.Client.Photon.Hashtable expectedCustomProperties = new ExitGames.Client.Photon.Hashtable()
        { {VRConstants.MAP_TYPE_KEY, _mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomProperties, 0);

    }

    #endregion


    #region Photon Callback Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateRandomRooms();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to servers again");
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("A room is created with name: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("The local player with name: " + PhotonNetwork.NickName + " has joined the room: " + PhotonNetwork.CurrentRoom.Name 
         + " and the player count is: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(VRConstants.MAP_TYPE_KEY))
        {
            object mapType;

            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(VRConstants.MAP_TYPE_KEY, out mapType))
            {
                Debug.Log("Joined room with map: " + (string)mapType);

                if((string)mapType == VRConstants.MAP_TYPE_VALUE_SCHOOL)
                {
                    PhotonNetwork.LoadLevel("World_School");
                }
                else if((string)mapType == VRConstants.MAP_TYPE_VALUE_OUTDOOR)
                {
                    PhotonNetwork.LoadLevel("World_Outdoor"); 
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined " + " Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList.Count == 0)
        {
            //There is no room at all
            _occupancyRateText_ForOutdoor.text = 0 + "/" + 20;
            _occupancyRateText_ForSchool.text = 0 + "/" + 20;
        }

        foreach(RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            
            if (room.Name.Contains(VRConstants.MAP_TYPE_VALUE_OUTDOOR))
            {
                Debug.Log("Room is outdoor with player count of: " + room.PlayerCount);
                _occupancyRateText_ForOutdoor.text = room.PlayerCount + "/" + 20;

            }

            else if (room.Name.Contains(VRConstants.MAP_TYPE_VALUE_SCHOOL))
            {
                Debug.Log("Room is school with player count of: " + room.PlayerCount);

                _occupancyRateText_ForSchool.text = room.PlayerCount + "/" + 20;

            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Room");
    }

    #endregion

    #region Private Methods

    void CreateRandomRooms()
    {
        string randomRoomName = "Room: " + _mapType +  Random.Range( 0, 1000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 18;

        string[] roomPropsInLobby = { VRConstants.MAP_TYPE_KEY };

        //We have two diff maps
        //1. Outdoor = "outdoor"
        //2. School = "school"

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { {VRConstants.MAP_TYPE_KEY, _mapType } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;


        PhotonNetwork.CreateRoom(randomRoomName, roomOptions) ;

    }

    #endregion


}
