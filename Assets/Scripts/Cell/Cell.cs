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

    // 셀 데이터 주입
    // -> 셀 동적 생성 시 호출 (외부에서 Instantiate, 초기화만 여기서)
    public void Init(CellData celldata)
    {
        cellType = celldata.CellType;

        spriteRenderer.sprite = celldata.CellObject.GetComponent<SpriteRenderer>().sprite;
    }
}
