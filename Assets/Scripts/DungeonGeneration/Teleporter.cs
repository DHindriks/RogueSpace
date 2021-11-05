using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    public GameObject TpTo;

    [SerializeField]
    int ToLayer = 0;

    [SerializeField]
    bool RenderOutside = true;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "PreventEntry" && !other.isTrigger)
        {
            Teleport(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "PreventEntry" && !collision.collider.isTrigger)
        {
            Teleport(collision.gameObject);
        }
    }

    void Teleport(GameObject obj)
    {

        obj.transform.position = TpTo.transform.position;
        if (obj.layer != 10) //layer 10 is the unique layer for the player.
        {
            obj.layer = ToLayer;
        }
        else
        {
            //TODO: get cam through game manager for performance
            GameManager.instance.camerascript.transform.position = TpTo.transform.position;
            Camera.main.transform.parent.GetComponent<Camerascript>().SwitchLayerMask(RenderOutside);
        }

    }
}
