using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject SmallDotPrefab;
    public GameObject LargeDotPrefab;

    public int dotCount = 0;
    public int DotCount { get { return dotCount; } }

    public List<SmallDot> smallDots = new List<SmallDot>();
    public List<BigDot> bigDots = new List<BigDot>();
    public List<PathmapTile> tiles = new List<PathmapTile>();
    public List<Cherry> cherry = new List<Cherry>();

    // Start is called before the first frame update
    void Start()
    {
        InitPathmap();
        InitDots();
        initBigDots();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool InitPathmap()
    {
        string[] lines = System.IO.File.ReadAllLines("Assets/Data/map.txt");
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                PathmapTile tile = new PathmapTile();
                tile.posX = x;
                tile.posY = y;
                tile.blocking = line[x] == 'x';
                tiles.Add(tile);
            }
        }
        return true;
    }

    public bool InitDots()
    {
        string[] lines = System.IO.File.ReadAllLines("Assets/Data/map.txt");
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '.')
                {
                    SmallDot dot = GameObject.Instantiate(SmallDotPrefab).GetComponent<SmallDot>();
                    dot.SetPosition(new Vector2((x - (line.Length / 2)) * 22 + 11, (-y + (lines.Length / 2)) * 22));
                    dotCount++;
                }
            }
        }
        return true;
    }

    public bool initBigDots()
    {
        string[] lines = System.IO.File.ReadAllLines("Assets/Data/map.txt");
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'o')
                {
                    BigDot dot = GameObject.Instantiate(LargeDotPrefab).GetComponent<BigDot>();
                    dot.SetPosition(new Vector2((x - (line.Length / 2)) * 22 + 11, (-y + (lines.Length / 2)) * 22));
                    dotCount++;
                }
            }
        }
        return true;
    }

    internal bool TileIsValid(int tileX, int tileY)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tileX == tiles[t].posX && tileY == tiles[t].posY && !tiles[t].blocking)
                return true;
        }
        return false;
    }

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

    public PathmapTile GetTile(int tileX, int tileY)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tileX == tiles[t].posX && tileY == tiles[t].posY)
                return tiles[t];
        }

        return null;
    }

    public bool HasIntersectedDot(Vector2 aPosition)
    {
        for (int d = 0; d < smallDots.Count; d++)
        {
            if ((smallDots[d].GetPosition() - aPosition).magnitude < 5.0f)
            {
                GameObject.DestroyImmediate(smallDots[d]);
                smallDots.Remove(smallDots[d]);
                return true;
            }
        }

        return false;
    }

    public bool HasIntersectedBigDot(Vector2 aPosition)
    {
        for (int d = 0; d < bigDots.Count; d++)
        {
            if ((bigDots[d].GetPosition() - aPosition).magnitude < 5.0f)
            {
                GameObject.DestroyImmediate(bigDots[d]);
                bigDots.Remove(bigDots[d]);
                return true;
            }
        }

        return false;
    }

    bool HasIntersectedCherry(Vector2 aPosition)
    {
        return true;
    }
}

