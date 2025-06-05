using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 병합 실행
/// </summary>

public class MatchMerger
{
    private Board board;

    // 생성자 주입 
    public MatchMerger(Board board)
    {
        this.board = board;
    }

    public void Merge(List<Block> blocks, MatchType matchType)
    {
        if (blocks == null || blocks.Count == 0)
        {
            return;
        }

        // 중심 블록 계산
        Block center = GetMergeCenter(blocks, matchType);
        if (matchType == MatchType.Line3_Row || matchType == MatchType.Line3_Col)
        {
            // 3매치: Num 업그레이드
            int newNum = blocks[0].Num + 1;
            
            if (blocks[0].Num == 6)
            {
                newNum = 6;
            }

            BlockData blockData = board.BlockDataManager.Get(newNum);

            center.Init(blockData);
            center.ApplySpecial(matchType);

            Debug.Log($"[Merge] 3매치 업그레이드: {blocks[0].Num} → {newNum}");
        }
        else
        {
            // 4매치 이상은 특수 블록 생성
            CreateSpecialBlock(center, blocks[0].Num, matchType);
        }

        // 나머지 블록 제거
        foreach (var block in blocks)
        {
            if (block != center)
            {
                block.Anime.MergeToAndPop(center.transform.position, board.BlockScale, 
                    () => { board.RemoveBlock(block); });

                // 나머지 블록이 모두 제거된 뒤 병합되도록 수정
                center.Anime.ResetScale(board.BlockScale);
            }
        }

        Debug.Log($"[MERGE] {matchType} merged at {center.BoardPos}");
    }

    private Block GetMergeCenter(List<Block> blocks, MatchType type)
    {
        // T자 → 중심 블록이 기준, 일반 라인 → 스왑 방향 기준 등
        return blocks[0]; // 간단 처리 (나중에 정책 따로 뺄 수 있음)
    }

    // 병합 후 특수 블럭 생성
    private void CreateSpecialBlock(Block block, int num, MatchType matchType)
    {
        block.ApplySpecial(matchType);
    }

    // 3매치 병합
    private void Create3MatchBlock()
    {

    }

}
