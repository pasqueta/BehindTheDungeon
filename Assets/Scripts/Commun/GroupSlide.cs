using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSlide : MonoBehaviour {

    public bool loop = true;

    public bool autoNextSlide = false;
    public float timePerSlide = 1.5f;
    float currentTime = 0.0f;

    public bool loadSceneAtTheEnd = true;
    public LoadSceneAsync loadSceneAsync;

    List<GameObject> slides;
    int index = 0;

	void Start ()
    {
        slides = new List<GameObject>();

        for(int i = 0; i < transform.childCount; i++)
        {
            slides.Add(transform.GetChild(i).gameObject);
            transform.GetChild(i).gameObject.SetActive(false);
        }
        slides[index].SetActive(true);
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetButtonDown("J1_RB") || Input.GetButtonDown("J2_RB"))
        {
            slides[index].SetActive(false);

            index++;
            if (index >= slides.Count)
                if (loop)
                    index = 0;
                else
                    index--;
            currentTime = 0.0f;
            slides[index].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetButtonDown("J1_LB") || Input.GetButtonDown("J2_LB"))
        {
            slides[index].SetActive(false);
            index--;
            if (index < 0)
                if (loop)
                    index = slides.Count - 1;
                else
                    index = 0;
            currentTime = 0.0f;
            slides[index].SetActive(true);
        }

        if (autoNextSlide)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timePerSlide)
            {
                currentTime = 0.0f;
                slides[index].SetActive(false);

                index++;
                if (index >= slides.Count)
                {
                    if (loop)
                        index = 0;
                    else
                        index--;

                    if (loadSceneAtTheEnd)
                    {
                        loadSceneAsync.EndLoading();
                    }
                }

                slides[index].SetActive(true);
            }
        }
    }
}
