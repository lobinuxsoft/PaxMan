using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MobileEntity : Entity
{
    public enum MovementDirection : int
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        DirectionCount = 4
    };

    protected int currentTileX;
    protected int currentTileY;
    protected int nextTileX;
    protected int nextTileY;
    protected Rigidbody2D rigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        nextTileX = currentTileX;
        nextTileY = currentTileY;
    }

    public bool IsAtDestination()
    {
        if (currentTileX == nextTileX && currentTileY == nextTileY)
        {
            return true;
        }

        return false;
    }

    public void SetNextTile(Vector2Int nextPos)
    {
        nextTileX = nextPos.x;
        nextTileY = nextPos.y;
    }

    public int GetCurrentTileX()
    {
        return currentTileX;
    }

    public int GetCurrentTileY()
    {
        return currentTileY;
    }

    public void SetPosition(Vector2Int newPos)
    {
        transform.position = new Vector2(currentTileX * 22, -currentTileY * 22);
    }

    public void Respawn(Vector2 respawnLocation)
    {
        SetPosition(respawnLocation);
        currentTileX = (int)respawnLocation.x / 22;
        currentTileY = (int)respawnLocation.y / 22;
        nextTileX = currentTileX;
        nextTileY = currentTileY;
    }
}