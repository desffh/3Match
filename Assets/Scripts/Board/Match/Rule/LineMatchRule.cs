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
    private int matchLength;      // 라인 매치 길이
    private MatchType MatchType;  // 매치 타입 (블럭에 저장될 특수 타입)
    private int Priority;         // 우선 순위
    
    public MatchType matchType => MatchType;
    public int priority => Priority;

    public LineMatchRule(int matchLength, int priority)
    {
        this.matchLength = matchLength;
        Priority = priority;
    }

    /// <summary>
    /// 라인 매치된 블럭 리스트를 반환
    /// </summary>
    /// <param name="group">BFS로 찾아낸 인접한 숫자 리스트 </param>
    /// <returns>매치된 게 있다면 매치 블럭 리스트를 반환</returns>

    public List<Block> ExtractMatchBlocks(List<Block> group)
    {
        // 매치된 게 있다면 true를 반환하여 match 리스트 return 
        if (FirstLineMatch(group, out List <Block> match))
        {
            return match;
        }

        return null;
    }

    private bool FirstLineMatch(List<Block> group, out List<Block> match)
    {
        match = null;

        // 가로(행) 검사 -> y를 기준으로 그룹화. (동일한 y행 끼리 그룹화)
        var horizontal = group.GroupBy(b => b.BoardPos.y);

        foreach (var line in horizontal)
        {
            // 각 그룹을 x를 기준으로 오름차순으로 정렬 (x 열)
            List <Block> sorted = line.OrderBy(b => b.BoardPos.x).ToList();


            // sorted.Count               : 정렬된 블록 개수
            // matchLength                : 연속으로 매치돼야 하는 최소 길이
            // sorted.Count - matchLength : 유효한 시작점의 최대값(인덱스 범위 오류 방지)

            for (int i = 0; i <= sorted.Count - matchLength; i++)
            {
                bool isMatch = true;

                for (int j = 1; j < matchLength; j++)
                {
                    if (sorted[i + j].BoardPos.x != sorted[i + j - 1].BoardPos.x + 1)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch) // 만약 매치된 게 있다면
                {
                    MatchType = GetMatchType(matchLength, true); // 매치 타입 지정 (true = 가로Row)
                    match = sorted.GetRange(i, matchLength);     // 매치 범위 리스트에 저장 후 반환
                    return true;
                }
            }
        }

        // 세로(열) 검사 -> x를 기준으로 그룹화. (동일한 x열 끼리 그룹화)
        var vertical = group.GroupBy(b => b.BoardPos.x);
        foreach (var line in vertical)
        {
            List <Block> sorted = line.OrderBy(b => b.BoardPos.y).ToList();
            for (int i = 0; i <= sorted.Count - matchLength; i++)
            {
                bool isMatch = true;
                for (int j = 1; j < matchLength; j++)
                {
                    if (sorted[i + j].BoardPos.y != sorted[i + j - 1].BoardPos.y + 1)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    MatchType = GetMatchType(matchLength, false); // 매치 타입 지정 (false = 세로Col)
                    match = sorted.GetRange(i, matchLength);
                    return true;
                }
            }
        }
        
        // 매치된 게 없다면 false 
        return false;
    }

    private MatchType GetMatchType(int length, bool isRow)
    {
        string matchtype;

        if(isRow == true)
            matchtype = "Row";
        else
            matchtype = "Col";

        string enumName = $"Line{length}_{matchtype}";
        
        // 열거형에 타입이 있다면 반환
        if (Enum.TryParse(enumName, out MatchType type))
        {
            return type;
        }

        return MatchType.None;
    }
    
}
