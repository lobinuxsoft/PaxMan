using System;
using UnityEngine;

public class PacMan : MobileEntity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int tileSize = 22;

        Vector2 destination = new Vector2(nextTileX * tileSize, nextTileY * tileSize);
        if (destination == Vector2.zero)
            return;

        Vector2 direction = new Vector2(destination.x - transform.position.x, destination.y - transform.position.y);

        float distanceToMove = Time.deltaTime * 100.0f;

        if (distanceToMove > direction.magnitude)
        {
            transform.position = destination;
            currentTileX = nextTileX;
            currentTileY = nextTileY;
        }
        else
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            direction.Normalize();
            SetPosition(position + direction * distanceToMove);
        }
    }

}