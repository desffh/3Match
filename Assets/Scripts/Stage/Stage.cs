using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] StageData stageData; // 나중에 읽기 전용으로 수정
 
    [SerializeField] Board board;

    // 스테이지 세팅 시 호출 -> stagedata 데이터 주입
    public void Init(StageData stageData)
    {
        if (stageData == null)
        {
            Debug.Log("스테이지 데이터 없음");
            return;
        }

        this.stageData = stageData;
    }

    // 스테이지 그리드 값에 따라 Board 그리드 세팅 (n x n)
    public void SetupBoard()
    {
        board.Init(stageData.CellX, stageData.CellY);
    }
}
