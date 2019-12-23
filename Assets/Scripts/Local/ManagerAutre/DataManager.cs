using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.EventSystems;

public class DataManager : MonoBehaviour
{
    public enum LANGUAGE
    {
        ENGLISH,
        FRANCAIS,       
    }

    public LANGUAGE language = LANGUAGE.ENGLISH;

    public TextAsset csvFile;

    public string[,] myTab;

    public static DataManager instance;

    [SerializeField] CameraController.ScreenMode screenModePeon;
    [SerializeField] Controller controllerPeon;
    [SerializeField] CameraController.ScreenMode screenModeBoss;
    [SerializeField] Controller controllerBoss;
    [SerializeField] GameObject canvasOptions;
    [SerializeField] EventSystem eventSystem;

    [SerializeField]
    GameObject canvasOptionsNet;

    //pause
    [HideInInspector]
    public CursorLockMode previousMode;
    GameObject instanceCanvas = null;

    [HideInInspector]
    public bool isUiActive;

    [HideInInspector]
    public bool destroyTheOptionCanvas = false;

    [HideInInspector]
    public int peonMaxTalentPoints = 0;
    [HideInInspector]
    public int bossMaxTalentPoints = 0;
    [HideInInspector]
    public int peonCurrentTalentPoints = 0;
    [HideInInspector]
    public int bossCurrentTalentPoints = 0;

    //manches
    public int currentManche = 0;
    public int nbManches = 2;


    // Sons
    private List<AudioSource> sourceMusic;
    private List<AudioSource> sourceEffect;
    private List<AudioSource> sourceVoice;

    int peonTalentMovementSpeed;
    int peonTalentCraftingSpeed;
    int peonTalentInventory;
    int peonTalentGatheringSpeed;
    int peonTalentPickaxe;

    int bossTalentMovementSpeed;
    int bossTalentTourbilol;
    int bossTalentDash;
    int bossTalentAttack;
    int bossTalentHealth;

    void Start()
    {
        if (instance)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Destroy(instance.gameObject);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        else
            instance = this;

        // Créer les clés des différents type de volumes si elles n'existent pas déjà.
        // Ces dites clés sont récupérées dans :    - Les options pour que les sliders portant les mêmes noms que les clés précédement citées affiche les bonnes valeurs.
        //                                          - Les scripts AudioTypeBehaviour pour assigner la bonne valeur au AudioSource à l'instanciation des objets.
        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        if (!PlayerPrefs.HasKey("EffectVolume"))
            PlayerPrefs.SetFloat("EffectVolume", 0.5f);
        if (!PlayerPrefs.HasKey("VoiceVolume"))
            PlayerPrefs.SetFloat("VoiceVolume", 0.5f);


        sourceMusic = new List<AudioSource>();
        sourceEffect = new List<AudioSource>();
        sourceVoice = new List<AudioSource>();

        //Load File With text
        myTab = SplitCsvGrid(csvFile.text);     


        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("peonTalentMovementSpeed"))
        {
            PlayerPrefs.SetInt("peonTalentMovementSpeed", 0);
        }

        if (!PlayerPrefs.HasKey("peonTalentPickAxe"))
        {
            PlayerPrefs.SetInt("peonTalentPickAxe", 0);
        }

        if (!PlayerPrefs.HasKey("peonTalentCraftingSpeed"))
        {
            PlayerPrefs.SetInt("peonTalentCraftingSpeed", 0);
        }

        if (!PlayerPrefs.HasKey("peonTalentInventory"))
        {
            PlayerPrefs.SetInt("peonTalentInventory", 0);
        }

        if (!PlayerPrefs.HasKey("peonTalentGatheringSpeed"))
        {
            PlayerPrefs.SetInt("peonTalentGatheringSpeed", 0);
        }

        if (!PlayerPrefs.HasKey("bossTalentMovementSpeed"))
        {
            PlayerPrefs.SetInt("bossTalentMovementSpeed", 0);
        }

        if (!PlayerPrefs.HasKey("bossTalentHealth"))
        {
            PlayerPrefs.SetInt("bossTalentHealth", 0);
        }

        if (!PlayerPrefs.HasKey("bossTalentAttack"))
        {
            PlayerPrefs.SetInt("bossTalentAttack", 0);
        }

        if (!PlayerPrefs.HasKey("bossTalentWhirlwind"))
        {
            PlayerPrefs.SetInt("bossTalentWhirlwind", 0);
        }

