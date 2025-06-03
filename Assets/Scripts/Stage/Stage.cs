using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public StageData stageData; // 나중에 읽기 전용으로 수정

    float time; // 제한 시간
    int score; // 목표 점수
    int cellX;
    int cellY;

    // 스테이지 입장 시 어드레서블로 SO받아오기?
    [SerializeField] private CellData cellData;

    [SerializeField] private List<BlockData> blockDataList;

    [SerializeField] Board board;

    private int random;

    // 스테이지 세팅 시 호출 -> stagedata 데이터 주입
    public void Init(StageData stageData)
    {
        if (stageData == null)
        {
            Debug.Log("스테이지 데이터 없음");
            return;
        }

        this.stageData = stageData;

        time = stageData.Time;
        score = stageData.Score;
        cellX = stageData.CellX;
        cellY = stageData.CellY;
    }

    // 스테이지 그리드 값에 따라 Board 생성 (n x n)
    public void SetupBoard()
    {
        board.Init(cellX, cellY);
        board.SpawnBoard(cellData, blockDataList);    // 오브젝트 생성 + 데이터 주입
    }
}
