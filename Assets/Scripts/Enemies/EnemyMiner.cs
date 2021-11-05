using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MinerStates
{
    Wander,
    Fleeing,
    Avoiding
}
[RequireComponent(typeof(Rigidbody))]
public class EnemyMiner : MonoBehaviour
{

    MinerStates State;

    Rigidbody rb;

    Vector3 Heading;

    [SerializeField]
    int range = 50;

    float AvoidCooldown;
    [SerializeField]
    Transform ViewOriginL;


    [SerializeField]
    Transform ViewOriginR;

    [SerializeField]
    LayerMask RayLayerMask;

    Quaternion rot;
    Vector3 dir;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        State = MinerStates.Wander;
        SetNewHeading();
    }

    void SetNewHeading()
    {
        Heading = new Vector3(Random.Range(1000, -1000), 0, Random.Range(1000, -1000));
    }

    // Update is called once per frame
    void Update()
    {
        if (State == MinerStates.Wander) //Wander state
        {
            dir = (Heading - transform.position).normalized;

            if (dir.magnitude > 0.01f)
            {
                rot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * 45);
            }

            if (Quaternion.Angle(rot, transform.rotation) < 10)
            {
                rb.AddForce(transform.forward * 600 * Time.deltaTime, ForceMode.Acceleration);
            }else if (Vector3.Distance(transform.position, Heading) < 50)
            {
                SetNewHeading();
            }

            //check obstacles
            RaycastHit hitL;
            if (Physics.Raycast(ViewOriginL.position, transform.TransformDirection(Vector3.forward), out hitL, range * 1.5f, RayLayerMask))
            {
                Debug.DrawRay(ViewOriginL.position, transform.TransformDirection(Vector3.forward) * hitL.distance, Color.red);
                State = MinerStates.Avoiding;
                CancelInvoke("ResetState");
                Invoke("ResetState", 10);
            }
            else
            {
                Debug.DrawRay(ViewOriginL.position, transform.TransformDirection(Vector3.forward) * range * 1.5f, Color.green);
            }

            RaycastHit hitR;
            if (Physics.Raycast(ViewOriginR.position, transform.TransformDirection(Vector3.forward), out hitR, range * 1.5f, RayLayerMask))
            {
                Debug.DrawRay(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * hitR.distance, Color.red);
                State = MinerStates.Avoiding;
                CancelInvoke("ResetState");
                Invoke("ResetState", 10);
            }
            else
            {
                Debug.DrawRay(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * range * 1.5f, Color.green);
            }
        }else if (State == MinerStates.Avoiding)  //Avoid state
        {
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
            else if(RayL && RayR)
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * 45);
            }

            if (Quaternion.Angle(rot, transform.rotation) < 10)
            {
                rb.AddForce(transform.forward * 600 * Time.deltaTime, ForceMode.Acceleration);
            }
            else if (Vector3.Distance(transform.position, Heading) < 150)
            {
                SetNewHeading();
            }

        }
    }

    void ResetState()
    {
        State = MinerStates.Wander;
    }

}
