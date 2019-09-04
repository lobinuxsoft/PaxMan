using UnityEngine;

public class Cherry : StaticEntity
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollectionEvent.Invoke();
        }
    }
}
