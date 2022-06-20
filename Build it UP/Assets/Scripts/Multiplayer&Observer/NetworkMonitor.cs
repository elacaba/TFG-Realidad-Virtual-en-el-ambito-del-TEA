using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;

public class NetworkMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform head;
    public Transform rightHand;
    public Transform leftHand;
    private PhotonView photonview;
    public static GameObject LocalPlayerInstance;

    private string emailMon = null;
    void Start()
    {
        photonview = GetComponent<PhotonView>();
        NetworkMonitor.LocalPlayerInstance = this.gameObject;
    }


}
