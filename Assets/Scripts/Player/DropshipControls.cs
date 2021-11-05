using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropshipControls : MonoBehaviour
{
    public bool CtrlEnabled;

    public float speed;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;

    [SerializeField]
    bool DBGControls;

    bool Manual;

    [SerializeField]
    Joystick Joystick;

    [SerializeField]
    Animator ShipAnim;

    Player player;

    Quaternion rot;

    //Dropship AI
    [SerializeField]
    ParticleSystem Signal;


    enum DropShipStates
    {
        Idle,
        Navigating,
        Avoiding
    }

    DropShipStates State;

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

    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameManager.instance.player;
        State = DropShipStates.Idle;
    }

    public void OpenDoor(bool opened)
    {
        ShipAnim.SetBool("Opened", opened);
    }

    // Update is called once per frame
    void Update()
    {
        if (State == DropShipStates.Idle)
        {
            if (DBGControls)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            }
            else if (CtrlEnabled)
            {
                moveDirection = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
            }
            else if (!CtrlEnabled && Vector3.Distance(player.transform.position, transform.position) > 150 && player.transform.position.y < 10)
            {
                State = DropShipStates.Navigating;
                Signal.Play();
                rb.drag = 1;
            }
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (moveDirection.magnitude > 0.01f)
            {
                rot = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, rot, Time.deltaTime * 175);
            }

            if (Quaternion.Angle(rot, transform.GetChild(0).rotation) < 18)
            {
                rb.AddForce(moveDirection * Time.deltaTime, ForceMode.Acceleration);
            }
        }
        else if (State == DropShipStates.Navigating)
        {
            Heading = player.transform.position;
            dir = (Heading - transform.position).normalized;

            if (dir.magnitude > 0.01f)
            {
                rot = Quaternion.LookRotation(dir, Vector3.up);
                transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, rot, Time.deltaTime * 15);
            }

            if (Quaternion.Angle(rot, transform.GetChild(0).rotation) < 10)
            {
                rb.AddForce(transform.GetChild(0).forward * speed * Time.deltaTime, ForceMode.Acceleration);
            }

            if (Vector3.Distance(transform.position, Heading) < 75)
            {
                State = DropShipStates.Idle;
                Signal.Stop();
                rb.drag = 10;
            }

            //check obstacles
            RaycastHit hitL;
            RaycastHit hitR;
            if (Physics.Raycast(ViewOriginL.position, transform.GetChild(0).TransformDirection(Vector3.forward), out hitL, 100, RayLayerMask))
            {
                Debug.DrawRay(ViewOriginL.position, transform.TransformDirection(Vector3.forward) * hitL.distance, Color.red);
                CancelInvoke("ResetState");
                State = DropShipStates.Avoiding;
                Invoke("ResetState", 5);
            }
            else if (Physics.Raycast(ViewOriginR.position, transform.GetChild(0).TransformDirection(Vector3.forward) * 100, out hitR, 100, RayLayerMask))
            {
                Debug.DrawRay(ViewOriginR.position, transform.TransformDirection(Vector3.forward) * hitR.distance, Color.red);
                CancelInvoke("ResetState");
                State = DropShipStates.Avoiding;
                Invoke("ResetState", 5);
            }
        }
        else if (State == DropShipStates.Avoiding)  //Avoid state
        {
            //check obstacles
            RaycastHit hitL;
            RaycastHit hitR;


            bool RayR = Physics.Raycast(ViewOriginR.position, transform.GetChild(0).TransformDirection(Vector3.forward), out hitR, range, RayLayerMask);
            bool RayL = Physics.Raycast(ViewOriginL.position, transform.GetChild(0).TransformDirection(Vector3.forward), out hitL, range, RayLayerMask);
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
                transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, rot, Time.deltaTime * 15);
            }

            if (Quaternion.Angle(rot, transform.GetChild(0).rotation) < 10)
            {
                rb.AddForce((transform.GetChild(0).forward * speed) * Time.deltaTime, ForceMode.Acceleration);
            }

            if (Vector3.Distance(transform.position, Heading) < 75)
            {
                State = DropShipStates.Idle;
                Signal.Stop();
                rb.drag = 10;
                CancelInvoke("ResetState");
            }
        }
    }

    void ResetState()
    {
        State = DropShipStates.Navigating;
        Signal.Play();
        CancelInvoke("ResetState");
    }
}


