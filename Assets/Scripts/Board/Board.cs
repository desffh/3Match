using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// 보드를 구성하는 셀 & 블럭 
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

    int row; // 행
    int col; // 열

    // 2차원 배열
    private Cell[,] cells;
    private Block[,] blocks;

    public int Row => row;
    public int Col => col;

    public Cell[,] Cells => cells;
    public Block[,] Blocks => blocks;


    private float cellSpacing;
    private Vector3 blockScale;

    public Vector3 BlockScale => blockScale;

    // 매치 이벤트
    public static event Action<Block> OnmatchFind;


    private void Awake()
    {
        blockCreater = GetComponent<BlockCreater>();
    }

    private void OnEnable()
    {
        BlockInput.OnSwapRequest += TrySwap;
    }

    private void Start()
    {
        blockDataManager.Init();
    }

    // 2차원 배열 크기 설정 [row, col]
    public void Init(int nRow, int nCol)
    {
        row = nRow;
        col = nCol;

        cells = new Cell[row, col];
        blocks = new Block[row, col];

        blockCreater.Init(); // 블럭 배열 생성되면 오브젝트 풀 생성 & 초기화

        CalculateLayout();
    }

    // 셀, 블럭 오브젝트 생성
    public void SpawnBoard(CellData cellData, List<BlockData> blockDataList)
    {
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                // 좌표값 (행x, 열y)
                CreateCell(x, y, cellData);
                CreateBlock(x, y, blockDataList);
            }
        }
    }

    // 셀 생성
    private void CreateCell(int x, int y, CellData cellData)
    {
        Vector3 pos = GetWorldPosition(x, y);
        GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);

        cellObj.transform.localScale = blockScale;

        Cell cell = cellObj.GetComponent<Cell>();

        cell.Init(cellData); // 데이터 주입

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

    // 셀, 블럭 위치 계산
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSpacing, y * cellSpacing, 0) + (Vector3)boardOrigin;
    }

    // 그리드 크기 & 위치 계산
    private void CalculateLayout()
    {
        // 전체 보드 사이즈
        float rawBoardWidth = col * BaseCellSpacing;
        float rawBoardHeight = row * BaseCellSpacing;

        // 화면에 맞추기 위한 스케일 계수
        float scaleFactor = Mathf.Min(targetWidth / rawBoardWidth, targetHeight / rawBoardHeight);

        // 스케일 조정
        cellSpacing = BaseCellSpacing * scaleFactor;
        blockScale = baseBlockScale * scaleFactor;

        float totalWidth = (col - 1) * cellSpacing;
        float totalHeight = (row - 1) * cellSpacing;

        // 보드 시작 위치 계산
        boardOrigin = new Vector2(-totalWidth / 2f, -totalHeight / 2f);
    }


    // 두 블럭 위치 스왑
    public void TrySwap(Block a, Block b)
    {
        Vector2Int posA = a.BoardPos;
        Vector2Int posB = b.BoardPos;

        // 1. 배열에서 위치 바꾸기
        blocks[posA.y, posA.x] = b;
        blocks[posB.y, posB.x] = a;

        // 2. 블록 내부 좌표 업데이트
        a.SetBoardPos(posB.x, posB.y);
        b.SetBoardPos(posA.x, posA.y);

        // 3. 월드 위치 바꾸기 (애니메이션 추가 가능)
        Vector3 worldA = GetWorldPosition(posA.x, posA.y);
        Vector3 worldB = GetWorldPosition(posB.x, posB.y);

        // 4. 애니메이션 처리
        Tween tweenA = a.Anime.MoveTo(GetWorldPosition(posB.x, posB.y));
        Tween tweenB = b.Anime.MoveTo(GetWorldPosition(posA.x, posA.y));

        // 콜백 등록 (두 트윈이 모두 완료되었을 때)
        DOTween.Sequence()
            .Append(tweenA)
            .Join(tweenB)
            .OnComplete(() =>
            {
                // 매치 검사 이벤트 호출
                OnmatchFind.Invoke(a);
            });

    }


    private void OnDisable()
    {
        BlockInput.OnSwapRequest -= TrySwap;
    }
}
