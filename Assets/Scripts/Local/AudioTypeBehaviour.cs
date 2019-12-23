using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTypeBehaviour : MonoBehaviour {

    public enum AudioType
    {
        MUSIC,
        EFFECT,
        VOICE
    }

    public static bool voiceIsPlaying = false;

    [SerializeField]
    AudioType audioType;

	// Use this for initialization
	void Start ()
    {
        foreach (AudioSource item in GetComponents<AudioSource>())
        {
            DataManager.instance.AddAudioSource(audioType, item);
        }
        //DataManager.instance.AddAudioSource(audioType, GetComponent<AudioSource>());
    }

    public void ResetVoieIsPlaying()
    {
        voiceIsPlaying = false;
    }



}
