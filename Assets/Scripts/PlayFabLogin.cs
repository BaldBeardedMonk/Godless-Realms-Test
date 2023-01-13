using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;

public class PlayFabLogin : MonoBehaviour
{
    public TMP_InputField userId;
    public TextMeshProUGUI infoText;

    public GameObject WorldPanel,LoginPanel;

    public void OnSubmit()
    {
        infoText.text = "";
        if(string.IsNullOrEmpty(userId.text))
        {
            infoText.color = Color.red;
            infoText.text= "*Please enter a user id to proceed";
        }
        else
        {
            infoText.color = Color.green;
            infoText.text = "Please wait, logging in...";
            var request = new LoginWithCustomIDRequest { CustomId = userId.text, CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login success");
        GlobalVariables.userID = userId.text;
        infoText.text = "";
        WorldPanel.SetActive(true);
        LoginPanel.SetActive(false);
        GameObject.FindGameObjectWithTag("UserDataManager").GetComponent<UserDataManager>().GetPlayfabID();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        infoText.color = Color.red;
        infoText.text = "*Login failed. please try again";
        Debug.LogWarning("Error logging in");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

}
