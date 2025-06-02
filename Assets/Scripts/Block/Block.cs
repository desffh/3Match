using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] int num;

    // Board ���� ��ǥ ���� 
    [SerializeField] Vector2Int boardPos;
    public Vector2Int BoardPos => boardPos;

    public int Num {  get { return num; } }
    public BlockType blockType { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Ÿ�� ������ ����
    // -> �� ���� ���� �� ȣ�� (Instantiate�� �ܺο���, �ʱ�ȭ�� ���⼭)
    public void Init(BlockData tiledata)
    {
        num = tiledata.Num;

        blockType = tiledata.Blocktype;

        spriteRenderer.sprite = tiledata.Sprite;
    }

    // ���忡�� ������ ��ǥ ����
    public void SetBoardPos(int x, int y)
    {
        boardPos = new Vector2Int(x, y);
    }

    // ��� Ÿ�� ���� (���� ��� ����)
}
