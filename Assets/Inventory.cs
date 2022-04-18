using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<ItemBase> inventory = new List<ItemBase>();

    public void AddItem(ItemBase item)
    {
        if (item.Type == ItemTypes.Resource)
        {
            foreach (ItemBase resource in inventory)
            {
                if (resource.Name == item.Name)
                {
                    resource.amount += item.amount;
                    return;
                }
            }
            inventory.Add(item);

        }
        else
        {
            inventory.Add(item);
        }
    }

}
