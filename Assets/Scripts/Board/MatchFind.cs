using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFind : MonoBehaviour
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    private void OnEnable()
    {
        Board.OnmatchFind += CheckAllMatches;
    }

    private void OnDisable()
    {
        Board.OnmatchFind -= CheckAllMatches;
    }

    private void CheckAllMatches()
    {
        Block[,] blocks = board.Blocks;
        int row = board.Row;
        int col = board.Col;

        bool[,] visited = new bool[row, col];

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                if (blocks[y, x] == null || visited[y, x])
                    continue;

                // ���� �˻�
                List<Block> horizontal = FindLineMatch(blocks, x, y, Vector2Int.right);
                if (horizontal.Count >= 3)
                {
                    foreach (var b in horizontal)
                        visited[b.BoardPos.y, b.BoardPos.x] = true;

                    MergeBlocks(horizontal);
                }

                // ���� �˻�
                List<Block> vertical = FindLineMatch(blocks, x, y, Vector2Int.up);
                if (vertical.Count >= 3)
                {
                    foreach (var b in vertical)
                        visited[b.BoardPos.y, b.BoardPos.x] = true;

                    MergeBlocks(vertical);
                }
            }
        }
    }

    /// <summary>
    /// Ư�� �������� ���� ������ ����� �����ؼ� ã�Ƴ�
    /// </summary>
    private List<Block> FindLineMatch(Block[,] blocks, int startX, int startY, Vector2Int dir)
    {
        List<Block> result = new List<Block>();
        int num = blocks[startY, startX].Num;

        int x = startX;
        int y = startY;

        while (x >= 0 && x < board.Col && y >= 0 && y < board.Row)
        {
            Block b = blocks[y, x];
            if (b == null || b.Num != num)
                break;

            result.Add(b);
            x += dir.x;
            y += dir.y;
        }

        return result;
    }

    private void MergeBlocks(List<Block> matched)
    {
        if (matched == null || matched.Count == 0) return;

        // �Ʒ��ʿ� �ִ� ����� ����
        Block main = matched[0];
        foreach (var b in matched)
        {
            if (b.BoardPos.y > main.BoardPos.y)
                main = b;
        }

        int newValue = main.Num + 1;
        int pendingAnimations = 0;

        foreach (var b in matched)
        {
            if (b != main)
            {
                pendingAnimations++;
                Vector3 targetPos = board.GetWorldPosition(main.BoardPos.x, main.BoardPos.y);

                b.Anime.MergeToAndPop(targetPos,board.BlockScale, () =>
                {
                    pendingAnimations--;

                    if (pendingAnimations == 0)
                    {
                        // ��� �ִϸ��̼� ���� �� �����
                        BlockData upgradedData = board.BlockDataManager.Get(newValue);

                        if (upgradedData != null)
                        {
                            main.Init(upgradedData);
                            main.Anime.ResetScale(board.BlockScale);
                        }
                        else
                        {
                            Debug.LogWarning($"BlockDataManager�� {newValue} ���� ���� �����Ͱ� �����ϴ�.");
                        }
                    }
                });
            }
        }

        // main ȥ�ڸ� ���� �ִϸ��̼� �����Ƿ� �ٷ� ó��
        if (pendingAnimations == 0)
        {
            BlockData upgradedData = board.BlockDataManager.Get(newValue);

            if (upgradedData != null)
            {
                main.Init(upgradedData);
                main.Anime.ResetScale(board.BlockScale);
            }
            else
            {
                Debug.LogWarning($"BlockDataManager�� {newValue} ���� ���� �����Ͱ� �����ϴ�.");
            }
        }
    }

}
