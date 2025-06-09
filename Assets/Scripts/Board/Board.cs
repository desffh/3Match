using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    [Header("셀 간 거리 및 시작 위치")]
    [SerializeField] private float BaseCellSpacing = 1.5f;
    [SerializeField] private Vector2 boardOrigin;

    [Header("기본 크기")]
    [SerializeField] private Vector3 baseBlockScale = new Vector3(0.8f, 0.8f, 0.8f);

    [Header("지정 크기")]
    [SerializeField] private float targetWidth = 8f;
    [SerializeField] private float targetHeight = 8f;

    [SerializeField] private BlockDataManager blockDataManager;
    public BlockDataManager BlockDataManager => blockDataManager;

    private BlockCreater blockCreater;

    private int row;
    private int col;

    private Cell[,] cells;
    private Block[,] blocks;

    private float cellSpacing;
    private Vector3 blockScale;

    public float dropDuration = 0.3f;

    public Vector3 BlockScale => blockScale;
    public Cell[,] Cells => cells;
    public Block[,] Blocks => blocks;
    public int Row => row;
    public int Col => col;

    public static event Action OnmatchFind;

    public static event Action FirstOnmatchFind;


    private List<BlockData> blockDatas = new List<BlockData>();


    [SerializeField] int dataCount;

    private BlockSwap blockSwap;


    private void Awake()
    {
        blockCreater = GetComponent<BlockCreater>();

        dataCount = BlockDataManager.blockDataList.Count;

    }

    private void OnEnable()
    {
        BlockInput.OnSwapRequest += TrySwap;
    }

    private void OnDisable()
    {
        BlockInput.OnSwapRequest -= TrySwap;
    }

    private void Start()
    {
        blockDataManager.Init();
    }


    /// <summary>
    /// 스테이지 데이터 세팅 
    /// 1. row행 / col열 2. 그리드 크리 할당 3. BlockCreater 오브젝트 풀 초기화
    /// </summary>

    public void Init(int nRow, int nCol)
    {
        row = nRow;
        col = nCol;

        cells = new Cell[row, col];
        blocks = new Block[row, col];

        blockCreater.Init();
        CalculateLayout();

        // 생성자 주입 
        blockSwap = new BlockSwap(this, Blocks);
    }

    /// <summary>
    /// 보드 생성 - 셀 생성 호출 & 블록 생성 이벤트 호출
    /// </summary>

    public void SpawnBoard(CellData cellData, List<BlockData> blockDataList)
    {
        blockDatas = blockDataList;

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                CreateCell(x, y, cellData);
            }
        }

        FirstOnmatchFind.Invoke();

    }

    /// <summary>
    /// 코루틴으로 FillRoutine() 호출 
    /// </summary>
    /// <returns></returns>

    public IEnumerator FillUntilStable()
    {
        bool filled;

        do
        {
            filled = FillRoutine();
            yield return new WaitForSeconds(dropDuration);
        } 
        while (filled); // filled == true라면 실행
    }

    /// <summary>
    /// 셀 동적 생성 & 2차원 배열에 할당
    /// </summary>

    private void CreateCell(int x, int y, CellData cellData)
    {
        Vector3 pos = GetWorldPosition(x, y);
        GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
        cellObj.transform.localScale = blockScale;

        Cell cell = cellObj.GetComponent<Cell>();
        cell.Init(cellData);

        cells[y, x] = cell;
    }

    // 블럭 생성
    private void CreateBlock(int x, int y, List<BlockData> blockDataList)
    {
        Vector3 pos = GetWorldPosition(x, y);

        BlockData blockData = blockDataList[UnityEngine.Random.Range(0, blockDataList.Count)];

        Block block = blockCreater.SpawnBlock(pos, blockData, new Vector2Int(x, y), blockScale);

        blocks[y, x] = block;
    }
    
    /// <summary>
    /// 블록 최상단 생성 & 빈자리가 없을 때 까지 생성
    /// </summary>
    
    public bool FillRoutine()
    {
        bool isBlockMove = false;

        // 아래에서 위로 빈 자리 찾기
        for (int j = 1; j < Row; j++)     // 행
        {
            for (int i = 0; i < Col; i++) // 열
            {
                if (blocks[j, i] == null) continue;

                Block curBlock = blocks[j, i];       // 현재 블럭
                Block belowBlock = blocks[j - 1, i]; // 아래 블럭

                // 아래 블럭이 비어있다면 
                if (belowBlock == null)
                {
                    BlockChange(curBlock, i, j - 1);
                    isBlockMove = true;
                }
            }
        }

        dataCount = 3;

        // 최상단 블럭 생성 : (Row, 0) ~ (Row, Col) 블럭 생성
        //
        // 데이터 주입 & 2차원 배열 좌표값에 블럭 할당

        for (int i = 0; i < Col; i++) // 열
        {
            if (blocks[Row - 1, i] == null) // blocks[행y][열x]
            {
                Vector3 pos = GetWorldPosition(i, Row - 1);

                BlockData blockData = BlockDataManager.blockDataList[UnityEngine.Random.Range(0, dataCount)];
                Block block = blockCreater.SpawnBlock(pos, blockData, new Vector2Int(i, Row - 1), blockScale);
                block.Anime.MoveTo2(pos, 0.2f);
                blocks[Row - 1, i] = block;

                isBlockMove = true;
            }
        }
        return isBlockMove;
    }


    /// <summary>
    /// 퍼즐 이동 후 2차원 배열 x, y 좌표값 변경
    /// 
    /// 이전 좌표값 null
    /// 
    /// 새로운 좌표로 이동 (world Position)
    /// 
    /// 새로 들어온 좌표값을 블럭에 할당 후 배열에 블럭 저장
    /// </summary>
    
    void BlockChange(Block curBlock, int newX, int newY)
    {
        blocks[curBlock.BoardPos.y, curBlock.BoardPos.x] = null;
        curBlock.SetBoardPos(newX, newY);// 좌표 설정 2차원 좌표 (x, y)
        curBlock.Anime.MoveTo2(GetWorldPosition(newX, newY), 0.1f);
        blocks[newY, newX] = curBlock;   // 좌표 설정 2차원 배열 blocks[y][x] == blocks[행][열]
    }

    /// <summary>
    /// 블록이 삭제될 때 호출 -> 오브젝트 풀 반환 & 2차원 배열 초기화
    /// </summary>

    public void RemoveBlock(Block block)
    {
        if (block == null) return;
        blocks[block.BoardPos.y, block.BoardPos.x] = null;
        blockCreater.DespawnBlock(block);
    }


    /// <summary>
    /// 그리드 행열 크기 계산 & 간격, 블럭 크기 계산
    /// </summary>

    private void CalculateLayout()
    {
        float rawWidth = col * BaseCellSpacing;
        float rawHeight = row * BaseCellSpacing;

        float scaleFactor = Mathf.Min(targetWidth / rawWidth, targetHeight / rawHeight);

        cellSpacing = BaseCellSpacing * scaleFactor;
        blockScale = baseBlockScale * scaleFactor;

        float totalWidth = (col - 1) * cellSpacing;
        float totalHeight = (row - 1) * cellSpacing;

        // 그리그 생성 시작 위치 좌표값 (0,0) 지점
        boardOrigin = new Vector2(-totalWidth / 2f, -totalHeight / 2f);
    }


    /// <summary>
    /// 2차원 배열 좌표에 해당하는 블럭 생성 지점 반환 (World Position) 
    /// </summary>

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSpacing, y * cellSpacing, 0) + (Vector3)boardOrigin;
    }


    /// <summary>
    /// 스왑 애니메이션이 모두 끝나고 매치 이벤트 호출
    ///</summary>

    public void TrySwap(Block a, Block b)
    {
        blockSwap.TrySwap(a, b, () =>
        {
            // 위치 바뀐 후의 매치 로직 실행
            OnmatchFind?.Invoke();
      
            // 전략 패턴 기반 특수 효과 실행

        });
    }

}