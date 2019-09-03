using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MobileEntity
{
    public enum Behaviour
    {
        Wander,
        Chase,
        Intercept,
        Fear,
    }

    public bool isClaimable;
    public bool isDead;

    public int desiredMovementX;
    public int desiredMovementY;

    public Behaviour myBehaviour;
    public List<PathmapTile> path = new List<PathmapTile>();

    // Start is called before the first frame update
    void Start()
    {
        isClaimable = false;
        isDead = false;

        desiredMovementX = 0;
        desiredMovementY = -1;
    }

    // Update is called once per frame
    public void onUpdate(Map map, PacMan avatar)
    {
        float speed = 30.0f;
        int nextTileX = currentTileX + desiredMovementX;
        int nextTileY = currentTileY + desiredMovementY;

        if (isDead)
            speed = 120.0f;

        if(IsAtDestination())
        {
            if(path.Count > 0)
            {
                PathmapTile nextTile = path[0];
                path.RemoveAt(0);
                SetNextTile(new Vector2Int(nextTile.posX, nextTile.posY));
            }
            else if(map.TileIsValid(nextTileX, nextTileY))
            {
                SetNextTile(new Vector2Int(nextTileX, nextTileY));
            }
            else
            {
                if(isClaimable)
                {
                    BehaveVulnerable();
                }
                else
                {
                    switch(myBehaviour)
                    {
                        case Behaviour.Chase:
                            BehaveChase(map, avatar);
                            break;
                        case Behaviour.Intercept:
                        case Behaviour.Wander:
                        default:
                            BehaveWander();
                            break;
                    }
                }

                isDead = false;
            }
        }

        int tileSize = 22;
        Vector2 destination = new Vector2(nextTileX * tileSize, nextTileY * tileSize);
        Vector2 direction = destination - GetPosition();

        float distanceToMove = Time.deltaTime * speed;

        if (distanceToMove > direction.magnitude)
        {
            SetPosition(destination);
            currentTileX = nextTileX;
            currentTileY = nextTileY;
        }
        else
        {
            direction.Normalize();
            SetPosition(GetPosition() + direction * distanceToMove);
        }
    }

    private void BehaveWander()
    {
        System.Random rng = new System.Random();
        MovementDirection nextDirection = (MovementDirection)(rng.Next((int)MovementDirection.DirectionCount));
        switch (nextDirection)
        {
            case MovementDirection.Up:
                desiredMovementX = 0;
                desiredMovementY = 1;
                break;
            case MovementDirection.Down:
                desiredMovementX = 0;
                desiredMovementY = -1;
                break;
            case MovementDirection.Left:
                desiredMovementX = -1;
                desiredMovementY = 0;
                break;
            case MovementDirection.Right:
                desiredMovementX = 1;
                desiredMovementY = 0;
                break;
            default:
                break;
        }
    }

    private void BehaveChase(Map map, PacMan avatar)
    {
        path.Clear();
        path = map.GetPath(currentTileX, currentTileY, avatar.currentTileX, avatar.currentTileY);
    }

    private void BehaveVulnerable()
    {
        throw new NotImplementedException();
    }

    public void Die(Map map)
    {
        path.Clear();
        path = map.GetPath(currentTileX, currentTileY, 13, 13);
    }
}
