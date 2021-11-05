using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject Source;

    public int Damage;

    void Start()
    {
        Destroy(gameObject, 15);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject != Source && !other.isTrigger)
        {
        Debug.Log("HIT" + other.name);
            if (other.transform.root.GetComponent<HealthScript>())
            {
                other.transform.root.GetComponent<HealthScript>().DoDamage(Damage);
            }if (other.transform.GetComponentInParent<HealthScript>())
            {
                other.transform.GetComponentInParent<HealthScript>().DoDamage(Damage);
            }


            if (other.transform.root.GetComponent<Rigidbody>())
            {
                other.transform.root.GetComponent<Rigidbody>().AddExplosionForce(1000, transform.position, 1.5f);
            }
            else if (other.transform.GetComponentInParent<Rigidbody>())
            {
                other.transform.GetComponentInParent<Rigidbody>().AddExplosionForce(1000, transform.position, 5);
            }



            Destroy(gameObject);
        }
    }
}
