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

    Block[,] blocks;      // board에 저장된 블럭들의 2차원 배열

    // 생성자 주입
    public BlockSwap(Board board, Block[,] blocks)
    {
        this.board = board;
        this.blocks = blocks;
    }


    /// <summary>
    /// 선택된 블럭끼리  2차원 배열의 좌표값, 월드 좌표 Swap 
    /// </summary>
    /// <param name="a">          선택된 블럭 a                                    </param>
    /// <param name="b">          선택된 블럭 b                                    </param>
    /// <param name="onComplete"> 스왑이 끝나고 호출될 이벤트 -> OnMatchFind 호출   </param>
    public void TrySwap(Block a, Block b, Action onComplete = null)
    {
        Vector2Int posA = a.BoardPos;
        Vector2Int posB = b.BoardPos;

        blocks[posA.y, posA.x] = b;    // 2차원 배열 좌표값 Swap
        blocks[posB.y, posB.x] = a;

        a.SetBoardPos(posB.x, posB.y); // 블럭 소유 Vector2Int 좌표값 Swap 
        b.SetBoardPos(posA.x, posA.y);

        //월드 좌표 Swap (Dotween Animation 사용)
        Tween tweenA = a.Anime.MoveTo(board.GetWorldPosition(posB.x, posB.y), Ease.OutQuart);
        Tween tweenB = b.Anime.MoveTo(board.GetWorldPosition(posA.x, posA.y), Ease.OutQuart);

        DOTween.Sequence()
            .Append(tweenA)
            .Join(tweenB)
            .OnComplete(() => onComplete?.Invoke());
    }
}