        if (!PlayerPrefs.HasKey("bossTalentDash"))
        {
            PlayerPrefs.SetInt("bossTalentDash", 0);
        }

    }

    // splits a CSV file into a 2D string array
    static public string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);

        // finds the max width of row
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[width + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];

                // This line was to replace "" with " in my output. 
                // Include or edit it as you wish.
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }

    // splits a CSV row 
    static public string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }

    private void Update()
    {
        //pause
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("J1_Start") || Input.GetButtonDown("J2_Start"))
            && !instanceCanvas && SceneManager.GetActiveScene().name != "SceneMenuAvecDonjonEnfond" && SceneManager.GetActiveScene().name != "Lobby" && !isUiActive)
            {
                InstantiatePause();
            }

        if (Boss.instance)
        {
            if (isUiActive && ((Input.GetKeyDown(KeyCode.Escape) && Boss.instance.Controller == Controller.K1)
                || (Input.GetButtonDown("J1_Start") && Boss.instance.Controller == Controller.J1) || (Input.GetButtonDown("J2_Start") && Boss.instance.Controller == Controller.J2))
                && !instanceCanvas && SceneManager.GetActiveScene().name != "SceneMenuAvecDonjonEnfond" && SceneManager.GetActiveScene().name != "Lobby")
            {
                InstantiatePause();
                eventSystem.SetSelectedGameObject(canvasOptions.transform.GetChild(3).gameObject);
            }
        }
        else if (BossNet.instance)
        {
            if (isUiActive && ((Input.GetKeyDown(KeyCode.Escape) && BossNet.instance.Controller == Controller.K1)
                || (Input.GetButtonDown("J1_Start") && BossNet.instance.Controller == Controller.J1) || (Input.GetButtonDown("J2_Start") && BossNet.instance.Controller == Controller.J2))
                && !instanceCanvas && SceneManager.GetActiveScene().name != "SceneMenuAvecDonjonEnfond" && SceneManager.GetActiveScene().name != "Lobby")
            {
                InstantiatePause();
                eventSystem.SetSelectedGameObject(canvasOptions.transform.GetChild(3).gameObject);
            }
        }

        //if (SceneManager.GetActiveScene().name == "Lobby")
        //{
        //    Cursor.lockState = CursorLockMode.Confined;
        //    Cursor.visible = true;
        //}

    }

    private void InstantiatePause()
    {
        if (WinLooseManager.instance)
        {
            if (!WinLooseManager.instance.gameIsAlreadyEnd)
            {
                instanceCanvas = Instantiate(canvasOptions);

                if (Peon.instance != null)
                {
                    SetPause();
                }
            }
        }
        else if (WinLooseManagerNet.instance)
        {
            if (!WinLooseManagerNet.instance.gameIsAlreadyEnd)
            {
                instanceCanvas = Instantiate(canvasOptionsNet);
            }
        }
    }
    public void NextManche()
    {
        currentManche++;

        CameraController.ScreenMode tmpSM = screenModePeon;
        screenModePeon = screenModeBoss;
        screenModeBoss = tmpSM;

        Controller tmpC = controllerPeon;
        controllerPeon = controllerBoss;
        controllerBoss = tmpC;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void SetPause()
    {
        Peon.instance.isBusy = true;
        Boss.instance.isBusy = true;
        CameraController.camBoss.EnableRotation = false;
        CameraController.camPeon.EnableRotation = false;

        Time.timeScale = 0.0f;
    }

    public Controller ControllerPeon { get { return controllerPeon; } set { controllerPeon = value; } }
    public Controller ControllerBoss { get { return controllerBoss; } set { controllerBoss = value; } }
    public CameraController.ScreenMode ScreenModePeon { get { return screenModePeon; } set { screenModePeon = value; } }
    public CameraController.ScreenMode ScreenModeBoss { get { return screenModeBoss; } set { screenModeBoss = value; } }

    public void AddAudioSource(AudioTypeBehaviour.AudioType type, AudioSource audioSource)
    {
        switch (type)
        {
            case AudioTypeBehaviour.AudioType.MUSIC:
                sourceMusic.Add(audioSource);
                audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
                break;
            case AudioTypeBehaviour.AudioType.EFFECT:
                sourceEffect.Add(audioSource);
                audioSource.volume = PlayerPrefs.GetFloat("EffectVolume");
                break;
            case AudioTypeBehaviour.AudioType.VOICE:
                sourceVoice.Add(audioSource);
                audioSource.volume = PlayerPrefs.GetFloat("VoiceVolume");
                break;
            default:
                break;
        }
    }

    public void UpdateMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        sourceMusic.RemoveAll(item => item == null);
        foreach (AudioSource source in sourceMusic)
        {
            source.volume = value;
        }
    }

    public void UpdateEffectVolume(float value)
    {
        PlayerPrefs.SetFloat("EffectVolume", value);
        sourceMusic.RemoveAll(item => item == null);
        foreach (AudioSource source in sourceEffect)
        {
            source.volume = value;
        }
    }

    public void UpdateVoiceVolume(float value)
    {
        PlayerPrefs.SetFloat("VoiceVolume", value);
        sourceVoice.RemoveAll(item => item == null);
        foreach (AudioSource source in sourceVoice)
        {
            source.volume = value;
        }
    }
}
