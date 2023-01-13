using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class WorldManager : MonoBehaviour
{
    public WorldData worldData;
    public TextMeshProUGUI worldName;
    int levelCount;
    GameObject LevelPanelToInstantiate;
    public GameObject WorldPanel;

    [System.Serializable]
    public class WorldDataFromJson
    {
        public int Id;
        public string Name;
        public int Levels;
    }

    public void GetWorldDataFromPlayfab(string key)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                if (result.Data == null || !result.Data.ContainsKey(key)) Debug.LogWarning("No World data present");
                else
                {
                    string json = result.Data[key];
                    WorldDataFromJson worldDataFromJson = JsonUtility.FromJson<WorldDataFromJson>(json);
                    worldData.id = worldDataFromJson.Id;
                    worldData.world_name = worldDataFromJson.Name;
                    worldData.levelCount = worldDataFromJson.Levels;
                    PopulateWorldData();
                }
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            });
    }

    void PopulateWorldData()
    {
        worldName.text = worldData.world_name;
        levelCount = worldData.levelCount;
    }

    public void OnWorldClicked()
    {
        GlobalVariables.selectedWorld = worldData.id;
        Debug.Log("World data id is " + worldData.id);
        GlobalVariables.levelCount = levelCount;
        LevelPanelToInstantiate = Instantiate(worldData.levelMap, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        WorldPanel.SetActive(false);
       
    }
}
