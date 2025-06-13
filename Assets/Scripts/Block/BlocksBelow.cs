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


    // �� �Ʒ��� ����߸���
    public bool DropBlocks()
    {
        bool isBlockMoved = false;

        for (int y = 1; y < Row; y++)           // ��
        {
            for (int x = 0; x < Col; x++)       // ��
            {
                if (blocks[y, x] == null) continue;

                Block current = blocks[y, x];   // ���� ��
                Block below = blocks[y - 1, x]; // �Ʒ� ��

                if (below == null) // �Ʒ��� ����ִٸ� �Ʒ��� ü����
                {
                    BlockChange(current, x, y - 1);
                    isBlockMoved = true;
                }
            }
        }

        return isBlockMoved;
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
        curBlock.Anime.MoveTo(getWorldPosition(newX, newY), Ease.InOutBack, 0.1f);
        blocks[newY, newX] = curBlock;   // ��ǥ ���� 2���� �迭 blocks[y][x] == blocks[��][��]
    }
}
