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
    public Vector2Int moveDirection;
    public List<Ghost> Ghosts;
    public int lives;
    public int score;
    public float myGhostGhostCounter = 20f;

    private float lastClaimableOn = 0;

    List<Vector3> ghostSpawnPos = new List<Vector3>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Avatar = Instantiate(PacManPrefab).GetComponent<PacMan>();
        Avatar.Respawn(Map.GetPlayerSpawnPos(), Map.GetTileFromWorldPos(Map.GetPlayerSpawnPos()));
        lives = 3;
        Ghosts = new List<Ghost>();
        ghostSpawnPos = Map.GetGhostSpawnPos();

        for (int g = 0; g < 4; g++)
        {
            Ghosts.Add(Instantiate(GhostPrefab).GetComponent<Ghost>());
            Ghosts[g].Respawn(ghostSpawnPos[g], Map.GetTileFromWorldPos(ghostSpawnPos[g]));
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
            //TODO victory
            return;
        }
        else if (lives <= 0)
        {
            //TODO Lose
            return;
        }

        MoveAvatar();
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
    }

    /// <summary>
    /// Set avatar damage and respawn, respawn ghost too.
    /// </summary>
    public void AvatarDamage()
    {
        UpdateLives(lives - 1);

        if (lives > 0)
        {
            Vector3 playerSpawnPos = Map.GetPlayerSpawnPos();

            Avatar.Respawn(playerSpawnPos, Map.GetTileFromWorldPos(playerSpawnPos));

            for (int i = 0; i < Ghosts.Count; i++)
            {
                Ghosts[i].Respawn(ghostSpawnPos[i], Map.GetTileFromWorldPos(ghostSpawnPos[i]));
            }
        }
        else
        {
            GameOver();
        }
    }

    /// <summary>
    /// Kill specific ghost
    /// </summary>
    /// <param name="ghostIndex"></param>
    public void KillGhost(int ghostIndex)
    {
        UpdateScore(50);
        Ghosts[ghostIndex].isDead = true;
        Ghosts[ghostIndex].Die(Map, ghostIndex);
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
        if (Input.GetAxis("Vertical") > 0)
        {
            moveDirection = new Vector2Int(0, 1);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            moveDirection = new Vector2Int(0, -1);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            moveDirection = new Vector2Int(1, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            moveDirection = new Vector2Int(-1, 0);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            return false;
        }

        return true;
    }

    public void MoveAvatar()
    {
        //int nextTileX = Avatar.GetCurrentTileX() + moveDirection.x;
        //int nextTileY = Avatar.GetCurrentTileY() + moveDirection.y;

        //if (Avatar.IsAtDestination())
        //{
        //    if (Map.TileIsValid(nextTileX, nextTileY))
        //    {
        //        Avatar.SetNextTile(new Vector2Int(nextTileX, nextTileY));
        //    }
        //}

        Avatar.SetDirectionToMove(moveDirection);
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

    public void CollectFruit(int score)
    {
        UpdateScore(score);
    }
}