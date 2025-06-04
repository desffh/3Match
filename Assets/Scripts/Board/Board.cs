using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// ���带 �����ϴ� �� & �� 
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

    int row; // ��
    int col; // ��

    // 2���� �迭
    private Cell[,] cells;
    private Block[,] blocks;

    public int Row => row;
    public int Col => col;

    public Cell[,] Cells => cells;
    public Block[,] Blocks => blocks;


    private float cellSpacing;
    private Vector3 blockScale;

    public Vector3 BlockScale => blockScale;

    // ��ġ �̺�Ʈ
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

    // 2���� �迭 ũ�� ���� [row, col]
    public void Init(int nRow, int nCol)
    {
        row = nRow;
        col = nCol;

        cells = new Cell[row, col];
        blocks = new Block[row, col];

        blockCreater.Init(); // �� �迭 �����Ǹ� ������Ʈ Ǯ ���� & �ʱ�ȭ

        CalculateLayout();
    }

    // ��, �� ������Ʈ ����
    public void SpawnBoard(CellData cellData, List<BlockData> blockDataList)
    {
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                // ��ǥ�� (��x, ��y)
                CreateCell(x, y, cellData);
                CreateBlock(x, y, blockDataList);
            }
        }
    }

    // �� ����
    private void CreateCell(int x, int y, CellData cellData)
    {
        Vector3 pos = GetWorldPosition(x, y);
        GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);

        cellObj.transform.localScale = blockScale;

        Cell cell = cellObj.GetComponent<Cell>();

        cell.Init(cellData); // ������ ����

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

    // ��, �� ��ġ ���
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSpacing, y * cellSpacing, 0) + (Vector3)boardOrigin;
    }

    // �׸��� ũ�� & ��ġ ���
    private void CalculateLayout()
    {
        // ��ü ���� ������
        float rawBoardWidth = col * BaseCellSpacing;
        float rawBoardHeight = row * BaseCellSpacing;

        // ȭ�鿡 ���߱� ���� ������ ���
        float scaleFactor = Mathf.Min(targetWidth / rawBoardWidth, targetHeight / rawBoardHeight);

        // ������ ����
        cellSpacing = BaseCellSpacing * scaleFactor;
        blockScale = baseBlockScale * scaleFactor;

        float totalWidth = (col - 1) * cellSpacing;
        float totalHeight = (row - 1) * cellSpacing;

        // ���� ���� ��ġ ���
        boardOrigin = new Vector2(-totalWidth / 2f, -totalHeight / 2f);
    }


    // �� �� ��ġ ����
    public void TrySwap(Block a, Block b)
    {
        Vector2Int posA = a.BoardPos;
        Vector2Int posB = b.BoardPos;

        // 1. �迭���� ��ġ �ٲٱ�
        blocks[posA.y, posA.x] = b;
        blocks[posB.y, posB.x] = a;

        // 2. ��� ���� ��ǥ ������Ʈ
        a.SetBoardPos(posB.x, posB.y);
        b.SetBoardPos(posA.x, posA.y);

        // 3. ���� ��ġ �ٲٱ� (�ִϸ��̼� �߰� ����)
        Vector3 worldA = GetWorldPosition(posA.x, posA.y);
        Vector3 worldB = GetWorldPosition(posB.x, posB.y);

        // 4. �ִϸ��̼� ó��
        Tween tweenA = a.Anime.MoveTo(GetWorldPosition(posB.x, posB.y));
        Tween tweenB = b.Anime.MoveTo(GetWorldPosition(posA.x, posA.y));

        // �ݹ� ��� (�� Ʈ���� ��� �Ϸ�Ǿ��� ��)
        DOTween.Sequence()
            .Append(tweenA)
            .Join(tweenB)
            .OnComplete(() =>
            {
                // ��ġ �˻� �̺�Ʈ ȣ��
                OnmatchFind.Invoke(a);
            });

    }


    private void OnDisable()
    {
        BlockInput.OnSwapRequest -= TrySwap;
    }
}
