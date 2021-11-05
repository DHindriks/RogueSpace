using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxCam : MonoBehaviour
{
    [SerializeField]
    GameObject FollowTarget;

    [SerializeField]
    Camerascript camerascript;

    void Update()
    {
        Vector3 FollowTrgt = new Vector3(FollowTarget.transform.position.x, 0, FollowTarget.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, FollowTrgt / 100, camerascript.PosLerpSpeed * 4 * Time.deltaTime);
    }
}
