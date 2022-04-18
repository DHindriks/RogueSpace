using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

[RequireComponent(typeof(HealthScript))]
public class Player : MonoBehaviour
{

    public bool CtrlEnabled;

    public float speed = 6.0F;

    [SerializeField]
    bool DBGControls;

    [SerializeField]
    Joystick Joystick;

    GameObject CurrentSkin;

    ItemBase EquippedWeapon;
    ItemBase EquippedEngine;
    ItemBase EquippedShield;

    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody controller;

    HealthScript Health;
    Quaternion rot;

    void Start()
    {
        Analytics.CustomEvent("StartGame");
        // Store reference to attached component
        controller = GetComponent<Rigidbody>();
        Health = GetComponent<HealthScript>();
        Health.OnHealthChanged.AddListener(CheckDamage);
        GameManager.instance.player = this;
        SetSkin(0);
    }

    public void SetSkin(int ID = 0)
    {
        foreach(Transform obj in transform.GetChild(0).GetChild(0))
        {
            Destroy(obj.gameObject);
        }

        CurrentSkin = Instantiate(GameManager.instance.SkinList[ID], transform.GetChild(0).GetChild(0));
        CurrentSkin.transform.localPosition = Vector3.zero;
        CurrentSkin.transform.localRotation = Quaternion.Euler(0, 0, 0);

    }

    public void EquipWeapon(ItemBase weapon)
    {
        UnEquip(ItemTypes.Weapon);
        foreach (Transform obj in transform.GetChild(0).GetChild(1))
        {
            Destroy(obj.gameObject);
        }
        EquippedWeapon = weapon;

        GameObject Spawnedprefab = Instantiate(weapon.EquippedObj, transform.GetChild(0).GetChild(1));

    }



    public void EquipShield(ItemBase shield)
    {
        UnEquip(ItemTypes.Shield);
        foreach (Transform obj in transform.GetChild(0).GetChild(2))
        {
            Destroy(obj.gameObject);
        }
        EquippedShield = shield;

        GameObject Spawnedprefab = Instantiate(shield.EquippedObj, transform.GetChild(0).GetChild(2));

    }

    public void EquipBooster(ItemBase booster)
    {
        UnEquip(ItemTypes.Engine);

        foreach (Transform obj in transform.GetChild(0).GetChild(3))
        {
            Destroy(obj.gameObject);
        }
        EquippedEngine = booster;

        GameObject Spawnedprefab = Instantiate(booster.EquippedObj, transform.GetChild(0).GetChild(3));

    }

    public void UnEquip(ItemTypes type)
    {
        switch(type)
        {
            case ItemTypes.Weapon:
                if(EquippedWeapon != null)
                {
                    GameManager.instance.MotherShipInv.AddItem(EquippedWeapon);
                    EquippedWeapon = null;
                    foreach (Transform obj in transform.GetChild(0).GetChild(1))
                    {
                        Destroy(obj.gameObject);
                    }
                }
                break;

            case ItemTypes.Engine:
                if (EquippedEngine != null)
                {
                    GameManager.instance.MotherShipInv.AddItem(EquippedEngine);
                    EquippedEngine = null;
                    foreach (Transform obj in transform.GetChild(0).GetChild(3))
                    {
                        Destroy(obj.gameObject);
                    }
                }
                break;

            case ItemTypes.Shield:
                if (EquippedShield != null)
                {
                    GameManager.instance.MotherShipInv.AddItem(EquippedShield);
                    EquippedShield = null;
                    foreach (Transform obj in transform.GetChild(0).GetChild(2))
                    {
                        Destroy(obj.gameObject);
                    }
                }
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Health.DoDamage(Mathf.Floor(collision.relativeVelocity.magnitude / 4));
    }

    void CheckDamage()
    {
        if (Health.Health == 0)
        {
            //player gameover
        }
    }

    void Update()
    {
        if (DBGControls)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }else if (CtrlEnabled)
        {
            moveDirection = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
        }


        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;


        if (moveDirection.magnitude > 0.01f)
        {
            rot = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, rot, Time.deltaTime * 250);

        }
    }

    void FixedUpdate()
    {
        if(Quaternion.Angle(rot, transform.GetChild(0).transform.rotation) < 28)
        {
            controller.AddForce(moveDirection);
        }
    }
}