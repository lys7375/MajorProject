using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    // 限制音符的下落速度
    public float fallSpeed = 0f;

    // 指定音符的下落时间
    public float fallTime = 0;

    // 音符生成高度
    public float finalHeight = 0;

    // 淡出持续时间
    private float fadeDuration = 0.2f;
    // 淡出计时器
    private float fadeTimer = 0f;

    private SpriteRenderer spriteRenderer;

    private bool fadeOutFlag = false;

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

        // 手势检测字典
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

        // 穿过检测区且没有被正确识别
        if (transform.position.y <= -4.51f)
        {
            //Debug.Log(gameObject.name + " has destroy");
            //GameManger.maxHitChain = 0;
            GameManger.chain = 0;
            //Debug.Log("Unhit!!!");
        }

        // Note到达销毁位置
        if (transform.position.y <= -6f)
        {
            GameManger.miss++;
            GameController.noteMaxNumber--;
            Destroy(gameObject);
        }

        // 手势检测
        DetectGestureMatch();

        // 淡出
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
    }

    // Note变淡消失
    private void FadeOut()
    {
        fadeTimer += Time.deltaTime;

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
            GameManger.chain++;
            GameManger.finalScore = GameManger.finalScore + GameManger.chain;

            if (GameManger.maxHitChain < GameManger.chain)
            {
                GameManger.maxHitChain = GameManger.chain;
            }

            Destroy(gameObject);
        }
    }
}
