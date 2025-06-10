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
    [Header("기본 속성")]
    [SerializeField] private BlockType blockType;
    [SerializeField] private int num;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Color color;


    [Header("특수 블록 스프라이트")]
    [SerializeField] private Sprite line4RowSprite;
    [SerializeField] private Sprite line4ColSprite;
    [SerializeField] private Sprite line5RowSprite;
    [SerializeField] private Sprite line5ColSprite;
    [SerializeField] private Sprite tShapeSprite;
    [SerializeField] private Sprite lShapeSprite;

    // === 프로퍼티 ===
    public BlockType Blocktype => blockType;
    public int Num => num;
    public Sprite Sprite => defaultSprite;
    public Color Color => color;

    /// <summary>
    /// 매치 타입에 따라 해당하는 스프라이트 반환
    /// </summary>
    public Sprite GetSpriteByMatchType(MatchType matchType)
    {
        return matchType switch
        {
            MatchType.Line4_Row => line4RowSprite,
            MatchType.Line4_Col => line4ColSprite,
            MatchType.Line5_Row => line5RowSprite,
            MatchType.Line5_Col => line5ColSprite,
            MatchType.TShape => tShapeSprite,
            MatchType.LShape => lShapeSprite,
            _ => defaultSprite,
        };
    }
}
