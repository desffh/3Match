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
    private int _matchLength;   // 몇 개 연속 (3, 4, 5)
    private MatchType _matchType;        // enum으로 변경됨
    private int _priority;               // 5매치 > 4매치 > 3매치

    public MatchType matchType => _matchType;
    public int priority => _priority;

    // 생성자 주입
    public LineMatchRule(int matchLength, int priority)
    {
        _matchLength = matchLength;
        _priority = priority;
    }

    /// <summary>
    /// 1. 가로 세로 직선 매치 확인
    /// 2. enum 매치 타입 저장
    /// 3. true 반환
    /// </summary>

    public bool isMatch(List<Block> group)
    {
        if (group == null || group.Count != _matchLength)
            return false;

        // 좌표 기준으로 정렬
        var ordered = group.OrderBy(b => b.BoardPos.x).ToList();

        // 가로 매치 확인 (모든 블록의 y값이 같아야 같은 행, x값은 연속적이어야 함)
        bool isHorizontal = group.All(b => b.BoardPos.y == group[0].BoardPos.y) &&
                            IsConsecutive(ordered.Select(b => b.BoardPos.x).ToList());

        if (isHorizontal)
        {
            _matchType = GetMatchType(_matchLength, isRow: true);
            return true;
        }

        // 세로 매치 확인 (모든 블록의 x값이 같아야 같은 열, y값은 연속적이어야 함)
        ordered = group.OrderBy(b => b.BoardPos.y).ToList();
        bool isVertical = group.All(b => b.BoardPos.x == group[0].BoardPos.x) &&
                          IsConsecutive(ordered.Select(b => b.BoardPos.y).ToList());

        if (isVertical)
        {
            _matchType = GetMatchType(_matchLength, isRow: false);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 문자열 -> enum으로 변환
    /// </summary>

    private MatchType GetMatchType(int length, bool isRow)
    {
        string enumName = $"Line{length}_{(isRow ? "Row" : "Col")}";
        
        if (Enum.TryParse(enumName, out MatchType parsed))
        {
            return parsed;
        }
        return MatchType.None;
    }

    /// <summary>
    /// x y 좌표값으로 오름차순 정렬된 리스트가 연속된 숫자인지 확인
    /// </summary>

    private bool IsConsecutive(List<int> values)
    {
        for (int i = 1; i < values.Count; i++)
        {
            if (values[i] != values[i - 1] + 1)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 외부에서 호출해서 사용
    /// -> IsMatch가 true일 경우 반환
    /// </summary>

    public List<Block> ExtractMatchBlocks(List<Block> group)
    {
        return group;
    }

}
