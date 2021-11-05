﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGen : MonoBehaviour
{
    [SerializeField]
    GameObject BtnBase;


    [SerializeField]
    OpenItemMenu Inventory;


    private void OnEnable()
    {
        GenerateInv();
    }

    public void GenerateInv()
    {
        foreach(Transform obj in transform)
        {
            Destroy(obj.gameObject);
        }

        //foreach (ItemBase item in Inventory.Inventory)
        for(int i = 0; i < Inventory.Inventory.Count; i++)
        {
           if(Inventory.Inventory[i].Type != ItemTypes.Resource)
            {
                GameObject Btn = Instantiate(BtnBase, transform);
                Button btnScript = Btn.GetComponent<Button>();

                Btn.name = i.ToString();

                ItemBase item = Inventory.Inventory[i];
                int thisID = i;

                switch (Inventory.Inventory[i].Type)
                {
                    case ItemTypes.Weapon:
                        btnScript.onClick.AddListener(delegate { GameManager.instance.player.EquipWeapon(item); });
                        btnScript.onClick.AddListener(delegate { RemoveItem(thisID); });
                        //btnScript.onClick.AddListener(delegate { GenerateInv(); });
                        break;

                    case ItemTypes.Shield:
                        btnScript.onClick.AddListener(delegate { GameManager.instance.player.EquipShield(item); });
                        btnScript.onClick.AddListener(delegate { RemoveItem(thisID); });
                        break;

                    case ItemTypes.Engine:
                        btnScript.onClick.AddListener(delegate { GameManager.instance.player.EquipBooster(item); });
                        btnScript.onClick.AddListener(delegate { RemoveItem(thisID); });
                        break;
                }

            }
        }

        float ContentHeight = Mathf.Ceil(transform.childCount / 8f) * 176;

        GetComponent<RectTransform>().sizeDelta = new Vector2(0, ContentHeight);
    }

    void RemoveItem(int ID, int Amount = 1)
    {
        Debug.Log("removed " + ID);
        Inventory.Inventory[ID].amount -= Amount;

        if(Inventory.Inventory[ID].amount <= 0)
        {
            Debug.Log(ID + "aa");
            Inventory.Inventory.RemoveAt(ID);
        }
        GenerateInv();
    }
}
