using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioKey : MonoBehaviour
{

    [SerializeField]
    int key = 0;

    // Use this for initialization
    void Start()
    {
        ChangeSound();
    }

    // Update is called once per frame
    public void ChangeSound()
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.clip = Resources.Load(DataManager.instance.myTab[(int)DataManager.instance.language + 1, key]) as AudioClip;
    }

    public void SetKey(int value)
    {
        key = value;
        ChangeSound();
    }
}
