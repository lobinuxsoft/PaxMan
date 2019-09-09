using UnityEngine;

public class PacMan : MobileEntity
{
    [SerializeField] float moveSpeed = 300f;
    [SerializeField] LayerMask collisionMoveLayer;
    Vector3 direction = Vector3.zero;
    bool isHit = false;

    Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (direction.magnitude < .1f)
            return;

        isHit = Physics2D.Raycast(this.transform.position, direction, .5f, collisionMoveLayer);
        if (!isHit)
        {
            MoveToDirection(direction * Time.fixedDeltaTime * moveSpeed);

            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);

            animator.speed = moveSpeed * .01f;

            currentTileX = nextTileX;
            currentTileY = nextTileY;
        }
        else
        {
            MoveToDirection(Vector2.zero);
            animator.speed = 0;
        }
    }

    public void SetDirectionToMove(Vector2Int value)
    {
        direction = new Vector3(value.x, value.y, 0f);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isHit)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawRay(this.transform.position, Vector3.ClampMagnitude(direction, .5f));
    }
#endif

}