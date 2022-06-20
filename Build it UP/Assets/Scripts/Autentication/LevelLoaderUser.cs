using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class LevelLoaderUser : MonoBehaviour
{
    [Header("UI")]
    EventSystem system;
    public Selectable firstInput;
    public TMP_InputField email;
    public TMP_InputField password;
    public Text messageText;
    public Animator transicion;
    public float transitionTime = 1f;
    bool loginCorrecto = false;
    bool cargando = false;
    private string entityId = "";
    private string entityType = "";
    private string emailMonitor;
    // Start is called before the first frame update

    public static LevelLoaderUser instance { get; private set; }
    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
    }
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Found Data persistance manager in scene");
        instance = this;
    }
    public string EntityType
    {
        get { return entityType; }
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
            {
                Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                if (previous != null)
                {
                    previous.Select();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                if (next != null)
                {
                    next.Select();
                }
            }
        }
    }

    public void Login(string sceneName)
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = email.text,
            Password = password.text,
            TitleId = PlayFabSettings.TitleId,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }

        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnSuccess, OnError);
        if (request != null)
        {
            StartCoroutine(Esperar(sceneName));
        }
        else
            Debug.Log("Usuario no encontrado");
    }
    IEnumerator Esperar(string sceneName)
    {
        yield return new WaitUntil(() => cargando is true);
        if (loginCorrecto)
            StartCoroutine(LoadLevel(sceneName));
    }
    IEnumerator LoadLevel(string sceneName)
    {
        transicion.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    void OnSuccess(LoginResult result)
    {
        messageText.text = "Logged in!";
        Debug.Log("Successful logging in");
        loginCorrecto = true;
        cargando = true;
        entityId = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;
        emailMonitor = email.text;


    }

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
}
