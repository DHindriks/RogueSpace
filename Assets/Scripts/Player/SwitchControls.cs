﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControls : MonoBehaviour
{
    [SerializeField]
    Transform pos;

    [SerializeField]
    DropshipControls dropshipControls;

    [SerializeField]
    Player player;

    public void SwitchShip()
    {
        dropshipControls.CtrlEnabled = !dropshipControls.CtrlEnabled;
        player.CtrlEnabled = !player.CtrlEnabled;

        if (dropshipControls.CtrlEnabled)
        {
            dropshipControls.GetComponent<Rigidbody>().drag = 1;
            dropshipControls.OpenDoor(false);
            player.transform.SetParent(pos.transform);
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.transform.position = pos.position;
            player.transform.GetChild(0).rotation = pos.rotation;
            player.gameObject.tag = "PreventEntry";
            GameManager.instance.camerascript.ChangeZoom(200);
        }
        else
        {
            dropshipControls.GetComponent<Rigidbody>().drag = 100;
            dropshipControls.OpenDoor(true);
            player.transform.SetParent(null);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
            player.GetComponent<Rigidbody>().isKinematic = false;
            GameManager.instance.camerascript.ChangeZoom(90);
            player.gameObject.tag = "Player";


        }
    }
}
