using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [Tooltip("The base tile map for generate in runtime an obtain data")]
    [SerializeField] private Tilemap levelMap;
    [SerializeField] private MapData mapData;

    [SerializeField] private int dotCount = 0;
    public int DotCount { get { return dotCount; } }

    [SerializeField] private List<PathmapTile> tiles = new List<PathmapTile>();

    [SerializeField] private List<Vector3> fruitSpwanPosList = new List<Vector3>();
    [SerializeField] private List<Vector3> ghostSpawnPosList = new List<Vector3>();
    private Vector3 playerSpawnPos = Vector3.zero;

    int mapIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitPathmap();
        InitPlayerSpawnPos();
        InitGhostSpawnPos();
        InitFruitSpawnPos();
        InitDots();
        InitBigDots();
    }

    /// <summary>
    /// Initializes the construction of the map, where you can pass and where there are walls, and tells the TileMap what to draw.
    /// </summary>
    private void InitPathmap()
    {
        //Initialize random index for mapData.mapsTxtFilePath mapData.wallTile
        Random.InitState(Mathf.RoundToInt(Time.unscaledTime));
        mapIndex = Random.Range(0, mapData.mapsTxtFilePath.Length);
        int tileIndex = Random.Range(0,mapData.wallTiles.Count);

        string[] lines = System.IO.File.ReadAllLines(mapData.mapsTxtFilePath[mapIndex]);
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                PathmapTile tile = new PathmapTile
                {
                    posX = x - (line.Length / 2),
                    posY = -(y - (lines.Length / 2)),
                    blocking = line[x] == 'x'
                };

                //Here create a graphic map
                Vector3Int tilePos = new Vector3Int(tile.posX, tile.posY, 0);

                if (tile.blocking)
                {
                    if (levelMap)
                    {
                        levelMap.SetTile(tilePos, mapData.wallTiles[tileIndex]);
                    }
                }
                else
                {
                    if (levelMap)
                    {
                        levelMap.SetTile(tilePos, null);
                    }
                }

                tiles.Add(tile);
            }
        }
    }

    /// <summary>
    /// Instance the SmallDot in the scene.
    /// </summary>
    private void InitDots()
    {
        string[] lines = System.IO.File.ReadAllLines(mapData.mapsTxtFilePath[mapIndex]);
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '.')
                {
                    Vector3Int tilePos = Vector3Int.zero;
                    tilePos.x = x - (line.Length / 2);
                    tilePos.y = -(y - (lines.Length / 2));

                    Vector3 worldPos = levelMap.CellToWorld(tilePos);
                    worldPos.x += .5f;
                    worldPos.y += .5f;

                    SmallDot dot = Instantiate(mapData.smallDot, levelMap.transform).GetComponent<SmallDot>();
                    dot.name = string.Format("SmallDot X= {0:0} Y= {1:0}", tilePos.x, tilePos.y);
                    dot.SetPosition(worldPos);
                    dotCount++;
                }
            }
        }
    }

    /// <summary>
    /// Instance the BigDot in the scene.
    /// </summary>
    private void InitBigDots()
    {
        string[] lines = System.IO.File.ReadAllLines(mapData.mapsTxtFilePath[mapIndex]);
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'o')
                {
                    Vector3Int tilePos = Vector3Int.zero;
                    tilePos.x = x - (line.Length / 2);
                    tilePos.y = -(y - (lines.Length / 2));

                    Vector3 worldPos = levelMap.CellToWorld(tilePos);
                    worldPos.x += .5f;
                    worldPos.y += .5f;

                    BigDot dot = Instantiate(mapData.bigDot).GetComponent<BigDot>();
                    dot.SetPosition(worldPos);
                    dotCount++;
                }
            }
        }
    }

    /// <summary>
    /// Initialize a list of valid positions to instantiate a fruit.
    /// </summary>
    public void InitFruitSpawnPos()
    {
        string[] lines = System.IO.File.ReadAllLines(mapData.mapsTxtFilePath[mapIndex]);
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'o' || line[x] == '.' || line[x] == ' ')
                {
                    Vector3Int tilePos = Vector3Int.zero;
                    tilePos.x = x - (line.Length / 2);
                    tilePos.y = -(y - (lines.Length / 2));

                    Vector3 worldPos = levelMap.CellToWorld(tilePos);
                    worldPos.x += .5f;
                    worldPos.y += .5f;

                    fruitSpwanPosList.Add(worldPos);
                }
            }
        }
    }

    /// <summary>
    /// Initialize a list of valid positions to instantiate a ghost.
    /// </summary>
    public void InitGhostSpawnPos()
    {
        string[] lines = System.IO.File.ReadAllLines(mapData.mapsTxtFilePath[mapIndex]);
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'g')
                {
                    Vector3Int tilePos = Vector3Int.zero;
                    tilePos.x = x - (line.Length / 2);
                    tilePos.y = -(y - (lines.Length / 2));

                    Vector3 worldPos = levelMap.CellToWorld(tilePos);
                    worldPos.x += .5f;
                    worldPos.y += .5f;

                    ghostSpawnPosList.Add(worldPos);
                }
            }
        }
    }

    /// <summary>
    /// Initialize a valid position to instantiate a player.
    /// </summary>
    public void InitPlayerSpawnPos()
    {
        string[] lines = System.IO.File.ReadAllLines(mapData.mapsTxtFilePath[mapIndex]);
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'p')
                {
                    Vector3Int tilePos = Vector3Int.zero;
                    tilePos.x = x - (line.Length / 2);
                    tilePos.y = -(y - (lines.Length / 2));

                    Vector3 worldPos = levelMap.CellToWorld(tilePos);
                    worldPos.x += .5f;
                    worldPos.y += .5f;

                    playerSpawnPos = worldPos;
                }
            }
        }
    }

    /// <summary>
    /// Return is a valid position in the grid.
    /// </summary>
    /// <param name="tileX"></param>
    /// <param name="tileY"></param>
    /// <returns></returns>
    internal bool TileIsValid(int tileX, int tileY)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tileX == tiles[t].posX && tileY == tiles[t].posY && !tiles[t].blocking)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Return a path from the curren tile to target tile
    /// </summary>
    /// <param name="currentTileX"></param>
    /// <param name="currentTileY"></param>
    /// <param name="targetX"></param>
    /// <param name="targetY"></param>
    /// <returns></returns>
    public List<PathmapTile> GetPath(int currentTileX, int currentTileY, int targetX, int targetY)
    {
        PathmapTile fromTile = GetTile(currentTileX, currentTileY);
        PathmapTile toTile = GetTile(targetX, targetY);

        for (int t = 0; t < tiles.Count; t++)
        {
            tiles[t].visited = false;
        }

        List<PathmapTile> path = new List<PathmapTile>();
        if (Pathfind(fromTile, toTile, path))
        {
            return path;
        }
        return null;
    }

    /// <summary>
    /// Pathfind calculation.
    /// </summary>
    /// <param name="fromTile"></param>
    /// <param name="toTile"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private bool Pathfind(PathmapTile fromTile, PathmapTile toTile, List<PathmapTile> path)
    {
        fromTile.visited = true;

        if (fromTile.blocking)
            return false;
        path.Add(fromTile);
        if (fromTile == toTile)
            return true;

        List<PathmapTile> neighbours = new List<PathmapTile>();

        PathmapTile up = GetTile(fromTile.posX, fromTile.posY - 1);
        if (up != null && !up.visited && !up.blocking && !path.Contains(up))
            neighbours.Insert(0, up);

        PathmapTile down = GetTile(fromTile.posX, fromTile.posY + 1);
        if (down != null && !down.visited && !down.blocking && !path.Contains(down))
            neighbours.Insert(0, down);

        PathmapTile right = GetTile(fromTile.posX + 1, fromTile.posY);
        if (right != null && !right.visited && !right.blocking && !path.Contains(right))
            neighbours.Insert(0, right);

        PathmapTile left = GetTile(fromTile.posX - 1, fromTile.posY);
        if (left != null && !left.visited && !left.blocking && !path.Contains(left))
            neighbours.Insert(0, left);

        for(int n = 0; n < neighbours.Count; n++)
        {
            PathmapTile tile = neighbours[n];

            path.Add(tile);

            if (Pathfind(tile, toTile, path))
                return true;

            path.Remove(tile);
        }

        return false;
    }

    /// <summary>
    /// Return a PathTile from the list of valid tiles
    /// </summary>
    /// <param name="tileX"></param>
    /// <param name="tileY"></param>
    /// <returns></returns>
    public PathmapTile GetTile(int tileX, int tileY)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tileX == tiles[t].posX && tileY == tiles[t].posY)
                return tiles[t];
        }

        return null;
    }

    /// <summary>
    /// Return Tile position from world position
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector3Int GetTileFromWorldPos(Vector3 worldPos)
    {
        return levelMap.WorldToCell(worldPos);
    }

    
    public Vector3 GetWorldPosFromTile(Vector3Int tilePos)
    {
        return levelMap.CellToWorld(tilePos);
    }

    /// <summary>
    /// Instance a fruit in a valid random position.
    /// </summary>
    /// <param name="level"></param>
    public void SpawnFruit(int level = 0)
    {
        Random.InitState(Mathf.RoundToInt(Time.unscaledTime));

        int indexFruit = 0;

        switch (level)
        {
            case 1:
                indexFruit = 0;
                break;
            case 2:
                indexFruit = 1;
                break;
            case 3:
            case 4:
                indexFruit = 2;
                break;
            case 5:
            case 6:
                indexFruit = 3;
                break;
            case 7:
            case 8:
                indexFruit = 4;
                break;
            case 9:
            case 10:
                indexFruit = 5;
                break;
            case 11:
            case 12:
                indexFruit = 6;
                break;
            default:
                indexFruit = 7;
                break;
        }

        Fruit fruit = (Fruit)Instantiate(
                mapData.fruits[indexFruit],
                fruitSpwanPosList[Random.Range(0, fruitSpwanPosList.Count)],
                Quaternion.identity,
                levelMap.transform
            );
    }

    /// <summary>
    /// Return a ghost spawn position
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetGhostSpawnPos()
    {
        return ghostSpawnPosList;
    }

    /// <summary>
    /// Return a player spawn position
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerSpawnPos()
    {
        return playerSpawnPos;
    }
}

