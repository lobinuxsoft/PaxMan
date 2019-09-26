using UnityEngine;

public class Fruit : StaticEntity
{
    [SerializeField] int fruitScore = 100;

    public int GetFruitScore()
    {
        return fruitScore;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.OnIntecractEvent.Invoke();
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        this.OnIntecractEvent.RemoveAllListeners();
    }
}
