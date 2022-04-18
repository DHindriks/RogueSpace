using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    
    public ItemBase ContainedItem;

    void Awake()
    {
        GameObject droppedobj = Instantiate(ContainedItem.DroppedItem, transform);
        GameObject Particle = Instantiate(ContainedItem.ParticleSystem, transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<Inventory>())
        {
            other.transform.root.GetComponent<Inventory>().AddItem(ContainedItem);
            Destroy(this);
            GameManager.instance.ShrinkDespawn(gameObject);
        }else if (other.GetComponent<Inventory>())
        {
            other.GetComponent<Inventory>().AddItem(ContainedItem);
            Destroy(this);
            GameManager.instance.ShrinkDespawn(gameObject);
        }
    }
}
