using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralMeshInit : MonoBehaviour {

    [SerializeField]
    Material inversedMeshMaterial;

    Mineral mineral;

	// Use this for initialization
	void Start ()
    {
        mineral = GetComponentInParent<Mineral>();

        GameObject go = Instantiate(mineral.models[(int)mineral.material], transform.position, Quaternion.identity, transform);

        go.AddComponent<MeshInverseUV>();
        go.GetComponent<Renderer>().material = inversedMeshMaterial;
        go.GetComponent<MeshRenderer>().enabled = false;

    }
	
}
