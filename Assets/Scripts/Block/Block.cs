using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// summary
// [ 블록 데이터 정보]
// : 숫자
// : 스프라이트
// : 색상
// : 블록 타입
//
// [특수 블록이 되었을 경우]
// : 특수 블록 타입
// : 스프라이트
// summary

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

    private BlockData _data;
    private BlockTypeData _blockTypeData;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Anime = GetComponent<BlockAnime>();
    }

    // 블럭 데이터 주입
    // -> 블럭 동적 생성 시 호출 (Instantiate는 외부에서, 초기화만 여기서)
    public void Init(BlockData data)
    {
        num = data.Num;

        blockType = data.Blocktype;

        spriteRenderer.sprite = data.Sprite;

        spriteRenderer.color = data.Color;

        _data = data;
    }

    // 블럭 특수 타입 주입
    public void SpecialInit(BlockTypeData specialData)
    {
        _blockTypeData = specialData;
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
