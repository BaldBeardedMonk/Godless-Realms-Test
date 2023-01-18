using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LevelManager : MonoBehaviour
{
    GameObject[] LevelButtons;
  
    private void Awake()
    {
        LevelButtons = new GameObject[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            LevelButtons[i] = transform.GetChild(i).gameObject;
        }
    }

    private void OnEnable()
    {
        foreach (GameObject G in LevelButtons)
        {
            G.GetComponent<Button>().interactable = false;
        }
        SetLevels();
    }

    void SetLevels()
    {
        if (GlobalVariables.selectedWorld<GlobalVariables.currentWorld)
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
        Debug.Log("Level count is" + GlobalVariables.levelCount);
        if(levelNo==GlobalVariables.currentLevel)
        {
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
            Debug.LogWarning("Reward already given");
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
