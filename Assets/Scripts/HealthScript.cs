using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    public UnityEvent OnHealthChanged;


    [SerializeField]
    float MaxHealth;

    public float Health;

    void Start()
    {
        Health = MaxHealth;
    }

    public void DoDamage(float Damage)
    {
        Health -= Damage;
        OnHealthChanged.Invoke();
    }
}
