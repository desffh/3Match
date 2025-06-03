using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CellType
{ 
    EMPTY = 0,
    BASIC = 1,  //배경있는 기본 형
}

// 이후 json 파일의 맵 추가

[CreateAssetMenu(menuName = "CellData")]
public class CellData : ScriptableObject
{
    [SerializeField] CellType cellType;
    public CellType CellType => cellType;

    [SerializeField] GameObject cellObject;
    public GameObject CellObject => cellObject;
}
