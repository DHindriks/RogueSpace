using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthScript))]
public class ItemDeposit : MonoBehaviour
{

    HealthScript health;

    public UnityEvent OnDestroyed;

    [SerializeField]
    GameObject DestroyedParticles;

    public List<LootObj> Inventory;

    void Start()
    {
        health = GetComponent<HealthScript>();
        health.OnHealthChanged.AddListener(CheckHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (health != null)
        {
            health.DoDamage(collision.relativeVelocity.magnitude / 2);
        }
    }

    void CheckHealth()
    {
        if (health.Health <= 0)
        {
            DestroyDeposit();
        }
    }

    void DestroyDeposit()
    {
        foreach (LootObj loot in Inventory)
        {
            GameObject SpawnedLoot = Instantiate(loot.ItemPrefab);
            SpawnedLoot.transform.position = transform.position;
            SpawnedLoot.GetComponent<ItemContainer>().ContainedItem.amount = loot.SetAmount;
        }
        if (DestroyedParticles != null)
        {
            GameObject part = Instantiate(DestroyedParticles);
            part.transform.position = transform.position;
        }

        OnDestroyed.Invoke();
        Destroy(gameObject);
    }
}
