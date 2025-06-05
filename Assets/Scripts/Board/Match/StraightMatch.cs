using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 연속된 매치 리스트 반환
/// </summary>

public class StraightMatch
{
    private Board _board;

    // 생성자 주입
    public StraightMatch(Board board)
    {
        _board = board;
    }

    // 공통 함수로 묶고 매개변수만 달리할 수 있을 거 같음

    /// <summary>
    /// map : x와 y를 기준으로 동일한 것끼리 그룹화 
    ///
    /// foreach(line) : 모아둔 그룹화 중 개별 그룹에 접근
    /// </summary>


    public List<Block> ConnectStraightMatch(List<Block> group)
    {
        HashSet<Block> resultSet = new HashSet<Block>();

        // 가로 매칭
        MatchStraightLines(group, b => b.BoardPos.y, b => b.BoardPos.x, pos => pos.x, resultSet);

        // 세로 매칭
        MatchStraightLines(group, b => b.BoardPos.x, b => b.BoardPos.y, pos => pos.y, resultSet);

        return resultSet.ToList(); // 연속된 매치 블록 리스트 반환
    }


    private void MatchStraightLines
    (
        // 매개변수
        List<Block> group,
        Func<Block, int> groupBySelector, // 그룹 정렬 기준
        Func<Block, int> orderBySelector, // 오름차순 정렬 기준
        Func<Vector2Int, int> compareSelector, // 좌표 비교
        HashSet<Block> resultSet
    )
    {
        var map = group.GroupBy(groupBySelector);

        foreach (var line in map)
        {
            var sorted = line.OrderBy(orderBySelector).ToList();
            int count = 1;

            for (int i = 1; i < sorted.Count; i++)
            {
                if (compareSelector(sorted[i].BoardPos) == compareSelector(sorted[i - 1].BoardPos) + 1)
                {
                    count++;
                    if (count >= 3)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            resultSet.Add(sorted[i - j]);
                        }
                    }
                }
                else
                {
                    count = 1;
                }
            }
        }
    }
}
