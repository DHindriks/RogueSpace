using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    HealthScript Health;

    GameObject Target;

    [SerializeField]
    GameObject BulletPrefab;

    [SerializeField]
    float FireRate;

    float NextShot;

    States State;
    public enum States
    {
        Idle,
        Aggressive,
        Dead
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = GetComponent<HealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (State == States.Aggressive && Target != null)
        {
            transform.LookAt(Target.transform);


            if (Time.time > NextShot)
            {
                NextShot = Time.time + FireRate;
                GameObject Bullet = Instantiate(BulletPrefab);
                Bullet.GetComponent<BulletScript>().Source = transform.root.gameObject;
                Bullet.GetComponent<BulletScript>().Damage = 40;
                Bullet.transform.position = transform.position;
                Bullet.transform.rotation = transform.rotation;
                Bullet.GetComponent<Rigidbody>().AddForce(Bullet.transform.forward * 40, ForceMode.VelocityChange);
            }
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PhysObj")
        {
            Target = other.gameObject;
            Debug.Log("Spotted " + other.name);
            State = States.Aggressive;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
        {
            State = States.Idle;
            Target = null;
            NextShot = Time.time + FireRate * 2;
        }
    }

}
