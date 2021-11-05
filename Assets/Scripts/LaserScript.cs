using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField]
    GameObject HitParticle;

    [SerializeField]
    List<EnemyTripWire> TripWires;

    public void DisableLaser()
    {
        foreach(EnemyTripWire Wire in TripWires)
        {
            if (Wire != null)
            {
                Wire.StopParticles();
            }
        }

        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.GetComponent<HealthScript>())
        {
            other.gameObject.GetComponent<HealthScript>().DoDamage(20);
        }else if (other.gameObject.GetComponentInParent<HealthScript>())
        {
            other.gameObject.GetComponentInParent<HealthScript>().DoDamage(20);
        }

        Debug.Log(other.gameObject.name);


        if (other.gameObject.GetComponent<Rigidbody>())
        {
            Vector3 dir = other.contacts[0].point - other.transform.position;
            dir = -dir.normalized;
            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * 50, ForceMode.VelocityChange);
            Debug.Log(dir);
        }
        else if (other.gameObject.GetComponentInParent<Rigidbody>())
        {
            Vector3 dir = other.contacts[0].point - transform.position;
            dir = -dir.normalized;
            other.gameObject.GetComponentInParent<Rigidbody>().AddForce(dir * 50, ForceMode.VelocityChange);
        }
        if (HitParticle != null)
        {
            GameObject Particle = Instantiate(HitParticle);
            Particle.transform.position = other.contacts[0].point;
        }
    }
}
