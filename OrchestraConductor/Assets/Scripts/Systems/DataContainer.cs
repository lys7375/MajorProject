using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    private string levelName;
    private int finalScore;
    private int maxHitChain;
    private int missHit;

    public DataContainer(string levelName, int finalScore, int maxHitChain, int missHit)
    {
        this.levelName = levelName;
        this.finalScore = finalScore;
        this.maxHitChain = maxHitChain;
        this.missHit = missHit;
    }
}
