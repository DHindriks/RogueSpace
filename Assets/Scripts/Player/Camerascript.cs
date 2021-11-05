using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerascript : MonoBehaviour
{

    public GameObject FollowTarget;
    public GameObject LookTarget;

    [HideInInspector]
    public float PosLerpSpeed = 10;
    float LookLerpspeed = 1f;

    Quaternion TargetRotation;
    Coroutine LastRoutine;

    Camera MainCamera;
    [SerializeField]
    GameObject playerobj;
    [HideInInspector]
    public float AxisLerpamount;
    float LookLerpamount;

    [SerializeField]
    LayerMask OnExterior;

    [SerializeField]
    LayerMask OnInterior;

    // Use this for initialization
    void Start()
    {
        MainCamera = gameObject.GetComponentInChildren<Camera>();
        SetcamPos();
        ChangeZoom();
        GameManager.instance.camerascript = this;
    }

    public void SwitchLayerMask(bool IsOutside = true)
    {
        if (IsOutside)
        {
            ChangeZoom(90, 1);
            MainCamera.cullingMask = OnExterior;
            MainCamera.clearFlags = CameraClearFlags.Nothing;
        }else
        {
            ChangeZoom(60, 1);
            MainCamera.cullingMask = OnInterior;
            MainCamera.clearFlags = CameraClearFlags.SolidColor;
        }
    }

    public void SetcamPos(GameObject Postarget = null, GameObject Lookttarget = null, float Axislerp = 0, float LookLerp = 0, float PosSpeed = 5f, float RotSpeed = 0f)
    {
        if (Postarget == null && playerobj != null)
        {
            Postarget = playerobj;
        }
        if (Lookttarget == null && playerobj != null)
        {
            Lookttarget = playerobj;
        }
        FollowTarget = Postarget;
        LookTarget = Lookttarget;
        AxisLerpamount = Axislerp;
        LookLerpamount = LookLerp;
        PosLerpSpeed = PosSpeed;
        LookLerpspeed = RotSpeed;
    }

    public void ChangeFOV(float FOV = 60, float LerpTime = 2)
    {
        if (LastRoutine != null)
        {
            StopCoroutine(LastRoutine);
        }
        LastRoutine = StartCoroutine(LerpFOV(FOV, LerpTime));
    }

    public void ChangeZoom(float Zoom = 90, float LerpTime = 2)
    {
        if (LastRoutine != null)
        {
            StopCoroutine(LastRoutine);
        }
        LastRoutine = StartCoroutine(LerpZoom(Zoom, LerpTime));
    }

    IEnumerator LerpFOV(float FOV, float LerpTime)
    {
        float Rate = 1.0f / LerpTime;
        float i = 0;

        while (i < 1)
        {
            i += Time.deltaTime * Rate;
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, FOV, i);
            yield return 0;
        }

        MainCamera.fieldOfView = FOV;
    }

    IEnumerator LerpZoom(float Zoom, float LerpTime)
    {
        float Rate = 1.0f / LerpTime;
        float i = 0;

        Vector3 pos = new Vector3();
        pos = MainCamera.transform.localPosition;

        while (i < 1)
        {
            i += Time.deltaTime * Rate;
            pos.y = Mathf.Lerp(pos.y, Zoom, i);
            MainCamera.gameObject.transform.localPosition = pos;
            yield return 0;
        }

        pos.y = Zoom;
        MainCamera.gameObject.transform.localPosition = pos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(Vector3.Lerp(FollowTarget.transform.position, this.transform.GetChild(0).transform.localPosition, AxisLerpamount).x, FollowTarget.transform.position.y, FollowTarget.transform.position.z), PosLerpSpeed * Time.deltaTime);
        TargetRotation = Quaternion.LookRotation(LookTarget.transform.position - this.transform.GetChild(0).position);
        this.transform.GetChild(0).rotation = Quaternion.Slerp(this.transform.GetChild(0).rotation, Quaternion.Lerp(TargetRotation, transform.rotation, LookLerpamount), LookLerpspeed * Time.deltaTime);
    }
}
