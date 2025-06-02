using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType
{
    Empty,
    Basic,
}

[CreateAssetMenu(menuName = "BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField] BlockType blockType;
    [SerializeField] int num;
    [SerializeField] Sprite sprite;
    // 컬러 추가 예정

    public BlockType Blocktype => blockType;
    public int Num => num;
    public Sprite Sprite => sprite;
}
