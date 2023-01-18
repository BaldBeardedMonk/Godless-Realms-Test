using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class UserDataManager : MonoBehaviour
{
    public TextMeshProUGUI userID, world, tokens, gems, coins;
    public WorldPanelScript worldPanelScript;

    public void SetUSerData()
    {
        userID.text = "UserId: " +GlobalVariables.userID;
        tokens.text = "Tokens: " + GlobalVariables.tokens.ToString();
        gems.text = "Gems: " + GlobalVariables.gems.ToString();
        coins.text = "Coins: " + GlobalVariables.coins.ToString();
        world.text = "World: " + GlobalVariables.currentWorld.ToString();
    }

    public void GetPlayfabID()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, Successs, fail);
    }
    void Successs(GetAccountInfoResult result)
    {
        GlobalVariables.playfabId = result.AccountInfo.PlayFabId;
        Debug.Log(GlobalVariables.playfabId);
        GetCurrentWorld();
    }


    void fail(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void GetCurrentWorld()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = GlobalVariables.playfabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("World"))
            {
                Debug.Log("No World Data present");
                SetInitialProgressionState();
            }
            else
            {
                Debug.Log("World: " + result.Data["World"].Value);
                GlobalVariables.currentWorld = int.Parse(result.Data["World"].Value);
                GetCurrentLevel();

            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    void SetInitialProgressionState()
    {
        GlobalVariables.currentWorld = 1;
        GlobalVariables.currentLevel = 1;
        SetUSerData();
        EnableWorlds();
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {"World", "1"},
            {"Level", "1"}
        }
        },
    result => Debug.Log("Successfully updated user data"),

    error => {
        Debug.Log("Got error setting user data Ancestor to Arthur");
        Debug.Log(error.GenerateErrorReport());
    });
    }

    void GetCurrentLevel()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = GlobalVariables.playfabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("Level"))
            {
                Debug.Log("No Level Data present");
            }
            else
            {
                Debug.Log("Level: " + result.Data["Level"].Value);
                GlobalVariables.currentLevel = int.Parse(result.Data["Level"].Value);
                SetUSerData();
                EnableWorlds();
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void EnableWorlds()
    {
        worldPanelScript.EnableWorlds();
    }
}
