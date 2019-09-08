using UnityEngine;

public class PacMan : MobileEntity
{
    Vector2 destination = Vector2.zero;
    Vector3 direction = Vector3.zero;
    // Update is called once per frame
    void FixedUpdate()
    {

        destination = Game.instance.Map.GetWorldPosFromTile(new Vector3Int(nextTileX, nextTileY, 0));
        if (destination == Vector2.zero)
            return;

        direction = new Vector3(destination.x - transform.position.x, destination.y - transform.position.y);

        float distanceToMove = Time.fixedDeltaTime * 100.0f;

        if (direction.magnitude > .25f)
        {
            SetPosition(transform.position + direction * Time.fixedDeltaTime * 100.0f);
            currentTileX = nextTileX;
            currentTileY = nextTileY;
        }
        //else
        //{
        //    Vector2 position = new Vector2(transform.position.x, transform.position.y);
        //    direction.Normalize();
        //    SetPosition(position + direction * distanceToMove);
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, destination);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.transform.position, direction);
    }

}