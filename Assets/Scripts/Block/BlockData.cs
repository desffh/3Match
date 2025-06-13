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
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Color color;
    [SerializeField] private int num;


    [Header("특수 블록 속성 스프라이트")]
    [SerializeField] private Sprite line4RowSprite;
    [SerializeField] private Sprite line4ColSprite;
    [SerializeField] private Sprite line5RowSprite;
    [SerializeField] private Sprite line5ColSprite;
    [SerializeField] private Sprite TShapeSprite;
    [SerializeField] private Sprite LShapeSprite;

    [Header("특수 블록 속성 색상")]
    [SerializeField] private Color line4Rowspecialcolor;
    [SerializeField] private Color line4Colspecialcolor;
    [SerializeField] private Color line5Rowspecialcolor;
    [SerializeField] private Color line5Colspecialcolor;
    [SerializeField] private Color Tspecialcolor;
    [SerializeField] private Color Lspecialcolor;


    // === 프로퍼티 ===
    public BlockType Blocktype => blockType;
    public int Num => num;
    public Sprite Sprite => defaultSprite;
    public Color Color => color;

    /// <summary>
    /// 매치 타입에 따라 해당하는 스프라이트 반환
    /// </summary>
    /// <param name="matchType"> 병합된 타입              </param>
    /// <returns>                특수 블럭 Sprite         </returns>
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
    /// 매치 타입에 따라 해당하는 색상 반환
    /// </summary>
    /// <param name="matchType"> 병합된 타입      </param>
    /// <returns>                특수 블럭 Color  </returns>
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
            _ => Color, // 3매치일 경우 일반 색상
        };
    }
}
