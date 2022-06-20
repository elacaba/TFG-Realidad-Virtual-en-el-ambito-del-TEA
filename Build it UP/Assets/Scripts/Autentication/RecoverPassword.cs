using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
public class RecoverPassword : MonoBehaviour
{
    public Text messageText;
    public InputField email;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecoverPasswordFunc()
    {
        if (email.text != "")
        {
            var request = new SendAccountRecoveryEmailRequest
            {
                Email = email.text,
                TitleId = "38C11"
            };
            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
        }
        else
        {
            messageText.text = "Por favor, introduce tu email en la casilla de email";
        }
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset email sent!";
    }
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
}
