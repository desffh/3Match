using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

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

    public void Init(int nRow, int nCol)
    {
        row = nRow;
        col = nCol;

        cells = new Cell[row, col];
        blocks = new Block[row, col];

        blockCreater.Init();
        CalculateLayout();
    }

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
        // 보드 셀 생성 후 블록 생성 시작
        //StartCoroutine(FillUntilStable());
        FirstOnmatchFind.Invoke();

    }





    public IEnumerator FillUntilStable()
    {
        bool filled;

        do
        {
            filled = FillRoutine();
            yield return new WaitForSeconds(dropDuration);
        } while (filled);
    }


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

    public bool FillRoutine()
    {
        bool isBlockMove = false;

        for (int j = 1; j < Row; j++) // j는 1부터 시작!
        {
            for (int i = 0; i < Col; i++)
            {
                if (blocks[j, i] == null) continue;

                Block curPuzzle = blocks[j, i];
                Block belowPuzzle = blocks[j - 1, i];

                if (belowPuzzle == null)
                {
                    PuzzleChange(curPuzzle, i, j - 1);
                    isBlockMove = true;
                }
            }
        }

        dataCount = 3;

        // 최상단 퍼즐 생성
        for (int i = 0; i < Col; i++)  //  여기도 Col 기준으로
        {
            if (blocks[Row - 1, i] == null)
            {
                Vector3 pos = GetWorldPosition(i, Row - 1);

                BlockData blockData = BlockDataManager.blockDataList[
                    UnityEngine.Random.Range(0, dataCount)];

                Block block = blockCreater.SpawnBlock(pos, blockData, new Vector2Int(i, Row - 1), blockScale);

                block.Anime.MoveTo2(pos, 0.2f);

                blocks[Row - 1, i] = block;

                isBlockMove = true;
            }
        }

        return isBlockMove;
    }


    //퍼즐 이동 후 배열,x,y값 바꾸기
    void PuzzleChange(Block curPuzzle, int newX, int newY)
    {
        blocks[curPuzzle.BoardPos.y, curPuzzle.BoardPos.x] = null;
        curPuzzle.SetBoardPos(newX, newY);
        curPuzzle.Anime.MoveTo2(GetWorldPosition(newX, newY), 0.1f);
        blocks[newY, newX] = curPuzzle;
    }

    public void RemoveBlock(Block block)
    {
        if (block == null) return;
        blocks[block.BoardPos.y, block.BoardPos.x] = null;
        blockCreater.DespawnBlock(block);
    }

    private void CalculateLayout()
    {
        float rawWidth = col * BaseCellSpacing;
        float rawHeight = row * BaseCellSpacing;

        float scaleFactor = Mathf.Min(targetWidth / rawWidth, targetHeight / rawHeight);

        cellSpacing = BaseCellSpacing * scaleFactor;
        blockScale = baseBlockScale * scaleFactor;

        float totalWidth = (col - 1) * cellSpacing;
        float totalHeight = (row - 1) * cellSpacing;

        boardOrigin = new Vector2(-totalWidth / 2f, -totalHeight / 2f);
    }


    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSpacing, y * cellSpacing, 0) + (Vector3)boardOrigin;
    }

    public void TrySwap(Block a, Block b)
    {
        Vector2Int posA = a.BoardPos;
        Vector2Int posB = b.BoardPos;

        blocks[posA.y, posA.x] = b;
        blocks[posB.y, posB.x] = a;

        a.SetBoardPos(posB.x, posB.y);
        b.SetBoardPos(posA.x, posA.y);

        Tween tweenA = a.Anime.MoveTo(GetWorldPosition(posB.x, posB.y));
        Tween tweenB = b.Anime.MoveTo(GetWorldPosition(posA.x, posA.y));

        DOTween.Sequence()
            .Append(tweenA)
            .Join(tweenB)
            .OnComplete(() => OnmatchFind?.Invoke());
        

    }

}