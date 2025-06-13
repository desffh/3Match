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

    // 최상단에 블럭 생성
    public bool TrySpawnTopBlocks()
    {
        bool isBlockSpawned = false;

        for (int x = 0; x < Col; x++)
        {
            int y = Row - 1; // 최상단 행

            if (blocks[y, x] == null)
            {
                int number = GetValidBlockNumber(x, y); // 3매치가 발생하지 않는 후보군 반환
                BlockData blockData = blockDataManager.Get(number); // 블럭 데이터 가져오기

                // 블럭 생성
                Vector3 pos = getWorldPosition(x, y);
                Block block = blockCreater.SpawnBlock(pos, blockData, new Vector2Int(x, y), blockScale);
                block.Anime.MoveTo(pos, Ease.InOutBack);
                blocks[y, x] = block;

                isBlockSpawned = true;
            }
        }

        return isBlockSpawned;
    }

    // 3매치가 발생하지 않는 블럭 생성
    private int GetValidBlockNumber(int x, int y)
    {
        // 시작 시 나올 데이터 고정
        List<int> candidates = new List<int> { 1, 2, 3, };

        for (int i = candidates.Count - 1; i >= 0; i--)
        {
            int n = candidates[i];

            // 가로 확인
            bool rowMatch = (x >= 2 &&
                blocks[y, x - 1]?.Num == n &&
                blocks[y, x - 2]?.Num == n);

            // 세로 확인
            bool colMatch = (
                y >= 2 &&
                blocks[y - 1, x]?.Num == n &&
                blocks[y - 2, x]?.Num == n);

            // 네모 확인
            bool squareMatch = (x >= 1 && y >= 1 &&
                blocks[y, x - 1]?.Num == n &&
                blocks[y - 1, x]?.Num == n &&
                blocks[y - 1, x - 1]?.Num == n);

            // 만약 하나라도 해당된다면 후보군에서 제외
            if (rowMatch || colMatch || squareMatch)
            {
                candidates.RemoveAt(i);
            }
        }

        // 남은 후보군 중 랜덤으로 블럭 데이터 반환
        if (candidates.Count > 0)
        {
            return candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }

        // 남은 후보군이 없다면 6 반환
        return 6;
    }

}
