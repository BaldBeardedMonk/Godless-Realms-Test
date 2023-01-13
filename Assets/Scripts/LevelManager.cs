using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LevelManager : MonoBehaviour
{
    GameObject[] LevelButtons;
    public GameObject RewardPanel;

    private void OnEnable()
    {
        LevelButtons = GameObject.FindGameObjectsWithTag("Levels");
        foreach (GameObject G in LevelButtons)
        {
            G.GetComponent<Button>().interactable = false;
        }
        SetLevels();
    }

    void SetLevels()
    {
        Debug.Log("Selected world is" + GlobalVariables.selectedWorld);
        Debug.Log("Current world is " + GlobalVariables.currentWorld);
        if(GlobalVariables.selectedWorld<GlobalVariables.currentWorld)
        {
            foreach (GameObject G in LevelButtons)
            {
                G.GetComponent<Button>().interactable = true;
                G.GetComponent<Button>().image.color = Color.blue;
            }
        }
        else
        {
            for(int i =0;i<GlobalVariables.currentLevel;i++)
            {
                LevelButtons[i].GetComponent<Button>().interactable = true;
                if(i>0)
                {
                    LevelButtons[i-1].GetComponent<Button>().image.color = Color.blue;
                }
            }
        }
    }

    public void OnLevelButtonClick(int levelNo)
    {
        if(levelNo==GlobalVariables.currentLevel)
        {
            //RewardPanel.SetActive(true);
            Debug.Log("RewardGiven");
            GlobalVariables.currentLevel++;
            if (GlobalVariables.currentLevel > GlobalVariables.levelCount)
            {
                if (GlobalVariables.currentWorld + 1 <= GlobalVariables.worldCount)
                {
                    GlobalVariables.currentWorld++;
                    GlobalVariables.currentLevel = 1;
                }
            }

            UploadUserDataToPlayfab();
            SetLevels();
        }
        else
        {
            Debug.Log("Reward already given");
        }
    }

    void UploadUserDataToPlayfab()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {"World", GlobalVariables.currentWorld.ToString()},
            {"Level", GlobalVariables.currentLevel.ToString()}
        }
        },
    result => Debug.Log("Successfully updated user data"),

    error => {
        Debug.Log("Got error setting user data Ancestor to Arthur");
        Debug.Log(error.GenerateErrorReport());
    });
    }
}
