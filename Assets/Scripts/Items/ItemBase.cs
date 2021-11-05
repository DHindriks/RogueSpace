using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemBase
{
    //base variables
    public string Name;
    public GameObject DroppedItem;
    public GameObject ParticleSystem;
    public Sprite Icon;

    //item type
    public ItemTypes Type;

    //equipment variables
    public float Durability;
    public GameObject EquippedObj;

    //amount of items, used to stack resource items
    public int amount;

}

public enum ItemTypes
{
    Resource,
    Weapon,
    Shield,
    Engine
}
