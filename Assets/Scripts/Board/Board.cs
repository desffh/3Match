using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("셀 간 거리 및 시작 위치")]
    [SerializeField] private Vector2 boardOrigin;          // 그리드 시작 위치
    private float cellSpacing;
    [SerializeField] private float dropDuration = 0.3f;    // 떨어지는 속도 간격
    [SerializeField] private Vector3 blockScale;           // 블록 크기
    public Vector3 BlockScale => blockScale;


    [Header("기본 블럭 크기")]
    private Vector3 baseBlockScale = new Vector3(0.8f, 0.8f, 0.8f);


    [Header("지정 보드 크기")]
    [SerializeField] private float targetWidth = 8f;
    [SerializeField] private float targetHeight = 8f;


    [Header("가져와서 사용하기 (컴포넌트)")]
    [SerializeField] private BlockDataManager blockDataManager;
    private BlockCreater blockCreater;
    private CellCreater cellCreater;
    public BlockCreater BlockCreater => blockCreater;
    public CellCreater CellCreater => cellCreater;
    public BlockDataManager BlockDataManager => blockDataManager;


    [Header("스테이지에 따른 행 열")]
    private int row;
    private int col;
    public int Row => row;
    public int Col => col;


    [Header("2차원 배열")]
    private Cell[,] cells;
    private Block[,] blocks;
    public Cell[,] Cells => cells;
    public Block[,] Blocks => blocks;





    [Header("일반 C# 클래스")]
    private BlockSwap blockSwap;
    private BlockSpawner blockSpawner;
    private BlocksBelow blocksBelow;


    public static event Action OnmatchFind;

    private void Awake()
    {
        blockCreater = GetComponent<BlockCreater>();
        cellCreater = GetComponent<CellCreater>();
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


        BoardContext context = new BoardContext
        {
            Blocks = blocks,
            GetWorldPosition = GetWorldPosition,
            col = col,
            row = row,
        };

        // 생성자 주입 
        blockSwap = new BlockSwap(this, Blocks);
        blockSpawner = new BlockSpawner(context, BlockCreater, BlockDataManager, BlockScale);
        blocksBelow = new BlocksBelow(context);
    }


    /// <summary>
    /// 셀 동적 생성 & 2차원 배열에 할당
    /// </summary>
    private void CreateCell(int x, int y, CellData cellData)
    {
        Vector3 pos = GetWorldPosition(x, y);

        Cell cell = CellCreater.SpawnCell(pos, cellData, BlockScale);

        cells[y, x] = cell;
    }


    /// <summary>
    /// 보드 생성 - 셀 생성 호출 & 블록 생성 이벤트 호출
    /// </summary>
    public void SpawnBoard(CellData cellData, List<BlockData> blockDataList)
    { 
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                CreateCell(x, y, cellData);
            }
        }
        OnmatchFind.Invoke();
    }

    /// <summary>
    /// 코루틴으로 FillRoutine() 호출 
    /// </summary>
    public IEnumerator FillBlocks()
    {
        bool filled;

        do
        {
            filled = FillRoutine(); // 블럭이 내려가거나 생성되면 true
            yield return new WaitForSeconds(dropDuration);
        }
        while (filled); // true면 계속 반복
    }


    /// <summary>
    /// 블록 생성 & 블록 아래로 내리기 루틴 / DropBlocks(), TrySpawnBlocks(), GetValidBlockNumber()
    /// </summary>
    public bool FillRoutine()
    {
        bool isBlockMoved = false;

        if (blocksBelow.DropBlocks()) isBlockMoved = true;
        if (blockSpawner.TrySpawnTopBlocks()) isBlockMoved = true;

        return isBlockMoved;
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
        float BaseCellSpacing = 1.5f; // 블럭 간 간격
        
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