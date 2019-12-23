using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarMeshInverseUv : MonoBehaviour {

    GameObject InversedUVGameObject;

    private void Start()
    {
        InversedUVGameObject = GetComponentInChildren<MeshInverseUV>().gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (InversedUVGameObject)
        {
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (InversedUVGameObject)
        {
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
        }
       
    }
}
