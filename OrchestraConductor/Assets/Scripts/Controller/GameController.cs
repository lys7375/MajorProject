using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // 定义 Note 的预制体
    public GameObject notePrefab;
    // 定义方块的目标位置
    public Vector3 targetPosition = new Vector3(0, 0, 0); // 目标位置
    // 限制方块的下落速度为 5f
    public float fallSpeed = 5f; // 下落速度（单位距离/单位时间）
    // 指定方块的下落时间为 11.025 秒
    public float fallTime = 0;

    // 用于监听的事件ID，确保这与你在Koreographer中设置的事件ID匹配
    public string eventID;
    public Koreography koreography;
    Koreography playingKoreo;

    // Note生成坐标
    private Dictionary<string, Vector3> notePosition = new Dictionary<string, Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        // 定义Note初始生成X,z坐标
        // 左手检测区
        notePosition["a"] = new Vector3 (-6.5f, -3.5f, 0); // left direction
        notePosition["s"] = new Vector3 (-4.5f, -3.5f, 0); // down direction
        notePosition["w"] = new Vector3 (-4.5f, -3.5f, 0); // up direction
        notePosition["d"] = new Vector3 (-2.5f, -3.5f, 0); // right direction

        notePosition["q"] = new Vector3(-4.5f, -3.5f, 0); // emphasis 手背
        notePosition["e"] = new Vector3(-4.5f, -3.5f, 0); // decrescendo 手心

        // 右手检测区域
        notePosition["j"] = new Vector3(6.5f, -3.5f, 0); // left direction
        notePosition["k"] = new Vector3(4.5f, -3.5f, 0); // down direction
        notePosition["l"] = new Vector3(4.5f, -3.5f, 0); // up direction
        notePosition["i"] = new Vector3(2.5f, -3.5f, 0); // right direction

        notePosition["k"] = new Vector3(4.5f, -3.5f, 0); // emphasis 手背
        notePosition["l"] = new Vector3(4.5f, -3.5f, 0); // decrescendo 手心


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
            GenerateNote(payload, targetPosition, fallSpeed, fallTime);
            Debug.Log(payload + " | " + sampleRate);
            Debug.Log("mesure: " + koreography.GetSampleTimeFromMeasureTime(fallTime));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 获得当前Sample Time
        //Debug.Log("koreography.GetLatestSampleTime(): " + koreography.GetLatestSampleTime());
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
        Debug.Log("Music event triggered: " + koreoEvent.GetTextValue());
    }

    // 生成音符
    void GenerateNote(string name, Vector3 targetPos, float fallSpeed, float fallTime)
    {
        // 实例化 Note 预制体
        GameObject note = Instantiate(notePrefab, notePosition[name], Quaternion.identity);
        //GameObject note = Instantiate(notePrefab, new Vector3(-6.5f, -3.5f, 0), Quaternion.identity);
        note.name = "Cube_" + name;

        // 获取 Cube 的 CubeController 组件
        NoteController noteController = note.GetComponent<NoteController>();

        // 设置 CubeController 的参数
        noteController.targetPosition = targetPos;
        noteController.fallSpeed = fallSpeed;
        noteController.fallTime = fallTime;
    }
}
