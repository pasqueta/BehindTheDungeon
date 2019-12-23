using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour {

	List<Collider>colliders;
	List<Rigidbody>rigidbodies;

	void Start () 
	{
		colliders = new List<Collider> ();
		rigidbodies = new List<Rigidbody> ();

		foreach (Collider c in transform.GetComponentsInChildren<Collider>()) 
		{
			colliders.Add (c);
			c.enabled = false;
		}

		foreach (Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>()) 
		{
			rigidbodies.Add (rb);
			rb.isKinematic = true;
		}

	}

	public void SetActiveRagdoll(bool b)
	{
		for (int i = 0; i < colliders.Count; i++) 
		{
			colliders [i].enabled = b;
		}

		for (int i = 0; i < rigidbodies.Count; i++) 
		{
			rigidbodies[i].isKinematic = !b;
			if (!b)
				rigidbodies [i].velocity = Vector3.zero;
		}
	}
}
