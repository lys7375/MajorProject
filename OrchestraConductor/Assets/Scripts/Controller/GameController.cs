using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static TreeEditor.TreeEditorHelper;

public class GameController : MonoBehaviour
{
    public static int noteMaxNumber = 0;
    public float delay = 3f;

    public GameObject curtain;
    // 渐变速度
    public float fadeSpeed = 0.5f;
    // 当前 alpha 值
    private float currentAlpha = 0f;

    public GameManger gameManager;

    // 定义 Note 的预制体
    public GameObject notePrefab;
    public GameObject leftArrowNote;
    public GameObject rightArrowNote;
    public GameObject upArrowNote;
    public GameObject downArrowNote;
    public GameObject crescendoNote;
    public GameObject decrescendoNote;

    // 限制方块的下落速度为 5f
    public float fallSpeed = 5f; // 下落速度（单位距离/单位时间）
    // 指定方块的下落时间为 11.025 秒
    //private float fallTime = 0;

    // 用于监听的事件ID，确保这与你在Koreographer中设置的事件ID匹配
    public string eventID;
    //public Koreography koreography;
    private Koreography koreography;
    Koreography playingKoreo;

    public Koreography koreography1;
    public Koreography koreography2;
    public Koreography koreography3;



    // Note生成坐标字典
    private Dictionary<string, Vector3> notePosition = new Dictionary<string, Vector3>();

    // Note种类字典
    private Dictionary<string, GameObject> noteType = new Dictionary<string, GameObject>();

    private List<string> noteNameList = new List<string>();
    //private List<float> noteFallTimeList = new List<float>();
    private List<float> noteHeightList = new List<float>();

    private SpriteRenderer spriteRenderer;

    private string sceneName;

    private bool dataSaveFlag = true;

    public GameObject Constant_Moderato_Audio;
    public GameObject Three_Tone_Composition_Audio;
    public GameObject A_Familiar_Sightand_Leisure_Audio;
    private string levelName;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = curtain.GetComponent<SpriteRenderer>();

        // 定义Note初始生成X,z坐标
        // 左手检测区
        notePosition["a"] = new Vector3 (-7f, -3.5f, 0); // left direction
        notePosition["s"] = new Vector3 (-5f, -3.5f, 0); // down direction
        notePosition["w"] = new Vector3 (-5f, -3.5f, 0); // up direction
        notePosition["d"] = new Vector3 (-3f, -3.5f, 0); // right direction
        // 特殊Note
        notePosition["q"] = new Vector3(-5f, -3.5f, 0); // crescendo  手背
        notePosition["e"] = new Vector3(-5f, -3.5f, 0); // decrescendo 手心

        // 右手检测区域
        notePosition["j"] = new Vector3(3f, -3.5f, 0); // left direction
        notePosition["k"] = new Vector3(5f, -3.5f, 0); // down direction
        notePosition["i"] = new Vector3(5f, -3.5f, 0); // up direction
        notePosition["l"] = new Vector3(7f, -3.5f, 0); // right direction
        // 特殊Note
        notePosition["u"] = new Vector3(5f, -3.5f, 0); // crescendo  手背
        notePosition["o"] = new Vector3(5f, -3.5f, 0); // decrescendo 手心

        // 定义Note种类
        noteType["a"] = leftArrowNote;
        noteType["s"] = downArrowNote;
        noteType["w"] = upArrowNote;
        noteType["d"] = rightArrowNote;
        noteType["q"] = crescendoNote;
        noteType["e"] = decrescendoNote;
        noteType["j"] = leftArrowNote;
        noteType["k"] = downArrowNote;
        noteType["i"] = upArrowNote;
        noteType["l"] = rightArrowNote;
        noteType["u"] = crescendoNote;
        noteType["o"] = decrescendoNote;

        // 获取当前活动的场景
        Scene currentScene = SceneManager.GetActiveScene();
        // 获取当前场景的名称
        sceneName = currentScene.name;

        Loadkoreography();

        // 注册事件监听器
        Koreographer.Instance.RegisterForEvents(eventID, OnMusicEvent);

        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

        Debug.Log("eventID: " + eventID);

        KoreographyTrackBase rhythmTrack = playingKoreo.GetTrackByID(eventID);
        List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

        noteMaxNumber = rawEvents.Count;

