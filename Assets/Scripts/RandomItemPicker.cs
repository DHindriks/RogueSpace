using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemDeposit))]
public class RandomItemPicker : MonoBehaviour
{
    [SerializeField]
    List<LootObj> PotentialItems;

    // Start is called before the first frame update
    void Start()
    {
        ItemDeposit deposit = GetComponent<ItemDeposit>();
        foreach(LootObj lootObj in PotentialItems)
        {
            lootObj.SetAmount = Random.Range(lootObj.MinAmount, lootObj.MaxAmount);
            if(lootObj.SetAmount != 0)
            {
                deposit.Inventory.Add(lootObj);
            }
        }
    }
}
