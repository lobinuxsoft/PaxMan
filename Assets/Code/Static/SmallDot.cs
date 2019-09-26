﻿using UnityEngine;

public class SmallDot : StaticEntity
{
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
