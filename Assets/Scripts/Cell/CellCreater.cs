using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCreater : MonoBehaviour
{
    [Header("�� ������ �ʿ��� �ɼ�")]
    [SerializeField] GameObject cellPrefab;  // ������ Prefab
    [SerializeField] Transform cellParent;   // ������ ���� ���� ��ġ


    /// <summary>
    /// �� ���� & ������ ����
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
