using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner
{
    private readonly BlockCreater blockCreater;
    private readonly BlockDataManager blockDataManager;
    private readonly Block[,] blocks;
    private readonly Func<int, int, Vector3> getWorldPosition;
    private readonly Vector3 blockScale;
    private readonly int Row;
    private readonly int Col;

    public BlockSpawner(BoardContext context, BlockCreater blockCreater, BlockDataManager blockDataManager, Vector3 blockScale)
    {
        this.blockCreater = blockCreater;
        this.blockDataManager = blockDataManager;

        blocks = context.Blocks;
        getWorldPosition = context.GetWorldPosition;

        Row = context.row;
        Col = context.col;
        
        this.blockScale = blockScale;
    }

    // �ֻ�ܿ� �� ����
    public bool TrySpawnTopBlocks()
    {
        bool isBlockSpawned = false;

        for (int x = 0; x < Col; x++)
        {
            int y = Row - 1; // �ֻ�� ��

            if (blocks[y, x] == null)
            {
                int number = GetValidBlockNumber(x, y); // 3��ġ�� �߻����� �ʴ� �ĺ��� ��ȯ
                BlockData blockData = blockDataManager.Get(number); // �� ������ ��������

                // �� ����
                Vector3 pos = getWorldPosition(x, y);
                Block block = blockCreater.SpawnBlock(pos, blockData, new Vector2Int(x, y), blockScale);
                block.Anime.MoveTo(pos, Ease.InOutBack);
                blocks[y, x] = block;

                isBlockSpawned = true;
            }
        }

        return isBlockSpawned;
    }

    // 3��ġ�� �߻����� �ʴ� �� ����
    private int GetValidBlockNumber(int x, int y)
    {
        // ���� �� ���� ������ ����
        List<int> candidates = new List<int> { 1, 2, 3, };

        for (int i = candidates.Count - 1; i >= 0; i--)
        {
            int n = candidates[i];

            // ���� Ȯ��
            bool rowMatch = (x >= 2 &&
                blocks[y, x - 1]?.Num == n &&
                blocks[y, x - 2]?.Num == n);

            // ���� Ȯ��
            bool colMatch = (
                y >= 2 &&
                blocks[y - 1, x]?.Num == n &&
                blocks[y - 2, x]?.Num == n);

            // �׸� Ȯ��
            bool squareMatch = (x >= 1 && y >= 1 &&
                blocks[y, x - 1]?.Num == n &&
                blocks[y - 1, x]?.Num == n &&
                blocks[y - 1, x - 1]?.Num == n);

            // ���� �ϳ��� �ش�ȴٸ� �ĺ������� ����
            if (rowMatch || colMatch || squareMatch)
            {
                candidates.RemoveAt(i);
            }
        }

        // ���� �ĺ��� �� �������� �� ������ ��ȯ
        if (candidates.Count > 0)
        {
            return candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }

        // ���� �ĺ����� ���ٸ� 6 ��ȯ
        return 6;
    }

}
