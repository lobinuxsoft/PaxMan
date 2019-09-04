using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class StaticEntity : Entity
{
    protected UnityEvent OnCollectionEvent;
    public void AddOnCollectionEvent(UnityAction method) { OnCollectionEvent.AddListener(method); }
}
