using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public TMP_Text uiText;
    public TMP_Text hitChian;
    public TMP_Text missHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = "Score: " + GameManger.finalScore;
        hitChian.text = "HitChain: " + GameManger.maxHitChain;
        missHit.text = "Miss Hit: " + GameManger.miss;
    }
}
