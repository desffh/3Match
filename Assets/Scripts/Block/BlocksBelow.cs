using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksBelow
{
    private readonly Block[,] blocks;
    private readonly Func<int, int, Vector3> getWorldPosition;
    private readonly int Row;
    private readonly int Col;

    public BlocksBelow(BoardContext context)
    {
        this.blocks = context.Blocks;
        this.getWorldPosition = context.GetWorldPosition;
        Row = context.row;
        Col = context.col;
    }


    // 블럭 아래로 떨어뜨리기
    public bool DropBlocks()
    {
        bool isBlockMoved = false;

        for (int y = 1; y < Row; y++)           // 행
        {
            for (int x = 0; x < Col; x++)       // 열
            {
                if (blocks[y, x] == null) continue;

                Block current = blocks[y, x];   // 현재 블럭
                Block below = blocks[y - 1, x]; // 아래 블럭

                if (below == null) // 아래가 비어있다면 아래와 체인지
                {
                    BlockChange(current, x, y - 1);
                    isBlockMoved = true;
                }
            }
        }

        return isBlockMoved;
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
        curBlock.Anime.MoveTo(getWorldPosition(newX, newY), Ease.InOutBack, 0.1f);
        blocks[newY, newX] = curBlock;   // 좌표 설정 2차원 배열 blocks[y][x] == blocks[행][열]
    }
}
