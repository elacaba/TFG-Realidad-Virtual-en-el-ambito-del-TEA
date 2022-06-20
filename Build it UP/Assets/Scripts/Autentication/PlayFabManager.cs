using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    [Header("UI")]
    EventSystem system;
    public Selectable firstInput;
    public Text messageText;
    public InputField email;
    public InputField username;
    public InputField password;
    public Animator transicion;
    public float transitionTime = 1f;
    bool registroCorrecto = false;
    bool cargando = false;
    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
    }
    void Update()
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
            Email = email.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        if (request != null)
        {
            StartCoroutine(Esperar(sceneName));

        }

    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Register success!";
        Debug.Log("Successful logging in");
        registroCorrecto = true;
        print("register correcto");
        cargando = true;

    }

        IEnumerator Esperar(string sceneName)
    {
        yield return new WaitUntil(() => cargando is true);
        if (registroCorrecto)
            StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transicion.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

// Update is called once per frame
/*public void Login(string sceneName)
{
    var request = new LoginWithEmailAddressRequest
    {
        Email = email.text,
        Password = password.text

    };
    PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccess, OnError);
    if (loginCorrecto == true)
    {
        StartCoroutine(LoadLevel(sceneName));
    }
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
    Debug.Log("Successful logging in/creating account");
    loginCorrecto = true;
}
*/
void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

    public void RecoverPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email.text,
            TitleId = "38C11"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset email sent!";
    }
}
