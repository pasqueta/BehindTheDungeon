using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneAsync : MonoBehaviour {

    AsyncOperation async;

    [SerializeField]
    string sceneName = "SceneJeu";

    [SerializeField]
    GameObject textSkip;
    [SerializeField]
    GameObject imageLoading;

    bool enableSkip = false;

    [SerializeField]
    GameObject canvasEn;
    [SerializeField]
    GameObject canvasFr;

    void Start ()
    {
        if (DataManager.instance && DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS)
        {
            canvasFr.SetActive(true);
        }
        else
        {
            canvasEn.SetActive(true);
        }

        // async = SceneManager.LoadSceneAsync(sceneName);
        // async.allowSceneActivation = false;
        // async.priority = 0;
        StartCoroutine(LoadScene());
        //StartCoroutine(DottoDotto());
	}


    void Update()
    {
        // if (async != null)
        // {
        //     Debug.Log(async.progress);
        //     imageLoading.GetComponent<Image>().fillAmount = async.progress;
        // }

        // if (!(async.progress < 0.9))
        // {
        //     imageLoading.transform.parent.gameObject.SetActive(false);
        //     textSkip.transform.parent.gameObject.SetActive(true);
        //     enableSkip = true;
        // }

        if (enableSkip &&( Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("J1_Start") || Input.GetButtonDown("J2_Start") || Input.GetButtonDown("J1_A") || Input.GetButtonDown("J2_A")))
        {
            async.allowSceneActivation = true;
        }
    }


    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        async.priority = 0;

        while (async.progress <0.9f)
        {
            yield return null;
        }

        imageLoading.transform.parent.gameObject.SetActive(false);
        textSkip.transform.parent.gameObject.SetActive(true);
        enableSkip = true;

        FindObjectsOfType<AnimationImage>()[0].animated = false;
        FindObjectsOfType<AnimationImage>()[1].animated = false;
    }

    IEnumerator DottoDotto()
    {
        while (!enableSkip)
        {
            textSkip.GetComponent<Text>().text = DataManager.instance && DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS ? "Chargement " : "Loading ";
            yield return new WaitForSeconds(0.25f);
            textSkip.GetComponent<Text>().text = DataManager.instance && DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS ? "Chargement ." : "Loading .";
            yield return new WaitForSeconds(0.25f);
            textSkip.GetComponent<Text>().text = DataManager.instance && DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS ? "Chargement .." : "Loading ..";
            yield return new WaitForSeconds(0.25f);
            textSkip.GetComponent<Text>().text = DataManager.instance && DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS ? "Chargement ..." : "Loading ...";
            yield return new WaitForSeconds(0.25f);
        }
        textSkip.GetComponent<Text>().text = DataManager.instance && DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS ? "Passer" : "Skip";
    }

    public void EndLoading()
    {
        if (enableSkip)
        {
            async.allowSceneActivation = true;
        }
    }
}
