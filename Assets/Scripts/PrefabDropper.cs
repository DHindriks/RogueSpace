using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDropper : MonoBehaviour
{
    [SerializeField]
    GameObject ItemToDrop;

    public void DropItem()
    {
        GameObject DroppedItem = Instantiate(ItemToDrop);
        DroppedItem.transform.position = this.transform.position;
    }
}
