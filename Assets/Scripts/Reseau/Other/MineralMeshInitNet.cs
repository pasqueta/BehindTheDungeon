using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralMeshInitNet : MonoBehaviour
{
    [SerializeField]
    Material inversedMeshMaterial;

    MineralNet mineral;

	// Use this for initialization
	void Start ()
    {
        mineral = GetComponentInParent<MineralNet>();

        GameObject go = Instantiate(mineral.models[(int)mineral.material], transform.position, Quaternion.identity, transform);

        go.AddComponent<MeshInverseUV>();
        go.GetComponent<Renderer>().material = inversedMeshMaterial;
        go.GetComponent<MeshRenderer>().enabled = false;

    }
}
