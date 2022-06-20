using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    // Start is called before the first frame update

    string MyPlayfabID;
    public Text DisplayName = null;
    public static string nombre = null;
    public static string email = null;
    public static TextDisplay instance { get; private set; }

    void Start()
    {
       
       GetAccountInfoRequest request = new GetAccountInfoRequest();
       PlayFabClientAPI.GetAccountInfo(request, Success, fail);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Success(GetAccountInfoResult result)
    {
        email = result.AccountInfo.PrivateInfo.Email;
        MyPlayfabID = result.AccountInfo.PlayFabId;
        GetPlayerProfile(MyPlayfabID);

    }

    void GetPlayerProfile(string PlayFabID)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = PlayFabID,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result => { DisplayName.text = result.PlayerProfile.DisplayName; nombre = DisplayName.text; },
        error => Debug.LogError(error.GenerateErrorReport()));

    }


    void fail(PlayFabError error)
    {

        Debug.LogError(error.GenerateErrorReport());
    }

}
