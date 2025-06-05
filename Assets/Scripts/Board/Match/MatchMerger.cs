using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ����
/// </summary>

public class MatchMerger
{
    private Board board;

    // ������ ���� 
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

        // �߽� ��� ���
        Block center = GetMergeCenter(blocks, matchType);
        if (matchType == MatchType.Line3_Row || matchType == MatchType.Line3_Col)
        {
            // 3��ġ: Num ���׷��̵�
            int newNum = blocks[0].Num + 1;
            
            if (blocks[0].Num == 6)
            {
                newNum = 6;
            }

            BlockData blockData = board.BlockDataManager.Get(newNum);

            center.Init(blockData);
            center.ApplySpecial(matchType);

            Debug.Log($"[Merge] 3��ġ ���׷��̵�: {blocks[0].Num} �� {newNum}");
        }
        else
        {
            // 4��ġ �̻��� Ư�� ��� ����
            CreateSpecialBlock(center, blocks[0].Num, matchType);
        }

        // ������ ��� ����
        foreach (var block in blocks)
        {
            if (block != center)
            {
                block.Anime.MergeToAndPop(center.transform.position, board.BlockScale, 
                    () => { board.RemoveBlock(block); });

                // ������ ����� ��� ���ŵ� �� ���յǵ��� ����
                center.Anime.ResetScale(board.BlockScale);
            }
        }

        Debug.Log($"[MERGE] {matchType} merged at {center.BoardPos}");
    }

    private Block GetMergeCenter(List<Block> blocks, MatchType type)
    {
        // T�� �� �߽� ����� ����, �Ϲ� ���� �� ���� ���� ���� ��
        return blocks[0]; // ���� ó�� (���߿� ��å ���� �� �� ����)
    }

    // ���� �� Ư�� �� ����
    private void CreateSpecialBlock(Block block, int num, MatchType matchType)
    {
        block.ApplySpecial(matchType);
    }

    // 3��ġ ����
    private void Create3MatchBlock()
    {

    }

}
