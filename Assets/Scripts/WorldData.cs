using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "World Data")]
public class WorldData : ScriptableObject
{
    public int id;
    public string world_name;
    public int levelCount;
    public GameObject levelMap;
}