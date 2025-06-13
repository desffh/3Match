using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [ 블록 데이터 정보]
/// : 숫자
/// : 스프라이트
/// : 색상
/// : 블록 타입
/// </summary>

/// <summary>
/// [특수 블록이 되었을 경우]
/// : 특수 블록 타입
/// : 스프라이트
/// : 색상
/// </summary>

public class Block : MonoBehaviour
{
    /// <summary>
    /// 블럭이 가진 고유한 데이터 (주입)
    /// </summary>
    [Header("블럭 고유 데이터")]
    [SerializeField] private int num;                          // 고유 숫자
    [SerializeField] private BlockData blockdata;              // 블럭 데이터 SO
    public int Num {  get { return num; } }                    // 고유 숫자 반환용
    public BlockType blockType { get; private set; }           // 블럭 타입
    public SpriteRenderer spriteRenderer { get; private set; } // 블럭 스프라이트

    
    /// <summary>
    /// 블럭이 가진 특수 데이터 (타입만 주입)
    /// </summary>
    [SerializeField] private MatchType matchType;
    public MatchType MatchType => matchType;


    /// <summary>
    /// 블럭이 가진 고유한 좌표값 (Vector2Int)
    /// </summary>
    [SerializeField] Vector2Int boardPos;
    public Vector2Int BoardPos => boardPos;


    /// <summary>
    /// 블럭 애니메이션
    /// </summary>

    public BlockAnime Anime { get; private set; }


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Anime = GetComponent<BlockAnime>();
    }


    /// <summary>
    /// 블럭 데이터 주입 (default 데이터)
    /// -> 블럭 동적 생성 시 호출 (Instantiate는 외부에서, 초기화만 여기서)
    /// </summary>
    /// <param name="data">BlockDataManager에 저장된 데이터 중 랜덤</param>    
    public void Init(BlockData data)
    {
        num = data.Num;
        blockType = data.Blocktype;
        spriteRenderer.sprite = data.Sprite;
        spriteRenderer.color = data.Color;
        blockdata = data;
    }


    /// <summary>
    /// 블럭 특수 타입 주입
    /// </summary>
    /// <param name="matchType">매치된 타입</param>
    public void ApplySpecial(MatchType matchType)
    {
        if (blockdata == null) return;

        this.matchType = matchType;

        // 매치타입에 해당하는 특수 스프라이트 & 색상 데이터 적용
        Sprite specialSprite = blockdata.GetSpriteByMatchType(matchType);
        Color specialColor = blockdata.GetColorByMatchType(matchType);

        if (specialSprite != null)
        {
            spriteRenderer.sprite = specialSprite;
            spriteRenderer.color = specialColor;    
        }
    }


    /// <summary>
    /// 보드에서 가져온 좌표 지정 Vector2Int(x, y) 
    /// : 2차원 배열 상에선 [y][x]
    /// </summary>
    /// <param name="x">행</param>
    /// <param name="y">열</param>
    public void SetBoardPos(int x, int y)
    {
        boardPos = new Vector2Int(x, y);
    } 
}
