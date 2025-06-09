using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// T자 모양 매치 판별 및 병합 대상 추출
/// </summary>
public class TShapeMatchRule : IMatchRule
{
    private const int requireBlockCount = 5;
    private MatchType _matchType = MatchType.TShape;
    private int _priority = 7;

    public MatchType matchType => _matchType;
    public int priority => _priority;

    public bool isMatch(List<Block> group)
    {
        if (group == null || group.Count < requireBlockCount)
            return false;

        var positions = group.Select(b => b.BoardPos).ToHashSet();
        var posToBlock = group.ToDictionary(b => b.BoardPos);

        return TryFindTShapeCenter(positions, posToBlock, out _);
    }

    public List<Block> ExtractMatchBlocks(List<Block> group)
    {
        if (group == null || group.Count < requireBlockCount)
            return null;

        var positions = group.Select(b => b.BoardPos).ToHashSet();
        var posToBlock = group.ToDictionary(b => b.BoardPos);

        if (TryFindTShapeCenter(positions, posToBlock, out var result))
            return result;

        return null;
    }

    private static readonly (Vector2Int[] line, Vector2Int[] arms)[] TPatterns =
    {
    (new[] { Vector2Int.up, Vector2Int.up * 2 }, new[] { Vector2Int.left, Vector2Int.right }),
    (new[] { Vector2Int.down, Vector2Int.down * 2 }, new[] { Vector2Int.left, Vector2Int.right }),
    (new[] { Vector2Int.left, Vector2Int.left * 2 }, new[] { Vector2Int.up, Vector2Int.down }),
    (new[] { Vector2Int.right, Vector2Int.right * 2 }, new[] { Vector2Int.up, Vector2Int.down }),
};

    private bool TryFindTShapeCenter(
        HashSet<Vector2Int> positions,
        Dictionary<Vector2Int, Block> posToBlock,
        out List<Block> matchedBlocks)
    {
        foreach (Vector2Int center in positions)
        {
            if (!posToBlock.ContainsKey(center)) continue;

            foreach (var (line, arms) in TPatterns)
            {
                Vector2Int line1 = center + line[0];
                Vector2Int line2 = center + line[1];
                Vector2Int arm1 = center + arms[0];
                Vector2Int arm2 = center + arms[1];

                // 모든 방향에 퍼즐이 있는지 확인
                if (positions.Contains(line1) && positions.Contains(line2) &&
                    positions.Contains(arm1) && positions.Contains(arm2))
                {
                    matchedBlocks = new()
                {
                    posToBlock[center],
                    posToBlock[line1],
                    posToBlock[line2],
                    posToBlock[arm1],
                    posToBlock[arm2]
                };
                    return true;
                }
            }
        }

        matchedBlocks = null;
        return false;
    }


}

