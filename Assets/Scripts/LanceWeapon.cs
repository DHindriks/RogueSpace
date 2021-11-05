using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceWeapon : MonoBehaviour
{

    Rigidbody rb;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HealthScript>())
        {
            other.GetComponent<HealthScript>().DoDamage(10);
            Vector3 dir = other.transform.position - transform.position;
            rb.AddForce( -dir.normalized * 2000 * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}
