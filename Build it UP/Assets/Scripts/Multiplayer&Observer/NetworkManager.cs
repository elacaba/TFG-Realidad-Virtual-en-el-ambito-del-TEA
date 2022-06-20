using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private string room = null;

    void Start()
    {
        ConnectToServer();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Trying to connect to server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        room = "Room1";
        PhotonNetwork.JoinOrCreateRoom(room, roomOptions, TypedLobby.Default);

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected with reason {0}", cause);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("You joined a room!");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}