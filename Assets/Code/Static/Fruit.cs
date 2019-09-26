using UnityEngine;

public class Fruit : StaticEntity
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollectionEvent.Invoke();
            Destroy(this.gameObject);
        }
    }
}
