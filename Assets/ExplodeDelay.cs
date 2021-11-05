using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeDelay : MonoBehaviour
{

    [SerializeField]
    ParticleSystem ExplosionEffect;

    [SerializeField]
    int range;

    [SerializeField]
    int Damage;

    [SerializeField]
    int Delay;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", Delay);

        if (GetComponent<Rigidbody>())
        {
            rb = GetComponent<Rigidbody>();
            rb.AddTorque(new Vector3(Random.Range(0, 20), Random.Range(0, 20), Random.Range(0, 20)), ForceMode.VelocityChange);
        }
    }

    void Explode()
    {
        GameObject Explosion = Instantiate(ExplosionEffect.gameObject);
        Explosion.transform.position = transform.position;

        Collider[] objects = UnityEngine.Physics.OverlapSphere(transform.position, range);

        foreach (Collider c in objects)
        {
            if (c.gameObject != gameObject)
            {
                Rigidbody r = c.transform.root.GetComponentInChildren<Rigidbody>();
                if (r != null)
                {
                    r.AddExplosionForce(1000, transform.position, range);
                }
                HealthScript health = c.GetComponent<HealthScript>();
                if (health != null && c.tag != "Player")
                {
                    health.DoDamage(Damage);
                }
                Debug.Log("Hit: " + c.gameObject.name);
            }

            Destroy(gameObject);

        }


    }
}
