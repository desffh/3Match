using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CellType
{ 
    EMPTY = 0,
    BASIC = 1,  //����ִ� �⺻ ��
}

// ���� json ������ �� �߰�

[CreateAssetMenu(menuName = "CellData")]
public class CellData : ScriptableObject
{
    [SerializeField] CellType cellType;
    public CellType CellType => cellType;

    [SerializeField] GameObject cellObject;
    public GameObject CellObject => cellObject;
}
