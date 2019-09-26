using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : StaticEntity
{
    [SerializeField] Color ownColor;
    [SerializeField] Color conectionColor;

    [SerializeField] bool isExit = false;
    [SerializeField] Teleport teleportConect;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isExit == false)
        {
            teleportConect.MakeExit();
            collision.transform.position = teleportConect.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isExit)
            {
                isExit = false;
            }
        }
    }

    public void SetConection(Teleport teleport)
    {
        teleportConect = teleport;
    }

    public void MakeExit()
    {
        isExit = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = ownColor;
        Gizmos.DrawWireSphere(this.transform.position + (Vector3.up * .5f), .25f);

        Gizmos.color = new Color(ownColor.r, ownColor.g, ownColor.b, .5f);
        Gizmos.DrawSphere(this.transform.position + (Vector3.up * .5f), .25f);

        if (teleportConect)
        {
            Gizmos.color = conectionColor;
            Gizmos.DrawWireSphere(teleportConect.transform.position + (Vector3.down * .5f), .25f);

            Gizmos.color = new Color(conectionColor.r, conectionColor.g, conectionColor.b, .5f);
            Gizmos.DrawSphere(teleportConect.transform.position + (Vector3.down * .5f), .25f);

            Gizmos.DrawLine(this.transform.position + (Vector3.up * .5f), teleportConect.transform.position + (Vector3.down * .5f));
        }
    }
#endif
}