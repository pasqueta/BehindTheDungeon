using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownArrow : MonoBehaviour {

    [SerializeField]
    GameObject Arrow;

    void Start()
    {
        Arrow.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 180));
    }

    void OnDestroy()
    {
        Arrow.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 180));
    }
}
