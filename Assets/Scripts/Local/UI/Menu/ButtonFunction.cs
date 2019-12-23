using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonFunction : MonoBehaviour
{

    float timeMenuTransition = 0;
    RectTransform[] transformToRemove;

    [SerializeField]
    GameObject animationrouageMenu;

    [SerializeField]
    GameObject animationrouageMenu2;

    [SerializeField]
    GameObject canvasMenuMenu;

    [SerializeField]
    GameObject canvasMenuPlay;

    [SerializeField]
    GameObject canvasMenuOption;

    [SerializeField]
    GameObject canvasMenuAide;

    [SerializeField]
    GameObject canvasMenuSelectCharacter;

    [SerializeField]
    GameObject canvasMenuCredits;

    [SerializeField]
    EventSystem es;

    GameObject lobbyManager;


    GameObject currentCanvas;

    enum CANVASTYPE
    {
        Menu,
        Play,
        Option,
        SelectCharacter,
        Credits,
        Aide,
    }
    CANVASTYPE currentCanvasType = CANVASTYPE.Menu;
    bool remove = false;
    public bool invert = false;

    private void Start()
    {
        if (GameObject.Find("LobbyManager") != null)
        {
            lobbyManager = GameObject.Find("LobbyManager");
            lobbyManager.GetComponent<Canvas>().enabled = false;
        }

        CreateCanvasMenu(currentCanvasType);

        foreach (RectTransform t in transformToRemove)
        {
            if (t != null)
            {
                Vector3 pos = t.position;
                pos.y -= (Screen.height / 2 + 500);
                t.position = pos;
            }
        }

        PhysicsMenu(true, currentCanvasType);
    }



    public void onPlayOrOnReturnMenu()
    {

        currentCanvasType = currentCanvasType == CANVASTYPE.Menu ? CANVASTYPE.Play : CANVASTYPE.Menu;

        PhysicsMenu(false, currentCanvasType);
        timeMenuTransition = 3;

    }
    public void onSelectCharacterOrOnPlay()
    {

        currentCanvasType = currentCanvasType == CANVASTYPE.Play ? CANVASTYPE.SelectCharacter : CANVASTYPE.Play;

        PhysicsMenu(false, currentCanvasType);
        timeMenuTransition = 3;

    }

    public void LinkCrea()
    {
        // a enlever lorsqu'on est sur xbox
        Application.OpenURL("http://www.creajeux.fr");
    }


    public void LunchAnimation()
    {
        animationrouageMenu.GetComponent<AnimationImage>().animated = !animationrouageMenu.GetComponent<AnimationImage>().animated;
        animationrouageMenu2.GetComponent<AnimationImage>().animated = !animationrouageMenu2.GetComponent<AnimationImage>().animated;
    }

    void RevertAnimation()
    {
        animationrouageMenu.GetComponent<AnimationImage>().RevertAnimation();
        animationrouageMenu2.GetComponent<AnimationImage>().RevertAnimation();
    }

    public void onPlayTwoLocal()
    {
        if (lobbyManager != null)
        {
            lobbyManager.GetComponent<Canvas>().enabled = false;
        }
        SceneManager.LoadScene("SceneChargementLocal");
    }

    public void onPlayOnline()
    {
        if (lobbyManager != null)
        {
            lobbyManager.GetComponent<Canvas>().enabled = true;
        }
        SceneManager.LoadScene(2);
    }

    public void onOptionMenuOrOnReturnMenu()
    {
        currentCanvasType = currentCanvasType == CANVASTYPE.Menu ? CANVASTYPE.Option : CANVASTYPE.Menu;

        PhysicsMenu(false, currentCanvasType);
        timeMenuTransition = 3;
    }

    public void onOptionMenuOrOnCredits()
    {
        currentCanvasType = currentCanvasType == CANVASTYPE.Option ? CANVASTYPE.Credits : CANVASTYPE.Option;

        PhysicsMenu(false, currentCanvasType);
        timeMenuTransition = 3;
    }

    public void onOptionMenuOrOnHelp()
    {
        currentCanvasType = currentCanvasType == CANVASTYPE.Option ? CANVASTYPE.Aide : CANVASTYPE.Option;

        PhysicsMenu(false, currentCanvasType);
        timeMenuTransition = 3;
    }

    public void onQuitMenu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void Update()
    {

        if (timeMenuTransition > 0)
        {
            timeMenuTransition -= Time.deltaTime;
        }

        if (timeMenuTransition > 2.8f)
        {
            foreach (RectTransform t in transformToRemove)
            {
                if (t != null)
                {
                    Vector3 pos = t.position;
                    pos.y -= 450 * Time.deltaTime;
                    t.position = pos;
                }

            }

        }
        else if (timeMenuTransition > 2 && !invert)
        {

            invert = !invert;
            RevertAnimation();
        }
        else if (timeMenuTransition > 1.4f)
        {
            foreach (RectTransform t in transformToRemove)
            {
                if (t != null)
                {
                    Vector3 pos = t.position;
                    pos.y += ((Screen.height / 2 + 500 - 450 * 0.2f) / 1.4f) * Time.deltaTime;
                    t.position = pos;
                }

            }

        }
        else if (timeMenuTransition > 0 && !remove)
        {
            Destroy(currentCanvas);
            CreateCanvasMenu(currentCanvasType);
            RevertAnimation();
            remove = !remove;
            invert = !invert;

        }
        else if (timeMenuTransition > 0)
        {
            foreach (RectTransform t in transformToRemove)
            {
                if (t != null)
                {
                    Vector3 pos = t.position;
                    pos.y -= ((Screen.height / 2 + 500) / 1.4f) * Time.deltaTime;
                    t.position = pos;
                }

            }
        }
        else if (remove)
        {
            remove = !remove;
            LunchAnimation();
            PhysicsMenu(true, currentCanvasType);
        }
        else
        {
            if (Input.GetButtonDown("Menu_Cancel"))
            {
                switch (currentCanvasType)
                {
                    case CANVASTYPE.Play:
                        LunchAnimation();
                        onPlayOrOnReturnMenu();
                        break;
                    case CANVASTYPE.Option:
                        LunchAnimation();
                        onOptionMenuOrOnReturnMenu();
                        break;
                    case CANVASTYPE.SelectCharacter:
                        LunchAnimation();
                        onSelectCharacterOrOnPlay();
                        break;
                    case CANVASTYPE.Credits:
                        LunchAnimation();
                        onOptionMenuOrOnCredits();
                        break;
                    case CANVASTYPE.Aide:
                        LunchAnimation();
                        onOptionMenuOrOnHelp();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void CreateCanvasMenu(CANVASTYPE ct)
    {
        switch (ct)
        {
            case CANVASTYPE.Menu:
                currentCanvas = Instantiate(canvasMenuMenu, transform);
                break;
            case CANVASTYPE.Play:
                currentCanvas = Instantiate(canvasMenuPlay, transform);
                break;
            case CANVASTYPE.Option:
                currentCanvas = Instantiate(canvasMenuOption, transform);
                break;
            case CANVASTYPE.SelectCharacter:
                currentCanvas = Instantiate(canvasMenuSelectCharacter, transform);
                break;
            case CANVASTYPE.Credits:
                currentCanvas = Instantiate(canvasMenuCredits, transform);
                break;
            case CANVASTYPE.Aide:
                currentCanvas = Instantiate(canvasMenuAide, transform);
                break;
            default:
                break;
        }


        Canvas[] tmpC = GetComponentsInChildren<Canvas>();


        foreach (Canvas C in tmpC)
        {
            if (C.gameObject.activeSelf)
            {
                transformToRemove = C.GetComponentsInChildren<RectTransform>();
            }
        }
        for (int i = 0; i < transformToRemove.Length; i++)
        {
            if (transformToRemove[i].GetComponent<LetMeGo>())
            {
                if (transformToRemove[i].GetComponent<HingeJoint2D>())
                {
                    transformToRemove[i].GetComponent<HingeJoint2D>().enabled = false;
                    transformToRemove[i].GetComponent<Rigidbody2D>().simulated = false;
                }
                transformToRemove[i] = null;
            }
            //else if (transformToRemove[i].GetComponent<Text>())
            //{
            //    transformToRemove[i] = null;
            //}
            //else if (transformToRemove[i].GetComponent<HingeJoint2D>())
            //{
            //    transformToRemove[i].GetComponent<HingeJoint2D>().enabled = false;
            //    transformToRemove[i].GetComponent<Rigidbody2D>().simulated = false;
            //    transformToRemove[i] = null;
            //}
        }
        foreach (RectTransform t in transformToRemove)
        {
            if (t != null)
            {
                if (t.GetComponent<FixedJoint2D>())
                {
                    foreach (FixedJoint2D f in t.GetComponents<FixedJoint2D>())
                    {
                        f.enabled = false;
                    }

                    t.GetComponent<Rigidbody2D>().simulated = false;
                }

            }


        }

        transformToRemove[0] = null;

        foreach (RectTransform t in transformToRemove)
        {
            if (t != null)
            {
                Vector3 pos = t.position;
                pos.y += (Screen.height / 2 + 500);
                t.position = pos;
            }

        }
    }

    void PhysicsMenu(bool active, CANVASTYPE ct)
    {
        if (!active)
        {
            Button[] buttons = currentCanvas.GetComponentsInChildren<Button>();
            foreach (Button b in buttons)
            {
                b.onClick.RemoveAllListeners();
            }
        }
        else
        {
            Button[] buttons = currentCanvas.GetComponentsInChildren<Button>();
            //es.firstSelectedGameObject = buttons[0].gameObject;
            if (ct != CANVASTYPE.SelectCharacter)
            {
                es.SetSelectedGameObject(buttons[0].gameObject);
            }

            switch (ct)
            {

                case CANVASTYPE.Menu:

                    buttons[0].onClick.AddListener(onPlayOrOnReturnMenu);
                    buttons[0].onClick.AddListener(LunchAnimation);
                    buttons[1].onClick.AddListener(onOptionMenuOrOnReturnMenu);
                    buttons[1].onClick.AddListener(LunchAnimation);
                    buttons[2].onClick.AddListener(onQuitMenu);
                    break;
                case CANVASTYPE.Play:

                    buttons[0].onClick.AddListener(onSelectCharacterOrOnPlay);
                    buttons[0].onClick.AddListener(LunchAnimation);
                    buttons[1].onClick.AddListener(onPlayOnline);
                    buttons[2].onClick.AddListener(onPlayOrOnReturnMenu);
                    buttons[2].onClick.AddListener(LunchAnimation);
                    break;
                case CANVASTYPE.Option:
                    buttons[0].onClick.AddListener(onOptionMenuOrOnReturnMenu);
                    buttons[0].onClick.AddListener(LunchAnimation);
                    buttons[1].onClick.AddListener(onOptionMenuOrOnCredits);
                    buttons[1].onClick.AddListener(LunchAnimation);
                    buttons[2].onClick.AddListener(onOptionMenuOrOnHelp);
                    buttons[2].onClick.AddListener(LunchAnimation);

                    break;
                case CANVASTYPE.SelectCharacter:
                    buttons[0].onClick.AddListener(onSelectCharacterOrOnPlay);
                    buttons[0].onClick.AddListener(LunchAnimation);
                    es.firstSelectedGameObject = null;
                    break;
                case CANVASTYPE.Credits:
                    buttons[1].onClick.AddListener(onOptionMenuOrOnCredits);
                    buttons[1].onClick.AddListener(LunchAnimation);
                    break;

                case CANVASTYPE.Aide:
                    buttons[0].onClick.AddListener(onOptionMenuOrOnHelp);
                    buttons[0].onClick.AddListener(LunchAnimation);
                    break;
                default:
                    break;
            }



        }
        Canvas[] tmpC = GetComponentsInChildren<Canvas>();


        foreach (Canvas C in tmpC)
        {
            if (C.gameObject.activeSelf)
            {
                transformToRemove = C.GetComponentsInChildren<RectTransform>();
            }
        }
        for (int i = 0; i < transformToRemove.Length; i++)
        {
            if (transformToRemove[i] != null)
            {

                if (transformToRemove[i].GetComponent<LetMeGo>())
                {
                    if (transformToRemove[i].GetComponent<HingeJoint2D>())
                    {
                        transformToRemove[i].GetComponent<HingeJoint2D>().enabled = active;
                        transformToRemove[i].GetComponent<Rigidbody2D>().simulated = active;

                    }
                    transformToRemove[i] = null;
                }
                //else if (transformToRemove[i].GetComponent<Text>())
                //{
                //    transformToRemove[i] = null;
                //}
                //else if (transformToRemove[i].GetComponent<HingeJoint2D>())
                //{
                //    transformToRemove[i].GetComponent<HingeJoint2D>().enabled = active;
                //    transformToRemove[i].GetComponent<Rigidbody2D>().simulated = active;
                //    transformToRemove[i] = null;
                //}
            }
        }
        foreach (RectTransform t in transformToRemove)
        {
            if (t != null)
            {
                if (t.GetComponent<FixedJoint2D>())
                {
                    foreach (FixedJoint2D f in t.GetComponents<FixedJoint2D>())
                    {
                        f.enabled = active;
                    }

                    t.GetComponent<Rigidbody2D>().simulated = active;
                }
            }
        }
        transformToRemove[0] = null;
    }
}
