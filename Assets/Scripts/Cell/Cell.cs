using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellType cellType { get; private set; }

    public SpriteRenderer spriteRenderer { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // �� ������ ����
    // -> �� ���� ���� �� ȣ�� (�ܺο��� Instantiate, �ʱ�ȭ�� ���⼭)
    public void Init(CellData celldata)
    {
        cellType = celldata.CellType;

        spriteRenderer.sprite = celldata.CellObject.GetComponent<SpriteRenderer>().sprite;
    }
}
