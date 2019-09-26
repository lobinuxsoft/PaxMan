using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleSystem))]

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

    protected Rigidbody2D mRigidbody2D;
    protected ParticleSystem mParticle;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();
        mParticle = GetComponent<ParticleSystem>();
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

    protected virtual void MoveToDirection(Vector2 direction)
    {
        if (mRigidbody2D)
        {
            mRigidbody2D.velocity = direction;
        }
    }

    public virtual void Respawn(Vector3 respawnLocation, Vector3Int tileLocation)
    {
        if (mParticle)
            mParticle.Emit(50);

        if(mRigidbody2D)
            mRigidbody2D.velocity = Vector2.zero;

        SetPosition(respawnLocation);

        currentTileX = tileLocation.x;
        currentTileY = tileLocation.y;
        nextTileX = currentTileX;
        nextTileY = currentTileY;
    }
}