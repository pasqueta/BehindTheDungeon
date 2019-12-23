using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtTarget : MonoBehaviour {

    enum Target
    {
        Boss,
        Peon
    }
    [SerializeField]
    Target target;
	
	// Update is called once per frame
	void Update () {
        if (target == Target.Boss)
        {
            if (Boss.instance != null)
            {
                transform.rotation = Boss.instance.GetCameraController().GetCameraScene().transform.rotation;
            }
            else if(BossNet.instance != null)
            {
                transform.rotation = BossNet.instance.GetCameraController().GetCameraScene().transform.rotation;
            }
        }
        if (target == Target.Peon)
        {
            if (Peon.instance != null)
            {
                transform.rotation = Peon.instance.GetCameraController().GetCameraScene().transform.rotation;
            }
            else if (PeonNet.instance != null)
            {
                transform.rotation = PeonNet.instance.GetCameraController().GetCameraScene().transform.rotation;
            }
        }
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = 0.0f;
        transform.rotation = Quaternion.Euler(rot);
    }
}
