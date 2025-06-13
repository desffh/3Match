using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("�� �� �Ÿ� �� ���� ��ġ")]
    [SerializeField] private Vector2 boardOrigin;          // �׸��� ���� ��ġ
    private float cellSpacing;
    [SerializeField] private float dropDuration = 0.3f;    // �������� �ӵ� ����
    [SerializeField] private Vector3 blockScale;           // ��� ũ��
    public Vector3 BlockScale => blockScale;


    [Header("�⺻ �� ũ��")]
    private Vector3 baseBlockScale = new Vector3(0.8f, 0.8f, 0.8f);


    [Header("���� ���� ũ��")]
    [SerializeField] private float targetWidth = 8f;
    [SerializeField] private float targetHeight = 8f;


    [Header("�����ͼ� ����ϱ� (������Ʈ)")]
    [SerializeField] private BlockDataManager blockDataManager;
    private BlockCreater blockCreater;
    private CellCreater cellCreater;
    public BlockCreater BlockCreater => blockCreater;
    public CellCreater CellCreater => cellCreater;
    public BlockDataManager BlockDataManager => blockDataManager;


    [Header("���������� ���� �� ��")]
    private int row;
    private int col;
    public int Row => row;
    public int Col => col;


    [Header("2���� �迭")]
    private Cell[,] cells;
    private Block[,] blocks;
    public Cell[,] Cells => cells;
    public Block[,] Blocks => blocks;





    [Header("�Ϲ� C# Ŭ����")]
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


        BoardContext context = new BoardContext
        {
            Blocks = blocks,
            GetWorldPosition = GetWorldPosition,
            col = col,
            row = row,
        };

        // ������ ���� 
        blockSwap = new BlockSwap(this, Blocks);
        blockSpawner = new BlockSpawner(context, BlockCreater, BlockDataManager, BlockScale);
        blocksBelow = new BlocksBelow(context);
    }


    /// <summary>
    /// �� ���� ���� & 2���� �迭�� �Ҵ�
    /// </summary>
    private void CreateCell(int x, int y, CellData cellData)
    {
        Vector3 pos = GetWorldPosition(x, y);

        Cell cell = CellCreater.SpawnCell(pos, cellData, BlockScale);

        cells[y, x] = cell;
    }


    /// <summary>
    /// ���� ���� - �� ���� ȣ�� & ��� ���� �̺�Ʈ ȣ��
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
    /// �ڷ�ƾ���� FillRoutine() ȣ�� 
    /// </summary>
    public IEnumerator FillBlocks()
    {
        bool filled;

        do
        {
            filled = FillRoutine(); // ���� �������ų� �����Ǹ� true
            yield return new WaitForSeconds(dropDuration);
        }
        while (filled); // true�� ��� �ݺ�
    }


    /// <summary>
    /// ��� ���� & ��� �Ʒ��� ������ ��ƾ / DropBlocks(), TrySpawnBlocks(), GetValidBlockNumber()
    /// </summary>
    public bool FillRoutine()
    {
        bool isBlockMoved = false;

        if (blocksBelow.DropBlocks()) isBlockMoved = true;
        if (blockSpawner.TrySpawnTopBlocks()) isBlockMoved = true;

        return isBlockMoved;
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
        float BaseCellSpacing = 1.5f; // �� �� ����
        
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