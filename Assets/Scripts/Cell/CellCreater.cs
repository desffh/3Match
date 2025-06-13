using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCreater : MonoBehaviour
{
    [Header("셀 생성에 필요한 옵션")]
    [SerializeField] GameObject cellPrefab;  // 생성할 Prefab
    [SerializeField] Transform cellParent;   // 생성될 계층 구조 위치


    /// <summary>
    /// 셀 생성 & 데이터 주입
    /// </summary>
    public Cell SpawnCell(Vector3 position, CellData celldata, Vector3 scale)
    {
        GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, cellParent);

        Cell cell = cellObj.GetComponent<Cell>();
        
        cell.transform.localScale = scale;
        cell.Init(celldata);

        return cell;    
    }
}
