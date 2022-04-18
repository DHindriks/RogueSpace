using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class OpenItemMenu : MonoBehaviour
{

    [SerializeField]
    InventoryGen Generator;

    [SerializeField]
    GameObject InventoryCanvas;

    Inventory inventory;

    Player player;

    void Start()
    {
        GameManager.instance.MotherShipInv = this;
        inventory = GetComponent<Inventory>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player" && !other.isTrigger)
        {
            InventoryCanvas.SetActive(true);
            player = other.transform.root.GetComponent<Player>();
            TransferItems();
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            InventoryCanvas.SetActive(false);
        }
    }

    void TransferItems()
    {
        foreach(ItemBase item in player.GetComponent<Inventory>().inventory)
        {
            AddItem(item);
        }

        player.GetComponent<Inventory>().inventory.Clear();
    }

    public void AddItem(ItemBase item)
    {
        if (item.Type == ItemTypes.Resource)
        {
            foreach (ItemBase resource in inventory.inventory)
            {
                if (resource.Name == item.Name)
                {
                    resource.amount += item.amount;
                    return;
                }
            }
            inventory.inventory.Add(item);

        }
        else
        {
            inventory.inventory.Add(item);
        }
        Generator.GenerateInv();
    }
}
