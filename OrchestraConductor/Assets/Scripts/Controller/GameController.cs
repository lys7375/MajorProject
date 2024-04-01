using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class GameController : MonoBehaviour
{
    public static int noteMaxNumber = 0;
    public static int num = 0;

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
    public float fallTime = 0;

    // 用于监听的事件ID，确保这与你在Koreographer中设置的事件ID匹配
    public string eventID;
    public Koreography koreography;
    Koreography playingKoreo;

    // Note生成坐标字典
    private Dictionary<string, Vector3> notePosition = new Dictionary<string, Vector3>();

    // Note种类字典
    private Dictionary<string, GameObject> noteType = new Dictionary<string, GameObject>();

    private List<string> noteNameList = new List<string>();
    //private List<float> noteFallTimeList = new List<float>();
    private List<float> noteHeightList = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
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



        // 注册事件监听器
        Koreographer.Instance.RegisterForEvents(eventID, OnMusicEvent);

        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

        KoreographyTrackBase rhythmTrack = playingKoreo.GetTrackByID(eventID);
        List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

        for (int i = 0; i < rawEvents.Count; ++i)
        {
            KoreographyEvent evt = rawEvents[i];
            string payload = evt.GetTextValue();
            float sampleRate = (float)evt.StartSample / (float)playingKoreo.SampleRate;

            fallTime = sampleRate;

            noteNameList.Add(payload);
            noteHeightList.Add(fallSpeed * sampleRate);
            GenerateNote(payload, fallSpeed * sampleRate);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
}
