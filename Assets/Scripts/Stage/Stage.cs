using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] StageData stageData; // ���߿� �б� �������� ����
 
    [SerializeField] Board board;

    // �������� ���� �� ȣ�� -> stagedata ������ ����
    public void Init(StageData stageData)
    {
        if (stageData == null)
        {
            Debug.Log("�������� ������ ����");
            return;
        }

        this.stageData = stageData;
    }

    // �������� �׸��� ���� ���� Board �׸��� ���� (n x n)
    public void SetupBoard()
    {
        board.Init(stageData.CellX, stageData.CellY);
    }
}
