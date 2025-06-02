using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellType cellType { get; private set; }

    // �� ������ ����
    // -> �� ���� ���� �� ȣ�� (�ܺο��� Instantiate, �ʱ�ȭ�� ���⼭)
    public void Init(CellData celldata)
    {
        cellType = celldata.CellType;
    }
}
