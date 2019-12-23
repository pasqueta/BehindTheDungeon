using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CameraController : MonoBehaviour
{
    public static CameraController camBoss;
    public static CameraController camPeon;

    public enum ScreenMode
    {
        none,
        fullscreen,
        splitscreen_left,
        splitscreen_right
    }

    [SerializeField]
    LayerMask mask;

    [SerializeField]
    public ScreenMode screenOption;

    [SerializeField]
    Vector3 targetOffset;

    [SerializeField]
    Vector3 cameraPosition;

    [SerializeField]
    [Range(40, 120)]
    int fov = 60;

    [SerializeField]
    float near = 0.1f;
    [SerializeField]
    float far = 100;

    [SerializeField]
    Transform target;

    [SerializeField]
    Camera camScene;
    [SerializeField]
    Camera camUI;

    bool enableRotation = true;

    public Controller Ctrl { get; set; }

    public bool EnableRotation
    {
        get
        {
            return enableRotation;
        }

        set
        {
            enableRotation = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        if (gameObject.CompareTag("Boss"))
        {
            if (!camBoss)
                camBoss = this;
            else
                Destroy(gameObject);
        }

        if (gameObject.CompareTag("Peon"))
        {
            if (!camPeon)
                camPeon = this;

            else
                Destroy(gameObject);
        }

        //camScene = GetComponentInChildren<Camera>();

        SetCameraPreset();
    }

    // Update is called once per frame
    void Update()
    {

        if (target)
        {
            //transform.position = target.position + targetOffset;

            // RECALAGE
            Ray ray = new Ray(transform.position + targetOffset, transform.rotation * (cameraPosition));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, cameraPosition.magnitude, mask))
            {
                camScene.transform.position = hit.point;
            }
            else
            {
                camScene.transform.localPosition = cameraPosition + targetOffset;
            }

            // LOOK
            camScene.transform.LookAt(transform.position + targetOffset);

            // ROTATION
            if (EnableRotation)
            {
                
                if (Ctrl == Controller.K1 /*&& Input.GetMouseButton(1)*/)
                {
                    /*
                    if (WinLooseManager.instance != null)
                    {
                        if (!WinLooseManager.instance.gameIsAlreadyEnd)
                        {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked;
                        }
                    }
                    else if(WinLooseManagerNet.instance != null)
                    {
                        if (!WinLooseManagerNet.instance.gameIsAlreadyEnd)
                        {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked;
                        }
                    }
                    */
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    transform.Rotate(0, Input.GetAxis(Ctrl + "_MXaxis") * 4, 0);
                }
                else
                {
                    transform.Rotate(0, Input.GetAxisRaw(Ctrl + "_RXaxis") * -4, 0);
                }
            }
            else
            {
                if (Ctrl == Controller.K1)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }

            camScene.fieldOfView = fov;
            camScene.nearClipPlane = near;
            camScene.farClipPlane = far;
        }
        else
        {
            if (gameObject.CompareTag("Peon"))
            {
                if (PeonNet.instance)
                {
                    target = PeonNet.instance.transform;
                }
                if (!target && Peon.instance)
                {
                    target = Peon.instance.transform;
                }
            }

            if (gameObject.CompareTag("Boss"))
            {
                if (BossNet.instance)
                {
                    target = BossNet.instance.transform;
                }
                if (!target && Boss.instance)
                {
                    target = Boss.instance.transform;
                }
            }
        }
    }
    /*
    private Vector3 LerpPosition(Vector3 currentPos, Vector3 target)
    {
        Vector3 pos = currentPos;
        dtSmooth += Time.deltaTime;
        if (dtSmooth >= smoothTime)
        {
            isLerping = false;
            return target;
        }
        pos = Vector3.Lerp(currentPos, target, dtSmooth / smoothTime);
        return pos;
    }
    */
    public void SetCameraPreset()
    {
        //camScene.transform.position = positionCam;

        switch (screenOption)
        {
            case ScreenMode.none:
                camScene.rect = new Rect(0, 0, 0, 0);
                camUI.rect = new Rect(0, 0, 0, 0);
                break;
            case ScreenMode.splitscreen_left:
                camScene.rect = new Rect(0, 0, 0.5f, 1);
                camUI.rect = new Rect(0, 0, 0.5f, 1);
                break;
            case ScreenMode.splitscreen_right:
                camScene.rect = new Rect(0.5f, 0, 0.5f, 1);
                camUI.rect = new Rect(0.5f, 0, 0.5f, 1);
                break;
            case ScreenMode.fullscreen:
            default:
                camScene.rect = new Rect(0, 0, 1, 1);
                camUI.rect = new Rect(0, 0, 1, 1);
                break;
        }
    }

    public void SetTarget(Transform _t)
    {
        target = _t;
    }

    public Camera GetCameraScene()
    {
        return camScene;
    }

    public Camera GetCameraUI()
    {
        return camUI;
    }

    public void SetPause()
    {
        enableRotation = false;
    }

    public void SetPlay()
    {
        enableRotation = true;
    }
}
