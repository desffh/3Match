using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    [Header("�� �� �Ÿ� �� ���� ��ġ")]
    [SerializeField] private float BaseCellSpacing = 1.5f;
    [SerializeField] private Vector2 boardOrigin;

    [Header("�⺻ ũ��")]
    [SerializeField] private Vector3 baseBlockScale = new Vector3(0.8f, 0.8f, 0.8f);

    [Header("���� ũ��")]
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
    /// �������� ������ ���� 
    /// 1. row�� / col�� 2. �׸��� ũ�� �Ҵ� 3. BlockCreater ������Ʈ Ǯ �ʱ�ȭ
    /// </summary>

    public void Init(int nRow, int nCol)
    {
        row = nRow;
        col = nCol;

        cells = new Cell[row, col];
        blocks = new Block[row, col];

        blockCreater.Init();
        CalculateLayout();

        // ������ ���� 
        blockSwap = new BlockSwap(this, Blocks);
    }

    /// <summary>
    /// ���� ���� - �� ���� ȣ�� & ��� ���� �̺�Ʈ ȣ��
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
    /// �ڷ�ƾ���� FillRoutine() ȣ�� 
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
        while (filled); // filled == true��� ����
    }

    /// <summary>
    /// �� ���� ���� & 2���� �迭�� �Ҵ�
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

    // �� ����
    private void CreateBlock(int x, int y, List<BlockData> blockDataList)
    {
        Vector3 pos = GetWorldPosition(x, y);

        BlockData blockData = blockDataList[UnityEngine.Random.Range(0, blockDataList.Count)];

        Block block = blockCreater.SpawnBlock(pos, blockData, new Vector2Int(x, y), blockScale);

        blocks[y, x] = block;
    }
    
    /// <summary>
    /// ��� �ֻ�� ���� & ���ڸ��� ���� �� ���� ����
    /// </summary>
    
    public bool FillRoutine()
    {
        bool isBlockMove = false;

        // �Ʒ����� ���� �� �ڸ� ã��
        for (int j = 1; j < Row; j++)     // ��
        {
            for (int i = 0; i < Col; i++) // ��
            {
                if (blocks[j, i] == null) continue;

                Block curBlock = blocks[j, i];       // ���� ��
                Block belowBlock = blocks[j - 1, i]; // �Ʒ� ��

                // �Ʒ� ���� ����ִٸ� 
                if (belowBlock == null)
                {
                    BlockChange(curBlock, i, j - 1);
                    isBlockMove = true;
                }
            }
        }

        dataCount = 3;

        // �ֻ�� �� ���� : (Row, 0) ~ (Row, Col) �� ����
        //
        // ������ ���� & 2���� �迭 ��ǥ���� �� �Ҵ�

        for (int i = 0; i < Col; i++) // ��
        {
            if (blocks[Row - 1, i] == null) // blocks[��y][��x]
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
    /// ���� �̵� �� 2���� �迭 x, y ��ǥ�� ����
    /// 
    /// ���� ��ǥ�� null
    /// 
    /// ���ο� ��ǥ�� �̵� (world Position)
    /// 
    /// ���� ���� ��ǥ���� ���� �Ҵ� �� �迭�� �� ����
    /// </summary>
    
    void BlockChange(Block curBlock, int newX, int newY)
    {
        blocks[curBlock.BoardPos.y, curBlock.BoardPos.x] = null;
        curBlock.SetBoardPos(newX, newY);// ��ǥ ���� 2���� ��ǥ (x, y)
        curBlock.Anime.MoveTo2(GetWorldPosition(newX, newY), 0.1f);
        blocks[newY, newX] = curBlock;   // ��ǥ ���� 2���� �迭 blocks[y][x] == blocks[��][��]
    }

    /// <summary>
    /// ����� ������ �� ȣ�� -> ������Ʈ Ǯ ��ȯ & 2���� �迭 �ʱ�ȭ
    /// </summary>

    public void RemoveBlock(Block block)
    {
        if (block == null) return;
        blocks[block.BoardPos.y, block.BoardPos.x] = null;
        blockCreater.DespawnBlock(block);
    }


    /// <summary>
    /// �׸��� �࿭ ũ�� ��� & ����, �� ũ�� ���
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

        // �׸��� ���� ���� ��ġ ��ǥ�� (0,0) ����
        boardOrigin = new Vector2(-totalWidth / 2f, -totalHeight / 2f);
    }


    /// <summary>
    /// 2���� �迭 ��ǥ�� �ش��ϴ� �� ���� ���� ��ȯ (World Position) 
    /// </summary>

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSpacing, y * cellSpacing, 0) + (Vector3)boardOrigin;
    }


    /// <summary>
    /// ���� �ִϸ��̼��� ��� ������ ��ġ �̺�Ʈ ȣ��
    ///</summary>

    public void TrySwap(Block a, Block b)
    {
        blockSwap.TrySwap(a, b, () =>
        {
            // ��ġ �ٲ� ���� ��ġ ���� ����
            OnmatchFind?.Invoke();
      
            // ���� ���� ��� Ư�� ȿ�� ����

        });
    }

}