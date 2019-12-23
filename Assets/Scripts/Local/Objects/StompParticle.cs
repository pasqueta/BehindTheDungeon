using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompParticle : MonoBehaviour
{
    public void OnEnable()
    {
        StartCoroutine(CoroutinePlayParticle());
    }

    IEnumerator CoroutinePlayParticle()
    {
        transform.parent = null;
        yield return new WaitForSeconds(1.2f);
        if (Boss.instance)
            transform.parent = Boss.instance.transform;
        else if (BossNet.instance)
            transform.parent = BossNet.instance.transform;
        transform.position = transform.parent.position + Vector3.up * 0.1f ;
        gameObject.SetActive(false);
    }
}
