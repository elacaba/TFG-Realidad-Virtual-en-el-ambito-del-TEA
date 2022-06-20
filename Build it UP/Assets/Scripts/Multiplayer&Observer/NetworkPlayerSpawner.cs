using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private GameObject SpawnedPlayerPrefab;
    private bool NoEsMonitor = false;
    private bool NoEsJugador = false;
    public GameObject cameraRig;
    public override void OnJoinedRoom()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log(player.GetType());
        }
        base.OnJoinedRoom();
        try {
            if (!StartGame.instance.esJugador)
                Debug.Log("Not instantiated");

        }
        catch (NullReferenceException e)
        {

            NoEsJugador = true;
        }
        try
        {
            if (!StartGameMonitor.instance.esMonitor)
                Debug.Log("Not instantiated");

        }
        catch (NullReferenceException e)
        {
            NoEsMonitor = true;
        }

        Debug.Log("No es jugador:" + NoEsJugador);
        Debug.Log("No es monitor: " + NoEsMonitor);

        if (!NoEsMonitor)
        {
           SpawnedPlayerPrefab = PhotonNetwork.Instantiate("Monitor", new Vector3(0, 0, 0), transform.rotation);
           SpawnedPlayerPrefab.transform.Find("Camera").gameObject.SetActive(true);
           cameraRig.SetActive(false);

           ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
           props.Add("Email", TextDisplay.email);
                //props.Add("Email", "eva.lacaba@estudiante.uam.es");
           props.Add("Name", "Monitor");
           PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else if (!NoEsJugador)
        {
            SpawnedPlayerPrefab = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -0.8f), transform.rotation);
            cameraRig.SetActive(true);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(SpawnedPlayerPrefab);
    }
}
