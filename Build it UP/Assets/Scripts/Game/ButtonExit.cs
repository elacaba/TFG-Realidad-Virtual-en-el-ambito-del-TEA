using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ButtonExit : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName;
    public float transitionTime = 1f;
    public static string emailMonitor;
    void Start()
    {

    }
    IEnumerator LoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goBack()
    {
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                try
                {
                    if (PhotonNetwork.PlayerList[i].CustomProperties["Name"].ToString() == "Monitor")
                    {
                        emailMonitor = PhotonNetwork.PlayerList[i].CustomProperties["Email"].ToString();
                    }
                }
                catch (NullReferenceException e)
                {
                    Debug.Log("No tiene propiedades");
                }
            }
        }
        DataPersistenceManager.instance.OnApplicationQuit();
        StartCoroutine(LoadLevel(sceneName));
    }
}
