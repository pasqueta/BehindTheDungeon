using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundsManager : MonoBehaviour {

    [SerializeField]
    AudioSource sound;
    
	void Start () {
        sound.Play();
	}
	
	void Update () {
		
	}
}
