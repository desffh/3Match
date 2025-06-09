using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// summary
/// 가로 세로 직선 매치 확인
/// </summary>

public class LineMatchRule : IMatchRule
{

    private int _matchLength;
    private MatchType _matchType;
    private int _priority;
    
    public MatchType matchType => _matchType;
    public int priority => _priority;

    public LineMatchRule(int matchLength, int priority)
    {
        _matchLength = matchLength;
        _priority = priority;
    }

    public bool isMatch(List<Block> group)
    {
        if (group == null || group.Count < _matchLength)
            return false;

        return TryFindFirstLineMatch(group, out _);
    }

    public List<Block> ExtractMatchBlocks(List<Block> group)
    {
        if (TryFindFirstLineMatch(group, out var match))
            return match;

        return null;
    }

    private bool TryFindFirstLineMatch(List<Block> group, out List<Block> match)
    {
        match = null;

        // 가로 검사
        var horizontal = group.GroupBy(b => b.BoardPos.y);
        foreach (var line in horizontal)
        {
            var sorted = line.OrderBy(b => b.BoardPos.x).ToList();
            for (int i = 0; i <= sorted.Count - _matchLength; i++)
            {
                bool isMatch = true;
                for (int j = 1; j < _matchLength; j++)
                {
                    if (sorted[i + j].BoardPos.x != sorted[i + j - 1].BoardPos.x + 1)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    _matchType = GetMatchType(_matchLength, true);
                    match = sorted.GetRange(i, _matchLength);
                    return true;
                }
            }
        }

        // 세로 검사
        var vertical = group.GroupBy(b => b.BoardPos.x);
        foreach (var line in vertical)
        {
            var sorted = line.OrderBy(b => b.BoardPos.y).ToList();
            for (int i = 0; i <= sorted.Count - _matchLength; i++)
            {
                bool isMatch = true;
                for (int j = 1; j < _matchLength; j++)
                {
                    if (sorted[i + j].BoardPos.y != sorted[i + j - 1].BoardPos.y + 1)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    _matchType = GetMatchType(_matchLength, false);
                    match = sorted.GetRange(i, _matchLength);
                    return true;
                }
            }
        }

        return false;
    }

    private MatchType GetMatchType(int length, bool isRow)
    {
        string enumName = $"Line{length}_{(isRow ? "Row" : "Col")}";
        if (Enum.TryParse(enumName, out MatchType parsed))
            return parsed;

        return MatchType.None;
    }
    
}
