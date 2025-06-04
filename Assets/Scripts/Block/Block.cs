using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// summary
// [ ��� ������ ����]
// : ����
// : ��������Ʈ
// : ����
// : ��� Ÿ��
//
// [Ư�� ����� �Ǿ��� ���]
// : Ư�� ��� Ÿ��
// : ��������Ʈ
// summary

public class Block : MonoBehaviour
{
    [SerializeField] int num;

    // Board ���� ��ǥ ���� 
    [SerializeField] Vector2Int boardPos;
    public Vector2Int BoardPos => boardPos;

    public int Num {  get { return num; } }
    public BlockType blockType { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public BlockAnime Anime { get; private set; }

    private BlockData _data;
    private BlockTypeData _blockTypeData;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Anime = GetComponent<BlockAnime>();
    }

    // �� ������ ����
    // -> �� ���� ���� �� ȣ�� (Instantiate�� �ܺο���, �ʱ�ȭ�� ���⼭)
    public void Init(BlockData data)
    {
        num = data.Num;

        blockType = data.Blocktype;

        spriteRenderer.sprite = data.Sprite;

        spriteRenderer.color = data.Color;

        _data = data;
    }

    // �� Ư�� Ÿ�� ����
    public void SpecialInit(BlockTypeData specialData)
    {
        _blockTypeData = specialData;
    }

    // ���忡�� ������ ��ǥ ����
    public void SetBoardPos(int x, int y)
    {
        boardPos = new Vector2Int(x, y);
    }

    // ��� ������ ���
    public void  blockdataSet(int num, BlockType type, Sprite sprite, Color color)
    {
        this.num = num;

        blockType = type;

        spriteRenderer.sprite = sprite;

        spriteRenderer.color = color;
    }

}
