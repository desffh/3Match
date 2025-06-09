using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  ��� ���� ��� : ����� ��ġ ���� & �ִϸ��̼� ȣ��
/// </summary>

public class BlockSwap
{
    private Board board;

    Block[,] blocks;

    // ������ ����
    public BlockSwap(Board board, Block[,] blocks)
    {
        this.board = board;

        this.blocks = blocks;
    }

    /// <summary>
    /// ���õ� ��a�� ��b�� 2���� �迭�� ��ǥ�� ����
    /// 
    /// ������ ���� ��ǥ���� ��ġ ���� (Anime.MoveTo ��Ʈ�� ȣ��)
    /// </summary>
    
    public void TrySwap(Block a, Block b, Action onComplete = null)
    {
        Vector2Int posA = a.BoardPos;
        Vector2Int posB = b.BoardPos;

        blocks[posA.y, posA.x] = b;
        blocks[posB.y, posB.x] = a;

        a.SetBoardPos(posB.x, posB.y);
        b.SetBoardPos(posA.x, posA.y);

        Tween tweenA = a.Anime.MoveTo(board.GetWorldPosition(posB.x, posB.y));
        Tween tweenB = b.Anime.MoveTo(board.GetWorldPosition(posA.x, posA.y));

        DOTween.Sequence()
            .Append(tweenA)
            .Join(tweenB)
            .OnComplete(() => onComplete?.Invoke());
    }
}
