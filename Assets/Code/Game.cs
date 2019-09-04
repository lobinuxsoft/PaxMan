using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    public GameObject PacManPrefab;
    public GameObject GhostPrefab;

    public Map Map;
    public PacMan Avatar;
    public Vector2Int nextMovement;
    public List<Ghost> Ghosts;
    public int lives;
    public int score;
    public float myGhostGhostCounter = 20f;

    private float lastClaimableOn = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Avatar = Instantiate(PacManPrefab).GetComponent<PacMan>();
        Avatar.SetPosition(new Vector2Int(13, 16));
        lives = 3;
        Ghosts = new List<Ghost>();
        for(int g = 0; g < 4; g++)
        {
            Ghosts.Add(Instantiate(GhostPrefab).GetComponent<Ghost>());
            Ghosts[g].SetPosition(new Vector2Int(13, 13));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!HandleInput())
        {
            Application.Quit();
            return;
        }

        if (Map.DotCount == 0)
        {
            return;
        }
        else if (lives <= 0)
        {
            return;
        }

        MoveAatar();
        for (int g = 0; g < Ghosts.Count; g++)
        {
            Ghosts[g].onUpdate(Map, Avatar);
        }

        if ((Time.time - lastClaimableOn) > myGhostGhostCounter)
        {
            for (int g = 0; g < Ghosts.Count; g++)
            {
                Ghosts[g].isClaimable = false;
            }
        }

        //TODO Evaluate this lines
        for (int g = 0; g < Ghosts.Count; g++)
        {
            if ((Ghosts[g].GetPosition() - Avatar.GetPosition()).magnitude < 16.0f)
            {
                if (myGhostGhostCounter <= 0.0f)
                {
                    UpdateLives(lives - 1);

                    if(lives > 0)
                    {
                        Avatar.Respawn(new Vector2(13 * 22, 16 * 22));
                        Ghosts[g].Respawn(new Vector2(13 * 22, 13 * 22));
                        break;
                    }
                    else
                    {
                        GameOver();
                        return;
                    }
                }
                else
                {
                    UpdateScore(50);
                    Ghosts[g].isDead = true;
                    Ghosts[g].Die(Map);
                }
            }
        }
    }

    private void GameOver()
    {
        throw new NotImplementedException();
    }

    private void UpdateLives(int v)
    {
        lives += v;
    }

    private void UpdateScore(int scoreGain)
    {
        score += scoreGain;
    }

    public bool HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            nextMovement = new Vector2Int(0, 1);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            nextMovement = new Vector2Int(0, -1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            nextMovement = new Vector2Int(1, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            nextMovement = new Vector2Int(-1, 0);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            return false;
        }

        return true;
    }

    public void MoveAatar()
    {
        int nextTileX = Avatar.GetCurrentTileX() + nextMovement.x;
        int nextTileY = Avatar.GetCurrentTileY() + nextMovement.y;

        if (Avatar.IsAtDestination())
        {
            if (Map.TileIsValid(nextTileX, nextTileY))
            {
                Avatar.SetNextTile(new Vector2Int(nextTileX, nextTileY));
            }
        }
    }

    public void CollectSmallDot()
    {
        UpdateScore(10);
    }

    public void CollectBigDot()
    {
        UpdateScore(20);
        lastClaimableOn = Time.time;
        for (int g = 0; g < Ghosts.Count; g++)
        {
            Ghosts[g].isClaimable = true;
        }
    }
}