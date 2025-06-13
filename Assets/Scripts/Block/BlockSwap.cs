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

    Block[,] blocks;      // board�� ����� ������ 2���� �迭

    // ������ ����
    public BlockSwap(Board board, Block[,] blocks)
    {
        this.board = board;
        this.blocks = blocks;
    }


    /// <summary>
    /// ���õ� ������  2���� �迭�� ��ǥ��, ���� ��ǥ Swap 
    /// </summary>
    /// <param name="a">          ���õ� �� a                                    </param>
    /// <param name="b">          ���õ� �� b                                    </param>
    /// <param name="onComplete"> ������ ������ ȣ��� �̺�Ʈ -> OnMatchFind ȣ��   </param>
    public void TrySwap(Block a, Block b, Action onComplete = null)
    {
        Vector2Int posA = a.BoardPos;
        Vector2Int posB = b.BoardPos;

        blocks[posA.y, posA.x] = b;    // 2���� �迭 ��ǥ�� Swap
        blocks[posB.y, posB.x] = a;

        a.SetBoardPos(posB.x, posB.y); // �� ���� Vector2Int ��ǥ�� Swap 
        b.SetBoardPos(posA.x, posA.y);

        //���� ��ǥ Swap (Dotween Animation ���)
        Tween tweenA = a.Anime.MoveTo(board.GetWorldPosition(posB.x, posB.y), Ease.OutQuart);
        Tween tweenB = b.Anime.MoveTo(board.GetWorldPosition(posA.x, posA.y), Ease.OutQuart);

        DOTween.Sequence()
            .Append(tweenA)
            .Join(tweenB)
            .OnComplete(() => onComplete?.Invoke());
    }
}