        for (int i = 0; i < rawEvents.Count; ++i)
        {
            KoreographyEvent evt = rawEvents[i];
            string eventName = evt.GetTextValue();
            float fallTime = (float)evt.StartSample / (float)playingKoreo.SampleRate;

            //fallTime = sampleRate;

            noteNameList.Add(eventName);
            noteHeightList.Add(fallSpeed * fallTime);
            GenerateNote(eventName, fallSpeed * fallTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("noteMaxNumber: " + noteMaxNumber);
        if (noteMaxNumber == 0)
        {
            StartCoroutine(DelayedExecution(delay));
        }
    }

    private void OnDestroy()
    {
        // 当对象被销毁时，注销事件监听器
        if (Koreographer.Instance != null)
        {
            Koreographer.Instance.UnregisterForEvents(eventID, OnMusicEvent);
            //Koreographer.Instance.RegisterForEventsWithTime(eventID, OnMusicEvent1);
        }
    }

    // 定义响应事件的方法
    void OnMusicEvent(KoreographyEvent koreoEvent)
    {
        //Debug.Log("Music event triggered: " + koreoEvent.GetTextValue());
    }

    // 生成音符
    void GenerateNote(string name, float height)
    {
        // 实例化Note预制体
        GameObject note = Instantiate(noteType[name], notePosition[name], Quaternion.identity);
        note.name = "Note_" + name;

        // 获取Note的Controller 组件
        NoteController noteController = note.GetComponent<NoteController>();
        // 设置NotePrefeb的参数
        noteController.finalHeight = height;
        noteController.fallSpeed = fallSpeed;
    }

    IEnumerator DelayedExecution(float delay)
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delay);

        FadeIn();

        yield return new WaitForSeconds(3);

        dataSaveFlag = true;

        SaveData();

        loadScene();
    }

    void FadeIn()
    {
        currentAlpha += fadeSpeed * Time.deltaTime;
        currentAlpha = Mathf.Clamp01(currentAlpha);

        // 计算从透明到白色的颜色渐变
        Color targetColor = Color.white;
        Color newColor = Color.Lerp(new Color(1, 1, 1, 0), targetColor, currentAlpha);

        // 更新精灵图的颜色
        spriteRenderer.color = newColor;

        if (currentAlpha >= 1f)
        {
            enabled = false;
        }
    }

    void SaveData()
    {
        string dataString = $"{levelName}|{GameManger.finalScore}|{GameManger.maxHitChain}|{GameManger.miss}";

        //Debug.Log(dataString);

        PlayerPrefs.SetString(levelName, dataString);
        PlayerPrefs.Save();

        if(levelName == "Constant_Moderato")
        {
            UpdateRecord("Constant_Moderato_Record", dataString);
        }
        else if(levelName == "A_Familiar_Sight_and_Leisure")
        {
            UpdateRecord("A_Familiar_Sight_and_Leisure_Record", dataString);
        }
        else
        {
            UpdateRecord("Three-Tone_Composition_Record", dataString);
        }
    }

    // 更新游戏记录
    void UpdateRecord(string key, string data)
    {
        Debug.Log("Update Record");

        if (PlayerPrefs.HasKey(key))
        {

            string record = PlayerPrefs.GetString(key);
            string[] dataPieces = record.Split('|');

            if (dataPieces.Length == 4) // 确保数据拆分正确
            {
                string sceneName = dataPieces[0];
                int finalScore = int.Parse(dataPieces[1]);
                int maxHitChain = int.Parse(dataPieces[2]);
                int miss = int.Parse(dataPieces[3]);

                sceneName = sceneName.Replace("_", " ");

                if (finalScore < GameManger.finalScore)
                {
                    PlayerPrefs.SetString(key, data);
                    PlayerPrefs.Save();
                }
            }
        }
        else
        {
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }
    }

    void loadScene()
    {
        string record = PlayerPrefs.GetString("Three-Tone_Composition_Record");
        Debug.Log("record: " + record);

        SceneManager.LoadScene("ResultScene");
    }

    // 初始化koreography
    void Loadkoreography()
    {
        levelName = LevelselectionSceneUIController.levelName;

        if (levelName == "Constant_Moderato")
        {
            koreography = koreography1;
            eventID = "ConstantModeratoTrack";
            Constant_Moderato_Audio.SetActive(true);
        }
        else if(levelName == "A_Familiar_Sight_and_Leisure")
        {
            Debug.Log("A_Familiar_Sight_and_Leisure run");
            koreography = koreography2;
            eventID = "AFamiliarSightandLeisureTrack";
            A_Familiar_Sightand_Leisure_Audio.SetActive(true);
        }
        else if( levelName == "Three-Tone_Composition")
        {
            koreography = koreography3;
            eventID = "ThreeToneCompositionTrack";
            Three_Tone_Composition_Audio.SetActive(true);
            AudioSource audioSource = Three_Tone_Composition_Audio.GetComponent<AudioSource>();
            audioSource.volume = 0.1f;
        }
        else
        {
            Debug.LogError("Error, koreography is load");
        }
    }
}
