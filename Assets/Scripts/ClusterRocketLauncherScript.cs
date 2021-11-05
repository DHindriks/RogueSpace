using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherScript : MonoBehaviour
{
    [SerializeField]
    Joystick Joystick;

    Vector3 moveDirection;
    Quaternion rot;

    [SerializeField]
    GameObject FollowProjectile;

    List<GameObject> Targets = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);

        //Joystick.gameObject.transform.eulerAngles = new Vector3(0, 0, -transform.root.GetChild(0).localRotation.eulerAngles.y);

        if (moveDirection.magnitude > 0.01f)
        {
            rot = Quaternion.LookRotation(moveDirection);

            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, rot, Time.deltaTime * 500);
        }else if (Targets.Count > 0)
        {
            foreach(GameObject target in Targets)
            {
                GameObject Projectile = Instantiate(FollowProjectile);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Targets.Add(other.gameObject);
            Debug.Log("Target found");
        }
    }

}
