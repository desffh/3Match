using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  블록 스왑 기능 : 블록의 위치 변경 & 애니메이션 호출
/// </summary>

public class BlockSwap
{
    private Board board;

    Block[,] blocks;

    // 생성자 주입
    public BlockSwap(Board board, Block[,] blocks)
    {
        this.board = board;

        this.blocks = blocks;
    }

    /// <summary>
    /// 선택된 블럭a와 블럭b의 2차원 배열의 좌표값 스왑
    /// 
    /// 서로의 월드 좌표끼리 위치 스왑 (Anime.MoveTo 두트윈 호출)
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
