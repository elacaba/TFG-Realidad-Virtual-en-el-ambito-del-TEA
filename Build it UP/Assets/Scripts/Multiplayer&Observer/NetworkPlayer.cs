using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NetworkPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform head;
    public Transform rightHand;
    public Transform leftHand;

    public GameObject avatar;
    private Animator anim;
    private Animator animSpawned;

    private Transform headRig;
    private Transform RHandRig;
    private Transform LHandRig;
    private Quaternion quat = new Quaternion(0, -1000, 180, 0);
    private Vector3 vector = new Vector3(0.08f,0.03f,-0.05f);

    private PhotonView photonview;
    [SerializeField]
    private string parentPath_vr = "";

    [SerializeField]
    private string alternativePath = "";

    void Start()
    {
        photonview = GetComponent<PhotonView>();
        var parent = GameObject.Find(photonview.IsMine ? parentPath_vr : alternativePath);
        headRig = parent.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
        RHandRig = parent.transform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");
        LHandRig = parent.transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor");

    }

    // Update is called once per frame
    void Update()
    {
        if (photonview.IsMine)
        {
            head.gameObject.SetActive(false);
            rightHand.gameObject.SetActive(false);
            leftHand.gameObject.SetActive(false);
            ParentObject(head, headRig);
            ParentObject(rightHand, RHandRig);
            ParentObject(leftHand, LHandRig);
            
        }

    }

    public void ParentObject(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position + vector;
        target.rotation = rigTransform.rotation * quat;
    }
}
