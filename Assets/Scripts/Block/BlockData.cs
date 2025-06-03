using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType
{
    EMPTY = 0,
    BASIC = 1,
}

[CreateAssetMenu(menuName = "BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField] BlockType blockType;
    [SerializeField] int num;
    [SerializeField] Sprite sprite;
    [SerializeField] Color color;

    public BlockType Blocktype => blockType;
    public int Num => num;
    public Sprite Sprite => sprite;

    public Color Color => color;
}
