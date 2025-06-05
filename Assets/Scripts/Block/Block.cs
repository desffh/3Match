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
///
/// [Ư�� ����� �Ǿ��� ���]
/// : Ư�� ��� Ÿ��
/// : ��������Ʈ
/// </summary>


public class Block : MonoBehaviour
{
    [SerializeField] int num; // ���� ����
    public int Num {  get { return num; } }

    // Board ���� ��ǥ ���� 
    [SerializeField] Vector2Int boardPos;
    public Vector2Int BoardPos => boardPos;

    public BlockType blockType { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public BlockAnime Anime { get; private set; }

    [SerializeField] private BlockData blockdata;
    [SerializeField] private MatchType _matchType;

    public MatchType _MatchType => _matchType;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Anime = GetComponent<BlockAnime>();
    }

    // �� ������ ���� (default ������)
    // -> �� ���� ���� �� ȣ�� (Instantiate�� �ܺο���, �ʱ�ȭ�� ���⼭)
    public void Init(BlockData data)
    {
        num = data.Num;

        blockType = data.Blocktype;

        spriteRenderer.sprite = data.Sprite;

        spriteRenderer.color = data.Color;

        blockdata = data;
    }

    // �� Ư�� Ÿ�� ����
    public void ApplySpecial(MatchType matchType)
    {
        if (blockdata == null) return;

        _matchType = matchType;

        Sprite specialSprite = blockdata.GetSpriteByMatchType(matchType);

        if (specialSprite != null)
        {
            spriteRenderer.sprite = specialSprite;
        }
    }

    // ���忡�� ������ ��ǥ ����
    public void SetBoardPos(int x, int y)
    {
        boardPos = new Vector2Int(x, y);
    } 
}
