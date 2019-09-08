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

    [SerializeField] protected int currentTileX;
    [SerializeField] protected int currentTileY;
    [SerializeField] protected int nextTileX;
    [SerializeField] protected int nextTileY;
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

    public override void SetPosition(Vector3 position)
    {
        if (rigidbody2D)
        {
            position.x = position.x + .5f;
            position.y = position.y + .5f;

            rigidbody2D.MovePosition(position);
        }
    }

    public void Respawn(Vector3 respawnLocation, Vector3Int tileLocation)
    {
        SetPosition(respawnLocation);

        currentTileX = tileLocation.x;
        currentTileY = tileLocation.y;
        nextTileX = currentTileX;
        nextTileY = currentTileY;
    }
}