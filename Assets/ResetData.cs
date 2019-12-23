using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetData : MonoBehaviour {

	public void OnReset()
    {

        string[] names = System.Enum.GetNames(typeof(ButtonTalent.TalentsList));

        for (int i = 0; i < names.Length; i++)
        {
            PlayerPrefs.SetInt(names[i], 0);
        }
    }
}
