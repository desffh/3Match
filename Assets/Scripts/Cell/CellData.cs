using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CellType
{ 
    BASIC = 1  //����ִ� �⺻ ��
}

[CreateAssetMenu(menuName = "CellData")]
public class CellData : ScriptableObject
{
    [SerializeField] CellType cellType;
    public CellType CellType => cellType;

    [SerializeField] GameObject cellprefab;
    public GameObject CellPrefab => cellprefab;

    //[SerializeField] Sprite cellsprite;
    //public Sprite CellSprite { get { return cellsprite; } }
}
