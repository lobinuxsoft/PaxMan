using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class StaticEntity : Entity
{
    protected UnityEvent OnIntecractEvent = new UnityEvent();
    public void AddOnInteractEvent(UnityAction method) { OnIntecractEvent.AddListener(method); }
}
