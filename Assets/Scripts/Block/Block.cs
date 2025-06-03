using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 블록 데이터
public class Block : MonoBehaviour
{
    [SerializeField] int num;

    // Board 상의 좌표 저장 
    [SerializeField] Vector2Int boardPos;
    public Vector2Int BoardPos => boardPos;

    public int Num {  get { return num; } }
    public BlockType blockType { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public BlockAnime Anime { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Anime = GetComponent<BlockAnime>();
    }

    // 타일 데이터 주입
    // -> 블럭 동적 생성 시 호출 (Instantiate는 외부에서, 초기화만 여기서)
    public void Init(BlockData data)
    {
        num = data.Num;

        blockType = data.Blocktype;

        spriteRenderer.sprite = data.Sprite;

        spriteRenderer.color = data.Color;
    }

    // 보드에서 가져온 좌표 지정
    public void SetBoardPos(int x, int y)
    {
        boardPos = new Vector2Int(x, y);
    }

    // 블록 데이터 등록
    public void  blockdataSet(int num, BlockType type, Sprite sprite, Color color)
    {
        this.num = num;

        blockType = type;

        spriteRenderer.sprite = sprite;

        spriteRenderer.color = color;
    }

}
