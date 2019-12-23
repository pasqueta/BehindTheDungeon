using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeModifier : MonoBehaviour {

    [SerializeField]
    Slider music;
    [SerializeField]
    Slider effect;
    [SerializeField]
    Slider voice;

    // Use this for initialization
    void Start ()
    {
        music.value = PlayerPrefs.GetFloat("MusicVolume");
        effect.value = PlayerPrefs.GetFloat("EffectVolume");
        voice.value = PlayerPrefs.GetFloat("VoiceVolume");

        music.onValueChanged.AddListener(delegate { ValueChangeMusic(); });
        effect.onValueChanged.AddListener(delegate { ValueChangeEffect(); });
        voice.onValueChanged.AddListener(delegate { ValueChangeVoice(); });
    }

    void ValueChangeMusic()
    {
        DataManager.instance.UpdateMusicVolume(music.value);
    }

    void ValueChangeEffect()
    {
        DataManager.instance.UpdateEffectVolume(effect.value);
    }

    void ValueChangeVoice()
    {
        DataManager.instance.UpdateVoiceVolume(voice.value);
    }
}
