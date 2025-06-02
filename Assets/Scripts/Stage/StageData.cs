using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StageData")]
public class StageData : ScriptableObject
{
    [SerializeField] float time; // 제한 시간
    [SerializeField] int score; // 목표 점수

    [SerializeField] int cellX;
    [SerializeField] int cellY;

    public float Time => time;  
    public int Score => score;

    public int CellX => cellX;
    public int CellY => cellY;
}
