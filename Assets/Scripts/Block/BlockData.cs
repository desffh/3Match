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
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Color color;
    [SerializeField] private int num;


    [Header("Ư�� ��� �Ӽ� ��������Ʈ")]
    [SerializeField] private Sprite line4RowSprite;
    [SerializeField] private Sprite line4ColSprite;
    [SerializeField] private Sprite line5RowSprite;
    [SerializeField] private Sprite line5ColSprite;
    [SerializeField] private Sprite TShapeSprite;
    [SerializeField] private Sprite LShapeSprite;

    [Header("Ư�� ��� �Ӽ� ����")]
    [SerializeField] private Color line4Rowspecialcolor;
    [SerializeField] private Color line4Colspecialcolor;
    [SerializeField] private Color line5Rowspecialcolor;
    [SerializeField] private Color line5Colspecialcolor;
    [SerializeField] private Color Tspecialcolor;
    [SerializeField] private Color Lspecialcolor;


    // === ������Ƽ ===
    public BlockType Blocktype => blockType;
    public int Num => num;
    public Sprite Sprite => defaultSprite;
    public Color Color => color;

    /// <summary>
    /// ��ġ Ÿ�Կ� ���� �ش��ϴ� ��������Ʈ ��ȯ
    /// </summary>
    /// <param name="matchType"> ���յ� Ÿ��              </param>
    /// <returns>                Ư�� �� Sprite         </returns>
    public Sprite GetSpriteByMatchType(MatchType matchType)
    {
        return matchType switch
        {
            MatchType.Line4_Row => line4RowSprite,
            MatchType.Line4_Col => line4ColSprite,
            MatchType.Line5_Row => line5RowSprite,
            MatchType.Line5_Col => line5ColSprite,
            MatchType.TShape => TShapeSprite,
            MatchType.LShape => LShapeSprite,
            _ => defaultSprite,
        };
    }


    /// <summary>
    /// ��ġ Ÿ�Կ� ���� �ش��ϴ� ���� ��ȯ
    /// </summary>
    /// <param name="matchType"> ���յ� Ÿ��      </param>
    /// <returns>                Ư�� �� Color  </returns>
    public Color GetColorByMatchType(MatchType matchType)
    {
        return matchType switch
        {
            MatchType.Line4_Row => line4Rowspecialcolor,
            MatchType.Line4_Col => line4Colspecialcolor,
            MatchType.Line5_Row => line5Rowspecialcolor,
            MatchType.Line5_Col => line5Colspecialcolor,
            MatchType.TShape => Tspecialcolor,
            MatchType.LShape => Lspecialcolor,
            _ => Color, // 3��ġ�� ��� �Ϲ� ����
        };
    }
}
