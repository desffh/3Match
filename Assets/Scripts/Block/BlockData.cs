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
    [Header("�⺻ �Ӽ�")]
    [SerializeField] private BlockType blockType;
    [SerializeField] private int num;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Color color;


    [Header("Ư�� ��� ��������Ʈ")]
    [SerializeField] private Sprite line4RowSprite;
    [SerializeField] private Sprite line4ColSprite;
    [SerializeField] private Sprite line5RowSprite;
    [SerializeField] private Sprite line5ColSprite;
    [SerializeField] private Sprite tShapeSprite;
    [SerializeField] private Sprite lShapeSprite;

    // === ������Ƽ ===
    public BlockType Blocktype => blockType;
    public int Num => num;
    public Sprite Sprite => defaultSprite;
    public Color Color => color;

    /// <summary>
    /// ��ġ Ÿ�Կ� ���� �ش��ϴ� ��������Ʈ ��ȯ
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
