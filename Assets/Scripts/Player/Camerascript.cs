using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Camerascript : MonoBehaviour
{

    public GameObject FollowTarget;
    public GameObject LookTarget;

    [HideInInspector]
    public float PosLerpSpeed = 10;
    float LookLerpspeed = 1f;

    Quaternion TargetRotation;
    Coroutine LastRoutine;
    Coroutine LastTiltRoutine;

    Camera MainCamera;

    [SerializeField]
    Camera SkyboxCam;

    [SerializeField]
    GameObject playerobj;
    [HideInInspector]
    public float AxisLerpamount;
    float LookLerpamount;

    [SerializeField]
    LayerMask OnExterior;

    [SerializeField]
    LayerMask OnInterior;

    [SerializeField]
    Animator FadeAnim;

    public UnityEvent Switched;

    Camera currentcam;
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

            SkyboxCam.cullingMask = LayerMask.GetMask("Skybox");
            SkyboxCam.clearFlags = CameraClearFlags.Skybox;
        }
        else
        {
            ChangeZoom(60, 1);
            MainCamera.cullingMask = OnInterior;
            MainCamera.clearFlags = CameraClearFlags.SolidColor;

            SkyboxCam.cullingMask = OnInterior;
            SkyboxCam.clearFlags = CameraClearFlags.SolidColor;
        }
    }

    public void SetcamPos(GameObject Postarget = null, GameObject Lookttarget = null, float Axislerp = 0, float LookLerp = 1, float PosSpeed = 5f, float RotSpeed = 0.5f)
    {
        if (Postarget == null && playerobj != null)
        {
            Postarget = playerobj;
        }
        if (Lookttarget == null && playerobj != null)
        {
            Lookttarget = playerobj.transform.GetChild(0).GetChild(4).gameObject;
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

    public void ChangeTilt(float LerpTime = 1, Vector3 Rotation = default)
    {
        if (LastTiltRoutine != null)
        {
            StopCoroutine(LastTiltRoutine);
        }
        LastTiltRoutine = StartCoroutine(LerpTilt(Rotation, LerpTime));
    }

    public void ChangeCam(Camera Newcam = null)
    {
        StopCoroutine(CamFadeTransition());

        StartCoroutine(CamFadeTransition(Newcam));
    }

    IEnumerator CamFadeTransition(Camera Newcam = null)
    {

        FadeAnim.SetBool("Faded", true);
        yield return new WaitForSeconds(FadeAnim.GetCurrentAnimatorStateInfo(0).length);

        //while (FadeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    yield return null;
        //}

        if (currentcam != null)
        {
            currentcam.enabled = false;
            currentcam = null;
        }
        if (Newcam == null)
        {
            MainCamera.enabled = true;
        }
        else
        {
            Newcam.enabled = true;
            currentcam = Newcam;
        }
        Switched.Invoke();
        FadeAnim.SetBool("Faded", false);
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

    IEnumerator LerpTilt(Vector3 RotateTo, float LerpTime)
    {
        float Rate = 1.0f / LerpTime;
        float i = 0;

        Quaternion pos = MainCamera.transform.root.rotation;

        while (i < 1)
        {
            i += Time.deltaTime * Rate;
            pos = Quaternion.Lerp(pos, Quaternion.Euler(RotateTo.x, RotateTo.y, RotateTo.z), i);

            MainCamera.gameObject.transform.root.rotation = pos;
            yield return 0;
        }

        MainCamera.gameObject.transform.root.rotation = Quaternion.Euler(RotateTo.x, RotateTo.y, RotateTo.z);
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
        TargetRotation = Quaternion.LookRotation(LookTarget.transform.position - this.transform.GetChild(0).position, transform.GetChild(0).up);
        this.transform.GetChild(0).rotation = Quaternion.Slerp(this.transform.GetChild(0).rotation, Quaternion.Lerp(transform.GetChild(0).rotation, TargetRotation, LookLerpamount), LookLerpspeed * Time.deltaTime);
    }
}
