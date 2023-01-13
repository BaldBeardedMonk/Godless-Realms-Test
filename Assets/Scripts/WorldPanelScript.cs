using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class WorldPanelScript : MonoBehaviour
{
    public Transform WorldLayout;
    int count;
    public GameObject LoadingText;

    private void Start()
    {
        LoadingText.SetActive(true);
        count = 0;
        LoadWorldData();

    }
    void LoadWorldData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                if (result.Data == null || !result.Data.ContainsKey("WorldCount")) Debug.LogWarning("No World data present");
                else
                {
                    Debug.Log("World data is: " + result.Data["WorldCount"]);
                    count = int.Parse(result.Data["WorldCount"]);
                    GlobalVariables.worldCount = count;
                    ShowWorlds();
                }
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            });

    }

    void ShowWorlds()
    {
        LoadingText.SetActive(false);
        for(int i=0;i<count;i++)
        {
            WorldLayout.transform.GetChild(i).gameObject.SetActive(true);
            string key = "World" + (i + 1);
            WorldLayout.transform.GetChild(i).GetComponent<WorldManager>().GetWorldDataFromPlayfab(key);
        }
    }

    public void EnableWorlds()
    {
        for(int i=0;i<GlobalVariables.currentWorld;i++)
        {
            WorldLayout.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }
}
