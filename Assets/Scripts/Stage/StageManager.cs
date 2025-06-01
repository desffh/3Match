using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] int score;
    [SerializeField] int cellX;
    [SerializeField] int cellY;


    public void Init(StageData stageData)
    {
        this.time = stageData.Time;
        this.score = stageData.Score;
        this.cellX = stageData.CellX;
        this.cellY = stageData.CellY;
    }
}
