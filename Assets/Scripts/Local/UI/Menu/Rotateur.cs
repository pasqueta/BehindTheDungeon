using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateur : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, 3.0f * Time.deltaTime);
	}
}
