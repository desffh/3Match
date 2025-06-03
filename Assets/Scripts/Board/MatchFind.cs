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

                // 가로 검사
                List<Block> horizontal = FindLineMatch(blocks, x, y, Vector2Int.right);
                if (horizontal.Count >= 3)
                {
                    foreach (var b in horizontal)
                        visited[b.BoardPos.y, b.BoardPos.x] = true;

                    MergeBlocks(horizontal);
                }

                // 세로 검사
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
    /// 특정 방향으로 같은 숫자의 블록을 연속해서 찾아냄
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

        // 아래쪽에 있는 블록을 기준
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
                        // 모든 애니메이션 끝난 뒤 실행됨
                        BlockData upgradedData = board.BlockDataManager.Get(newValue);

                        if (upgradedData != null)
                        {
                            main.Init(upgradedData);
                            main.Anime.ResetScale(board.BlockScale);
                        }
                        else
                        {
                            Debug.LogWarning($"BlockDataManager에 {newValue} 값에 대한 데이터가 없습니다.");
                        }
                    }
                });
            }
        }

        // main 혼자면 병합 애니메이션 없으므로 바로 처리
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
                Debug.LogWarning($"BlockDataManager에 {newValue} 값에 대한 데이터가 없습니다.");
            }
        }
    }

}
