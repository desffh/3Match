using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Board 이벤트 구독자 -> 매치 실행

public class MatchFind : MonoBehaviour
{
    private Board board;

    FindConnectMatch findConnectMatch;

    StraightMatch straightMatch;

    private void Awake()
    {
        board = GetComponent<Board>();

        findConnectMatch = new FindConnectMatch(board);

        straightMatch = new StraightMatch(board);
    }

    private void OnEnable()
    {
        Board.OnmatchFind += CheckAllMatches;
    }

    private void OnDisable()
    {
        Board.OnmatchFind -= CheckAllMatches;
    }

    public void CheckAllMatches(Block swapBlock)
    {
        Block[,] blocks = board.Blocks;
        int row = board.Row;
        int col = board.Col;
        bool[,] visited = new bool[row, col];

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                // 이미 방문했다면 건너뛰기 
                if (blocks[y, x] == null || visited[y, x])
                    continue;

                // 인접한 모든 매치 찾기 (동일한 숫자끼리의 덩어리)
                List<Block> group = findConnectMatch.FindConnectedMatch(blocks, x, y, visited);

                if(group.Count <= 1)
                {
                    // 되돌리기 애니메이션
                }

                // 연속된 매치 찾기
                List<Block> validGroup = straightMatch.ConnectStraightMatch(group);

                // 연속된 매치 갯수가 3개 이상이라면 
                if (validGroup.Count >= 3)
                {
                    // 모양이 T 라면
                    bool isT = IsTShapeMatch(validGroup);
                    
                    // T : T자 병합 실행
                    //일반: 일반 병합 실행
                    MergeBlocks(validGroup, swapBlock, isT);
                }
            }
        }
    }

    // 연속된 블럭 리스트 matched에서 겹치는 블럭 찾기

    private bool IsTShapeMatch(List<Block> matched)
    {
        Dictionary<int, List<int>> rowMap = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> colMap = new Dictionary<int, List<int>>();
        HashSet<Vector2Int> matchedSet = new HashSet<Vector2Int>();

        foreach (Block b in matched)
        {
            int x = b.BoardPos.x;
            int y = b.BoardPos.y;

            if (!rowMap.ContainsKey(y)) rowMap[y] = new List<int>();
            rowMap[y].Add(x);

            if (!colMap.ContainsKey(x)) colMap[x] = new List<int>();
            colMap[x].Add(y);

            matchedSet.Add(new Vector2Int(x, y));
        }

        List<Vector2Int> horizontalCenters = new List<Vector2Int>();
        foreach (var kvp in rowMap)
        {
            var xs = kvp.Value;
            xs.Sort();

            for (int i = 0; i < xs.Count - 2; i++)
            {
                if (xs[i + 1] == xs[i] + 1 && xs[i + 2] == xs[i] + 2)
                {
                    int centerX = xs[i + 1];
                    horizontalCenters.Add(new Vector2Int(centerX, kvp.Key));
                }
            }
        }

        foreach (var kvp in colMap)
        {
            var ys = kvp.Value;
            ys.Sort();

            for (int i = 0; i < ys.Count - 2; i++)
            {
                if (ys[i + 1] == ys[i] + 1 && ys[i + 2] == ys[i] + 2)
                {
                    int centerY = ys[i + 1];
                    Vector2Int candidate = new Vector2Int(kvp.Key, centerY);
                    if (horizontalCenters.Contains(candidate))
                        return true;
                }
            }
        }

        return false;
    }

    private Block GetCrossCenterBlock(List<Block> group)
    {
        Dictionary<int, List<Block>> rowMap = new Dictionary<int, List<Block>>();
        Dictionary<int, List<Block>> colMap = new Dictionary<int, List<Block>>();

        foreach (var b in group)
        {
            int x = b.BoardPos.x;
            int y = b.BoardPos.y;

            if (!rowMap.ContainsKey(y)) rowMap[y] = new List<Block>();
            rowMap[y].Add(b);

            if (!colMap.ContainsKey(x)) colMap[x] = new List<Block>();
            colMap[x].Add(b);
        }

        List<Block> horizontal = null;
        List<Block> vertical = null;

        foreach (var row in rowMap)
        {
            if (row.Value.Count >= 3)
            {
                var xs = row.Value.Select(b => b.BoardPos.x).ToList();
                xs.Sort();
                for (int i = 0; i < xs.Count - 2; i++)
                {
                    if (xs[i + 1] == xs[i] + 1 && xs[i + 2] == xs[i] + 2)
                    {
                        horizontal = row.Value;
                        break;
                    }
                }
            }
        }

        foreach (var col in colMap)
        {
            if (col.Value.Count >= 3)
            {
                var ys = col.Value.Select(b => b.BoardPos.y).ToList();
                ys.Sort();
                for (int i = 0; i < ys.Count - 2; i++)
                {
                    if (ys[i + 1] == ys[i] + 1 && ys[i + 2] == ys[i] + 2)
                    {
                        vertical = col.Value;
                        break;
                    }
                }
            }
        }

        if (horizontal != null && vertical != null)
        {
            foreach (var h in horizontal)
            {
                foreach (var v in vertical)
                {
                    if (h == v)
                        return h;
                }
            }
        }

        return GetLowestBlock(group);
    }

    private Block GetLowestBlock(List<Block> group)
    {
        Block lowest = group[0];
        foreach (var b in group)
        {
            if (b.BoardPos.y > lowest.BoardPos.y)
            {
                lowest = b;
            }
        }
        return lowest;
    }

    private void MergeBlocks(List<Block> matched, Block swapBlock, bool isTShape)
    {
        if (matched == null || matched.Count == 0) return;

        Block main = null;

        if (isTShape)
        {
            main = GetCrossCenterBlock(matched);
        }
        else
        {
            if (matched.Contains(swapBlock))
                main = swapBlock;
            else
                main = GetLowestBlock(matched);
        }

        int newValue = main.Num + 1;
        int pendingAnimations = 0;

        foreach (var b in matched)
        {
            if (b != main)
            {
                pendingAnimations++;
                Vector3 targetPos = board.GetWorldPosition(main.BoardPos.x, main.BoardPos.y);

                b.Anime.MergeToAndPop(targetPos, board.BlockScale, () =>
                {
                    pendingAnimations--;
                    if (pendingAnimations == 0)
                    {
                        ApplyMergedBlock(main, newValue);
                    }
                });
            }
        }

        if (pendingAnimations == 0)
        {
            ApplyMergedBlock(main, newValue);
        }
    }


    // 블럭 데이터 업그레이드 & 병합
    private void ApplyMergedBlock(Block main, int newValue)
    {
        BlockData upgradedData = board.BlockDataManager.Get(newValue);
        if (upgradedData != null)
        {
            main.Init(upgradedData);
            main.Anime.ResetScale(board.BlockScale);
        }
        else
        {
            Debug.Log("6매치 불가능");
        }
    }

}
