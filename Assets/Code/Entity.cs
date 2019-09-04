using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector2 GetPosition()
    {
        return transform.position / 22;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
