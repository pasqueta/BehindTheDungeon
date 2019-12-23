using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BridgeNet : NetworkBehaviour
{
    // BOSS PART
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    GameObject modelPivot;
    [SerializeField]
    GameObject pivot;
    [SerializeField]
    RectTransform rectTransformBoss;
    [SerializeField]
    AudioSource bridgeReadySound;

    [SerializeField]
    Camera camBridge;

    [SerializeField]
    float rotFactor = 4.0f;
    [SerializeField]
    float levierFactor = 5.0f;

    [SerializeField]
    GameObject pillarInverseUv;

    // PEON PART
    [SerializeField]
    Jumper[] jumpPos;
    [SerializeField]
    RectTransform rectTransformPeon;
    [SerializeField]
    GameObject collidersOutBridge;
    [SerializeField]
    GameObject collidersOnBridge;
    [SerializeField]
    float jumpMarge = 10.0f;

    [SerializeField]
    float jumpTime = 1.5f;
    [SyncVar]
    float dtJump = 0.0f;

    int peonPos = -1;

    Vector3 startPos, endPos;
    
    float lastRot = 0.0f;
    [SyncVar]
    bool inUseByBoss = false;
    [SyncVar]
    bool inUseByPeon = false;
    [SyncVar]
    public bool enableJump = false;
    [SyncVar]
    bool onJump = false;

    private void Start()
    {
        // CAN JUMP OR NOT
        if ((transform.rotation.eulerAngles.y >= (90 - jumpMarge) && transform.rotation.eulerAngles.y <= (90 + jumpMarge)) || (transform.rotation.eulerAngles.y >= (270 - jumpMarge) && transform.rotation.eulerAngles.y <= (270 + jumpMarge)))
        {
            enableJump = true;
            if (!AudioTypeBehaviour.voiceIsPlaying)
            {
                bridgeReadySound.Play();
                AudioTypeBehaviour.voiceIsPlaying = true;
                bridgeReadySound.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", bridgeReadySound.clip.length);
            }
        }
        else
        {
            enableJump = false;
        }
    }

    void onJumpBool()
    {
        onJump = true;
    }

    private void Update()
    {
        // BOSS IS NEAR OF THE PILLAR
        if (pillarInverseUv.GetComponent<MeshRenderer>().enabled == true)
        {
            if (BossNet.instance.Controller == Controller.K1)
            {
                if (Input.GetButtonDown("K1_E"))
                {
                    if (!inUseByBoss)
                    {
                        GetComponent<SphereCollider>().radius = 2.0f;
                        canvas.SetActive(true);
                        SetCanvasBoss();
                        BossNet.instance.GetCameraController().EnableRotation = false;
                        pivot.transform.up = Input.mousePosition - pivot.transform.position;
                        lastRot = pivot.transform.rotation.eulerAngles.z;
                        inUseByBoss = true;
                        BossNet.instance.isBusy = true;

                        SetBossPosition();
                        BossNet.instance.GetComponent<Animator>().SetBool("useBridge", true);

                        camBridge.rect = BossNet.instance.GetCameraController().GetCameraScene().rect;
                        camBridge.gameObject.SetActive(true);

                        BossNet.instance.HideWeapon(true);
                    }
                    else
                    {
                        canvas.SetActive(false);
                        BossNet.instance.GetCameraController().EnableRotation = true;
                        inUseByBoss = false;
                        camBridge.gameObject.SetActive(false);
                        BossNet.instance.isBusy = false;
                        BossNet.instance.GetComponent<Animator>().SetBool("useBridge", false);
                        BossNet.instance.GetComponent<Animator>().speed = 1;

                        BossNet.instance.HideWeapon(false);
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown(BossNet.instance.Controller+"_A") && !inUseByBoss)
                {
                    canvas.SetActive(true);
                    SetCanvasBoss();
                    BossNet.instance.GetCameraController().EnableRotation = false;
                    pivot.transform.up = Input.mousePosition - pivot.transform.position;
                    lastRot = pivot.transform.rotation.eulerAngles.z;
                    inUseByBoss = true;
                    BossNet.instance.isBusy = true;
                    SetBossPosition();
                    BossNet.instance.GetComponent<Animator>().SetBool("useBridge", true);

                    camBridge.rect = BossNet.instance.GetCameraController().GetCameraScene().rect;
                    camBridge.gameObject.SetActive(true);

                    BossNet.instance.HideWeapon(true);
                }
                if (Input.GetButtonDown(BossNet.instance.Controller + "_B") && inUseByBoss)
                {
                    canvas.SetActive(false);
                    BossNet.instance.GetCameraController().EnableRotation = true;
                    inUseByBoss = false;
                    camBridge.gameObject.SetActive(false);
                    BossNet.instance.isBusy = false;
                    BossNet.instance.GetComponent<Animator>().SetBool("useBridge", false);
                    BossNet.instance.GetComponent<Animator>().speed = 1;

                    BossNet.instance.HideWeapon(false);
                }
            }
        }


        // PEON IS NEAR OF THE BRIDGE
        bool peonIsNear = false;
        for (int i = 0; i < 4; i++)
        {
            if (jumpPos[i].GetPeonIsNear())
            {
                peonPos = i;
                peonIsNear = true;
            }
        }

        if (!peonIsNear)
        {
            peonPos = -1;
        }
        else
        {
            if (enableJump)
            {
                CanvasManagerNet.instance.GetActionTextPeon().SetActive(true);
            }
            else
            {
                CanvasManagerNet.instance.GetActionTextPeon().SetActive(false);
            }
        }

        if (peonPos != -1 && enableJump && !onJump)
        {
            // set canvas peon

            if (PeonNet.instance.Controller == Controller.K1)
            {
                if (Input.GetButtonDown("K1_E"))
                {
                    PeonNet.instance.isBusy = true;
                    PeonNet.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    dtJump = 0.0f;
                    inUseByPeon = true;
                    startPos = PeonNet.instance.transform.position;
                    endPos = GetNearestPoint();
                    PeonNet.instance.GetComponent<Animator>().SetTrigger("Jump");
                    Invoke("onJumpBool", 0.5f);
                }
            }
            else
            {
                if (Input.GetButtonDown(PeonNet.instance.Controller+"_A"))
                {
                    PeonNet.instance.isBusy = true;
                    PeonNet.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    dtJump = 0.0f;
                    inUseByPeon = true;
                    startPos = PeonNet.instance.transform.position;
                    endPos = GetNearestPoint();
                    PeonNet.instance.GetComponent<Animator>().SetTrigger("Jump");
                    Invoke("onJumpBool", 0.5f);
                }
            }
        }
        else
        {
            // masqué canvas peon
        }
        
        // LE JUMP
        if (onJump)
        {
            if (PeonNet.instance.isLocalPlayer)
            {
                if (dtJump < jumpTime)
                {
                    Vector3 pos = Vector3.Lerp(startPos, endPos, dtJump / jumpTime);
                    pos.y += 2 * Mathf.Cos(dtJump / jumpTime);
                    PeonNet.instance.transform.position = pos;
                }
                else
                {
                    inUseByPeon = peonPos == 0 || peonPos == 1 ? false : true;
                    PeonNet.instance.isBusy = false;
                    onJump = false;
                }
                dtJump += Time.deltaTime;
            }
        }

        // BOX COLLIDERS
        if (inUseByPeon)
        {
            collidersOnBridge.SetActive(true);
            collidersOutBridge.SetActive(false);
        }
        else
        {
            collidersOnBridge.SetActive(false);
            collidersOutBridge.SetActive(true);
        }
    }

    public int GetPeonPos()
    {
        return peonPos;
    }

    public bool GetEnableJump()
    {
        return enableJump;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Boss"))
        {
            return;
        }
        
        CanvasManagerNet.instance.GetActionTextBoss().SetActive(true);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (!collider.CompareTag("Boss") || !inUseByBoss || inUseByPeon)
        {
            return;
        }

        BossNet.instance.nearOfInteractibleObject = true;

        if (BossNet.instance.Controller == Controller.K1)
        {
            if (Input.GetMouseButton(0))
            {
                modelPivot.transform.up = Input.mousePosition - modelPivot.transform.position;

                Vector2 m = modelPivot.transform.up;
                Vector2 p = -pivot.transform.up;

                float angle = Mathf.Acos(Vector2.Dot(m, p) / (m.magnitude * p.magnitude)) * 180.0f / Mathf.PI;

                if (180 - Mathf.Abs(angle) > levierFactor)
                {
                    if (m.x * p.y - p.x * m.y < 0)
                    {
                        pivot.transform.Rotate(Vector3.forward, -levierFactor);
                    }
                    else if (m.x * p.y - p.x * m.y > 0)
                    {
                        pivot.transform.Rotate(Vector3.forward, levierFactor);
                    }
                }
                else
                {
                    Vector3 rot = Vector3.zero;
                    rot.z = modelPivot.transform.rotation.eulerAngles.z;
                    pivot.transform.rotation = Quaternion.Euler(rot);
                }
            }
        }
        else // MANETTE
        {

            float x = Input.GetAxis(BossNet.instance.Controller + "_RXaxis");
            float y = Input.GetAxis(BossNet.instance.Controller + "_RYaxis");

            if (x != 0.0f || y != 0.0f)
            {
                Vector2 m = modelPivot.transform.up;
                Vector2 p = -pivot.transform.up;

                modelPivot.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(y, x) * Mathf.Rad2Deg + 90);
              
                float angle = 3.14f- Mathf.Acos(Vector2.Dot(m, p) / (m.magnitude * p.magnitude));
                if (angle > 0.2f)
                {
                    if (180 - Mathf.Abs(angle) > levierFactor)
                    {
                        if (m.x * p.y - p.x * m.y < 0)
                        {
                            pivot.transform.Rotate(Vector3.forward, -levierFactor);
                        }
                        else if (m.x * p.y - p.x * m.y > 0)
                        {
                            pivot.transform.Rotate(Vector3.forward, levierFactor);
                        }
                    }
                    else
                    {
                        Vector3 rot = Vector3.zero;
                        rot.z = modelPivot.transform.rotation.eulerAngles.z;
                        pivot.transform.rotation = Quaternion.Euler(rot);
                    }
                }
            }
        }

        // ROTATION PONT
        if (lastRot > 310.0f && pivot.transform.rotation.eulerAngles.z < 50.0f)
        {
            transform.Rotate(Vector3.up, (pivot.transform.rotation.eulerAngles.z - (lastRot - 360.0f)) / rotFactor);
        }
        else if (lastRot < 50.0f && pivot.transform.rotation.eulerAngles.z > 310.0f)
        {
            transform.Rotate(Vector3.up, ((360.0f - pivot.transform.rotation.eulerAngles.z) - lastRot) / rotFactor);
        }
        else
        {
            transform.Rotate(Vector3.up, (pivot.transform.rotation.eulerAngles.z - lastRot) / rotFactor);
        }
        
        // CAN JUMP OR NOT
        if ((transform.rotation.eulerAngles.y >= (90 - jumpMarge) && transform.rotation.eulerAngles.y <= (90 + jumpMarge)) || (transform.rotation.eulerAngles.y >= (270 - jumpMarge) && transform.rotation.eulerAngles.y <= (270 + jumpMarge)))
        {
            enableJump = true;
        }
        else
        {
            enableJump = false;
        }

        BossNet.instance.GetComponent<Animator>().speed = lastRot != pivot.transform.rotation.eulerAngles.z ? 1 : 0;
        
        if (!isServer)
        {
            BossNet.instance.CmdMoveBridge(gameObject, pivot.transform.rotation.eulerAngles.z, lastRot, enableJump, rotFactor, jumpMarge);
        }
        else
        {
            BossNet.instance.RpcMoveBridge(gameObject, pivot.transform.rotation.eulerAngles.z, lastRot, enableJump, rotFactor, jumpMarge);
        }

        lastRot = pivot.transform.rotation.eulerAngles.z;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.CompareTag("Boss"))
        {
            return;
        }
        BossNet.instance.nearOfInteractibleObject = false;
        canvas.SetActive(false);
        BossNet.instance.GetCameraController().EnableRotation = true;
        inUseByBoss = false;
        CanvasManagerNet.instance.GetActionTextBoss().SetActive(false);
        BossNet.instance.GetComponent<Animator>().speed = 1;
        GetComponent<SphereCollider>().radius = 5.0f;
    }

    void SetCanvasBoss()
    {
        switch (BossNet.instance.GetCameraController().screenOption)
        {
            case CameraController.ScreenMode.none:
                rectTransformBoss.position = new Vector3(-Screen.width, -Screen.height);
                return;
            case CameraController.ScreenMode.fullscreen:
                rectTransformBoss.position = new Vector3(Screen.width / 2, Screen.height / 3);
                return;
            case CameraController.ScreenMode.splitscreen_left:
                rectTransformBoss.position = new Vector3(Screen.width/4, Screen.height/3);    
                break;
            case CameraController.ScreenMode.splitscreen_right:
                rectTransformBoss.position = new Vector3(Screen.width *0.75f, Screen.height / 3);
                break;
            default: return;
        }
    }

    void SetCanvasPeon()
    {
        switch (PeonNet.instance.GetCameraController().screenOption)
        {
            case CameraController.ScreenMode.none:
                rectTransformPeon.position = new Vector3(-Screen.width, - Screen.height);
                return;
            case CameraController.ScreenMode.fullscreen:
                return;
            case CameraController.ScreenMode.splitscreen_left:
                rectTransformPeon.position = new Vector3(Screen.width / 4, 2 * Screen.height / 3);
                break;
            case CameraController.ScreenMode.splitscreen_right:
                rectTransformPeon.position = new Vector3(3 * Screen.width / 4, Screen.height / 3);
                break;
            default: return;
        }
    }

    Vector3 GetNearestPoint()
    {
        Vector3 pos = jumpPos[peonPos].transform.position;

        if (peonPos == 0 || peonPos == 1)
        {
           pos = Vector3.Distance(jumpPos[peonPos].transform.position, jumpPos[2].transform.position) < Vector3.Distance(jumpPos[peonPos].transform.position, jumpPos[3].transform.position) ? jumpPos[2].transform.position : jumpPos[3].transform.position;
        }
        else if (peonPos == 2 || peonPos == 3)
        {
            pos = Vector3.Distance(jumpPos[peonPos].transform.position, jumpPos[0].transform.position) < Vector3.Distance(jumpPos[peonPos].transform.position, jumpPos[1].transform.position) ? jumpPos[0].transform.position : jumpPos[1].transform.position;
        }

        return pos;
    }

    void SetBossPosition()
    {
        Vector3 dist = BossNet.instance.transform.position - transform.position;
        dist.y = 0;
        dist.Normalize();
        dist *= 2.5f;

        BossNet.instance.transform.position = new Vector3(transform.position.x, BossNet.instance.transform.position.y, transform.position.z) + dist;
        BossNet.instance.transform.LookAt(new Vector3(transform.position.x, BossNet.instance.transform.position.y, transform.position.z));
    }
}