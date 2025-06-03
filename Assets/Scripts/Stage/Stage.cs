using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public StageData stageData; // ���߿� �б� �������� ����

    float time; // ���� �ð�
    int score; // ��ǥ ����
    int cellX;
    int cellY;

    // �������� ���� �� ��巹����� SO�޾ƿ���?
    [SerializeField] private CellData cellData;

    [SerializeField] private List<BlockData> blockDataList;

    [SerializeField] Board board;

    private int random;

    // �������� ���� �� ȣ�� -> stagedata ������ ����
    public void Init(StageData stageData)
    {
        if (stageData == null)
        {
            Debug.Log("�������� ������ ����");
            return;
        }

        this.stageData = stageData;

        time = stageData.Time;
        score = stageData.Score;
        cellX = stageData.CellX;
        cellY = stageData.CellY;
    }

    // �������� �׸��� ���� ���� Board ���� (n x n)
    public void SetupBoard()
    {
        board.Init(cellX, cellY);
        board.SpawnBoard(cellData, blockDataList);    // ������Ʈ ���� + ������ ����
    }
}
