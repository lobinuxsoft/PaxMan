using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class StaticEntity : Entity
{
    protected UnityEvent OnCollectionEvent = new UnityEvent();
    public void AddOnCollectionEvent(UnityAction method) { OnCollectionEvent.AddListener(method); }
}
