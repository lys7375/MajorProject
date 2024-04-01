using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    // 定义方块的目标位置
    public Vector3 targetPosition = new Vector3(0, 0, 0); // 目标位置

    // 限制方块的下落速度为 5f
    public float fallSpeed = 0f; // 下落速度（单位距离/单位时间）

    // 指定方块的下落时间
    public float fallTime = 0;

    private bool flag = true;

    private bool gestureCheckFlag = true;

    // 淡出持续时间
    private float fadeDuration = 0.2f;
    // 淡出计时器
    private float fadeTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private bool fadeOutFlag = false;

    public float finalHeight = 0;

    // Note种类字典
    private Dictionary<string, string> noteType = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        // 计算Note的最终高度
        //finalHeight = fallSpeed * fallTime;

        // 设置Note的初始位置
        transform.position = new Vector3(transform.position.x, finalHeight, transform.position.y);

        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        //Debug.Log("GameObject: " + this.transform.name + " fallTime: " + fallTime + " finalHeight: " + finalHeight);

        noteType["Note_a"] = "Lleft";
        noteType["Note_s"] = "Ldown";
        noteType["Note_w"] = "Lup";
        noteType["Note_d"] = "Lright";

        noteType["Note_q"] = "LleftCrescendo";
        noteType["Note_e"] = "LleftDecrescendo";

        noteType["Note_j"] = "Rleft";
        noteType["Note_k"] = "Rdown";
        noteType["Note_i"] = "Rup";
        noteType["Note_l"] = "Rright";

        noteType["Note_u"] = "RrightCrescendo";
        noteType["Note_o"] = "RrightDecrescendo";
    }

    // Update is called once per frame
    void Update()
    {
        // 计算Note的下一个位置
        Vector3 nextPosition = transform.position + Vector3.down * fallSpeed * Time.deltaTime;

        // 更新Note的位置
        transform.position = nextPosition;

        if (flag == true && transform.position.y <= targetPosition.y)
        {
            //Debug.Log(gameObject.name + " arraves 0,0,0");
            flag = false;
            //Debug.Log("Sample time!!!!" + Koreographer.GetSampleTime());
        }

        // 如果Note到达了销毁位置
        if (transform.position.y <= -10)
        {
            //Debug.Log(gameObject.name + " has destroy");

            // 销毁Note
            Destroy(gameObject);
        }

        DetectGestureMatch();

        if(fadeOutFlag != false)
        {
            FadeOut();
        }
    }

    // 当音符位于检测区间时对玩家手势进行检测
    private void DetectGestureMatch()
    {
        // 位于检测区间
        if (transform.position.y < -2.5f && transform.position.y > -4.5f)
        {
            //Debug.Log("位于检测区间: " + transform.name);
            if (noteType[this.transform.name] == UDPCommunicator.leftHandDirection || noteType[this.transform.name] == UDPCommunicator.rightHandDirection)
                fadeOutFlag = true;
        }

        //if(gestureCheckFlag == true)
        //{
        //    // 位于检测区间
        //    if (transform.position.y < -2.5f && transform.position.y > -4.5f)
        //    {
        //        //Debug.Log("位于检测区间: " + transform.name);
        //        fadeOutFlag = true;
        //    }
        //}
    }

    // Note变淡消失
    private void FadeOut()
    {
        fadeTimer += Time.deltaTime;

        //Debug.Log("fadeTimer: " + fadeTimer + " | fadeDuration: " + fadeDuration);

        if (fadeTimer < fadeDuration)
        {
            float fadeAmount = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

            Color newColor = spriteRenderer.color;
            newColor.a = fadeAmount;
            spriteRenderer.color = newColor;
        }
        else
        {
            GameController.noteMaxNumber--;
            GameController.num++;
            Debug.Log("num: " + GameController.num);
            Destroy(gameObject);
        }
    }
}
