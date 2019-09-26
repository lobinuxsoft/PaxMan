using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// It contains all the necessary data so that the 'Map' Class can generate a map in the level.
/// </summary>

[CreateAssetMenu(fileName = "New MapData", menuName = "Map/MapData")]
public class MapData : ScriptableObject
{
    [Tooltip("This list is automatically loaded as long as there are files of type '.txt' in the 'Assets/Data' folder.")]
    public string[] mapsTxtFilePath;
    [Tooltip("It contains all types of tiles that can be used in map generation, so the levels are different in each load.")]
    public List<TileBase> wallTiles;
    public StaticEntity smallDot;
    public StaticEntity bigDot;
    public List<StaticEntity> fruits;
    public StaticEntity teleport;

    private void Awake()
    {
        string dir = Application.dataPath + "/Data/";
        if (Directory.Exists(dir))
        {
            mapsTxtFilePath = Directory.GetFiles(dir, "*.txt", SearchOption.AllDirectories);
        }
    }
}
