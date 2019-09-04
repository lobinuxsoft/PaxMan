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

    //[SerializeField] private List<SmallDot> smallDots = new List<SmallDot>();
    //[SerializeField] private List<BigDot> bigDots = new List<BigDot>();
    //[SerializeField] private List<Cherry> cherry = new List<Cherry>();

    int mapIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitPathmap();
        InitDots();
        InitBigDots();
    }

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

    public void InitBigDots()
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

    //TODO Evaluate this
    internal bool TileIsValid(int tileX, int tileY)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tileX == tiles[t].posX && tileY == tiles[t].posY && !tiles[t].blocking)
                return true;
        }
        return false;
    }

    //TODO Evaluate this
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

    //TODO Evaluate this
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

    //TODO Evaluate this
    public PathmapTile GetTile(int tileX, int tileY)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tileX == tiles[t].posX && tileY == tiles[t].posY)
                return tiles[t];
        }

        return null;
    }

    //TODO modify this methods for new colision implementation
    //public bool HasIntersectedDot(Vector2 aPosition)
    //{
    //    for (int d = 0; d < smallDots.Count; d++)
    //    {
    //        if ((smallDots[d].GetPosition() - aPosition).magnitude < 5.0f)
    //        {
    //            GameObject.DestroyImmediate(smallDots[d]);
    //            smallDots.Remove(smallDots[d]);
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public bool HasIntersectedBigDot(Vector2 aPosition)
    //{
    //    for (int d = 0; d < bigDots.Count; d++)
    //    {
    //        if ((bigDots[d].GetPosition() - aPosition).magnitude < 5.0f)
    //        {
    //            GameObject.DestroyImmediate(bigDots[d]);
    //            bigDots.Remove(bigDots[d]);
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //bool HasIntersectedCherry(Vector2 aPosition)
    //{
    //    return true;
    //}
}

