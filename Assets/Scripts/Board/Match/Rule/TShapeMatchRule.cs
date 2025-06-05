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

    /// <summary>
    /// T자 매치 여부 판단
    /// </summary>
    public bool isMatch(List<Block> group)
    {
        if (group == null || group.Count < requireBlockCount)
            return false;

        var positions = group.Select(b => b.BoardPos).ToHashSet();
        var posToBlock = group.ToDictionary(b => b.BoardPos);

        return TryFindTShapeCenter(positions, posToBlock, out _);
    }

    /// <summary>
    /// 병합에 사용할 T자 블록 5개 반환
    /// -> 외부에서 호출해서 사용
    /// </summary>
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

    /// <summary>
    /// T자 중심과 주변 3방향이 연결된 블록을 찾음
    /// </summary>
    private bool TryFindTShapeCenter(
        HashSet<Vector2Int> positions,
        Dictionary<Vector2Int, Block> posToBlock,
        out List<Block> matchedBlocks)
    {
        foreach (Vector2Int center in positions)
        {
            List<Vector2Int> connectedDirs = new();

            if (positions.Contains(center + Vector2Int.up)) connectedDirs.Add(Vector2Int.up);
            if (positions.Contains(center + Vector2Int.down)) connectedDirs.Add(Vector2Int.down);
            if (positions.Contains(center + Vector2Int.left)) connectedDirs.Add(Vector2Int.left);
            if (positions.Contains(center + Vector2Int.right)) connectedDirs.Add(Vector2Int.right);

            if (connectedDirs.Count >= 3 && posToBlock.ContainsKey(center))
            {
                var result = new List<Block> { posToBlock[center] };

                foreach (var dir in connectedDirs)
                {
                    Vector2Int neighborPos = center + dir;
                    if (posToBlock.TryGetValue(neighborPos, out var neighbor))
                        result.Add(neighbor);
                }

                // 핵심: 빠진 블록이 group에 하나 더 있을 수 있음!
                if (result.Count < 5)
                {
                    // group에서 아직 추가되지 않은 블록을 하나 더 넣는다
                    foreach (var b in posToBlock.Values)
                    {
                        if (!result.Contains(b))
                        {
                            result.Add(b);
                            break;
                        }
                    }
                }

                if (result.Count == 5)
                {
                    matchedBlocks = result;
                    return true;
                }
            }
        }

        matchedBlocks = null;
        return false;
    }
}
