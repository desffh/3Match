using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [ ��� ������ ����]
/// : ����
/// : ��������Ʈ
/// : ����
/// : ��� Ÿ��
/// </summary>

/// <summary>
/// [Ư�� ����� �Ǿ��� ���]
/// : Ư�� ��� Ÿ��
/// : ��������Ʈ
/// : ����
/// </summary>

public class Block : MonoBehaviour
{
    /// <summary>
    /// ���� ���� ������ ������ (����)
    /// </summary>
    [Header("�� ���� ������")]
    [SerializeField] private int num;                          // ���� ����
    [SerializeField] private BlockData blockdata;              // �� ������ SO
    public int Num {  get { return num; } }                    // ���� ���� ��ȯ��
    public BlockType blockType { get; private set; }           // �� Ÿ��
    public SpriteRenderer spriteRenderer { get; private set; } // �� ��������Ʈ

    
    /// <summary>
    /// ���� ���� Ư�� ������ (Ÿ�Ը� ����)
    /// </summary>
    [SerializeField] private MatchType matchType;
    public MatchType MatchType => matchType;


    /// <summary>
    /// ���� ���� ������ ��ǥ�� (Vector2Int)
    /// </summary>
    [SerializeField] Vector2Int boardPos;
    public Vector2Int BoardPos => boardPos;


    /// <summary>
    /// �� �ִϸ��̼�
    /// </summary>

    public BlockAnime Anime { get; private set; }


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Anime = GetComponent<BlockAnime>();
    }


    /// <summary>
    /// �� ������ ���� (default ������)
    /// -> �� ���� ���� �� ȣ�� (Instantiate�� �ܺο���, �ʱ�ȭ�� ���⼭)
    /// </summary>
    /// <param name="data">BlockDataManager�� ����� ������ �� ����</param>    
    public void Init(BlockData data)
    {
        num = data.Num;
        blockType = data.Blocktype;
        spriteRenderer.sprite = data.Sprite;
        spriteRenderer.color = data.Color;
        blockdata = data;
    }


    /// <summary>
    /// �� Ư�� Ÿ�� ����
    /// </summary>
    /// <param name="matchType">��ġ�� Ÿ��</param>
    public void ApplySpecial(MatchType matchType)
    {
        if (blockdata == null) return;

        this.matchType = matchType;

        // ��ġŸ�Կ� �ش��ϴ� Ư�� ��������Ʈ & ���� ������ ����
        Sprite specialSprite = blockdata.GetSpriteByMatchType(matchType);
        Color specialColor = blockdata.GetColorByMatchType(matchType);

        if (specialSprite != null)
        {
            spriteRenderer.sprite = specialSprite;
            spriteRenderer.color = specialColor;    
        }
    }


    /// <summary>
    /// ���忡�� ������ ��ǥ ���� Vector2Int(x, y) 
    /// : 2���� �迭 �󿡼� [y][x]
    /// </summary>
    /// <param name="x">��</param>
    /// <param name="y">��</param>
    public void SetBoardPos(int x, int y)
    {
        boardPos = new Vector2Int(x, y);
    } 
}
