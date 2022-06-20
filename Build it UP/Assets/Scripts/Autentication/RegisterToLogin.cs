using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RegisterToLogin : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI")]
    EventSystem system;
    public Selectable firstInput;
    public Text messageText;
    public TMP_InputField username;
    public TMP_InputField password;
    public Animator transicion;
    public string sceneName;
    private float transitionTime = 1f;
    void Start()
    {
        StartCoroutine(LoadLevel(sceneName));
    }
    public void RegisterButton(string sceneName)
    {
        if (password.text.Length < 6)
        {
            messageText.text = "Password too short";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = username.text,
            Username = username.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);

    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Register success!";
        Debug.Log("Successful register");
        StartCoroutine(LoadLevel(sceneName));

    }
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log("aaaa" + error.GenerateErrorReport());
        Debug.Log("que pasa" + error);
    }


    IEnumerator LoadLevel(string sceneName)
    {
        transicion.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
