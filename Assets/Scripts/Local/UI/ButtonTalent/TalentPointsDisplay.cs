using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentPointsDisplay : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(GetComponentInParent<Lifebar>())
        {
            GetComponent<Text>().text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 114] + " : " + DataManager.instance.bossCurrentTalentPoints;
        }
        else
        {
            GetComponent<Text>().text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 114] + " : " + DataManager.instance.peonCurrentTalentPoints;
        }
    }
}
