using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenItemMenu : MonoBehaviour
{
    public List<ItemBase> Inventory;

    [SerializeField]
    InventoryGen Generator;

    [SerializeField]
    GameObject InventoryCanvas;

    Player player;

    void Start()
    {
        GameManager.instance.MotherShipInv = this;
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
        foreach(ItemBase item in player.Inventory)
        {
            AddItem(item);
        }

        player.Inventory.Clear();
    }

    public void AddItem(ItemBase item)
    {
        if (item.Type == ItemTypes.Resource)
        {
            foreach (ItemBase resource in Inventory)
            {
                if (resource.Name == item.Name)
                {
                    resource.amount += item.amount;
                    return;
                }
            }
            Inventory.Add(item);

        }
        else
        {
            Inventory.Add(item);
        }
        Generator.GenerateInv();
    }
}
