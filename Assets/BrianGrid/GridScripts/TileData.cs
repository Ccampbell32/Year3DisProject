using UnityEngine;

[System.Serializable]
public class TileData
{
    public string tileName = "Default";
    public GameObject prefab;
    public int moveCost = 1;
    public bool isWalkable = true;
}
