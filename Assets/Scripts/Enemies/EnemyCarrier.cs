using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CarrierStates
{
    Wander,
    Fleeing,
    Avoiding
}
[RequireComponent(typeof(Rigidbody))]
public class EnemyCarrier : MonoBehaviour
{

    CarrierStates State;

    Rigidbody rb;

    Vector3 Heading;

    [SerializeField]
    int speed = 600;

    float AvoidCooldown;

    [SerializeField]
    int range = 100;

    [SerializeField]
    int TurnSpeed = 200;

    [SerializeField]
    Transform ViewOriginL;


    [SerializeField]
    Transform ViewOriginR;

    [SerializeField]
    LayerMask RayLayerMask;

    Quaternion rot;
    Vector3 dir;

    [SerializeField]
    List<ItemDeposit> itemDeposits;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        State = CarrierStates.Wander;
        SetNewHeading();

        foreach(ItemDeposit deposit in itemDeposits)
        {
            deposit.OnDestroyed.AddListener(EnterFleeMode);
        }

    }

    void SetNewHeading() // generates random location where carrier will travel to.
    {
        Heading = new Vector3(Random.Range(1000, -1000), 0, Random.Range(1000, -1000));
    }

    // Update is called once per frame
    void Update()
    {
        if (State == CarrierStates.Wander) //Wander state
        {
            dir = (Heading - transform.position).normalized;

            if (dir.magnitude > 0.01f)
            {
                rot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * 15);
            }

            if (Quaternion.Angle(rot, transform.rotation) < 10)
            {
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Acceleration);
            }else if (Vector3.Distance(transform.position, Heading) < 50)
            {
                SetNewHeading();
            }

            //check obstacles
            RaycastHit hitL;
            RaycastHit hitR;
            if (Physics.Raycast(ViewOriginL.position, transform.TransformDirection(Vector3.forward), out hitL, 100, RayLayerMask))
            {
                Debug.DrawRay(ViewOriginL.position, transform.TransformDirection(Vector3.forward) * hitL.distance, Color.red);
                CancelInvoke("ResetState");
                State = CarrierStates.Avoiding;
                Invoke("ResetState", 30);
            }else if (Physics.Raycast(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * 100, out hitR, 100, RayLayerMask))
            {
                Debug.DrawRay(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * hitR.distance, Color.red);
                CancelInvoke("ResetState");
                State = CarrierStates.Avoiding;
                Invoke("ResetState", 30);
            }
        }else if (State == CarrierStates.Avoiding)  //Avoid state
        {
            ////check obstacles
            //RaycastHit hitL;
            //RaycastHit hitR;
            //if (Physics.Raycast(ViewOriginL.position, transform.TransformDirection(Vector3.forward), out hitL, 100, RayLayerMask) && Time.time > AvoidCooldown)
            //{
            //    //rotate away
            //    dir = (hitL.point - transform.position).normalized;
            //    dir = Quaternion.AngleAxis(90, Vector3.up) * dir;
            //    AvoidCooldown = Time.time + 3;
            //    Debug.DrawRay(ViewOriginL.position, transform.TransformDirection(Vector3.forward) * TurnSpeed, Color.red);
            //}
            //else if (Physics.Raycast(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * 100, out hitR, 100, RayLayerMask) && Time.time > AvoidCooldown)
            //{
            //    // rotate away
            //    dir = (hitR.point - transform.position).normalized;
            //    dir = Quaternion.AngleAxis(-90, Vector3.up) * dir;
            //    AvoidCooldown = Time.time + 3;
            //    Debug.DrawRay(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * TurnSpeed, Color.red);
            //}

            //check obstacles
            RaycastHit hitL;
            RaycastHit hitR;


            bool RayR = Physics.Raycast(ViewOriginR.position, transform.TransformDirection(Vector3.forward), out hitR, range, RayLayerMask);
            bool RayL = Physics.Raycast(ViewOriginL.position, transform.TransformDirection(Vector3.forward), out hitL, range, RayLayerMask);
            Debug.DrawRay(ViewOriginL.position, transform.TransformDirection(Vector3.forward) * range, Color.green);
            Debug.DrawRay(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * range, Color.green);
            if (RayL && Vector3.Distance(transform.position, hitL.point) < range && !RayR)
            {
                //rotate away
                dir = (hitL.point - transform.position).normalized;
                dir = Quaternion.AngleAxis(45, Vector3.up) * dir;
                Debug.DrawRay(ViewOriginL.position, transform.TransformDirection(Vector3.forward) * hitL.distance, Color.red);
            }
            else if (!RayL && Vector3.Distance(transform.position, hitR.point) < range && RayR)
            {
                // rotate away
                dir = (hitR.point - transform.position).normalized;
                dir = Quaternion.AngleAxis(-45, Vector3.up) * dir;
                Debug.DrawRay(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * hitR.distance, Color.red);
            }
            else if (RayL && RayR)
            {
                if (Vector3.Distance(hitL.point, transform.position) > Vector3.Distance(hitR.point, transform.position) && Time.time > AvoidCooldown)
                {
                    // rotate away right
                    dir = (hitR.point - transform.position).normalized;
                    dir = Quaternion.AngleAxis(-90, Vector3.up) * dir;
                    AvoidCooldown = Time.time + 5;
                }
                else
                {
                    // rotate away left
                    dir = (hitL.point - transform.position).normalized;
                    dir = Quaternion.AngleAxis(90, Vector3.up) * dir;
                    AvoidCooldown = Time.time + 5;
                }
            }


            if (dir.magnitude > 0.01f)
            {
                rot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * 15);
            }

            if (Quaternion.Angle(rot, transform.rotation) < 10)
            {
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Acceleration);
            }
            else if (Vector3.Distance(transform.position, Heading) < 150)
            {
                SetNewHeading();
            }

        }
    }

    void ResetState()
    {
        State = CarrierStates.Wander;
    }

    void EnterFleeMode()
    {
        State = CarrierStates.Avoiding;
        speed = 1800;
        TurnSpeed = 350;
        Invoke("Despawn", 20);
    }

    void Despawn()
    {
        GetComponent<Animator>().SetBool("Despawn", true);
        Destroy(gameObject, 10);
    }


}
