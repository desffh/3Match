using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// summary
// 연속된 매치 리스트 반환
// summary

public class StraightMatch
{
    private Board _board;

    // 생성자 주입
    public StraightMatch(Board board)
    {
        _board = board;
    }

    // 공통 함수로 묶고 매개변수만 달리할 수 있을 거 같음

    public List<Block> ConnectStraightMatch(List<Block> group)
    {
        HashSet<Block> resultSet = new HashSet<Block>();

        // 가로 줄 : 행 (y를 기준으로 그룹화)
        var rowMap = group.GroupBy(b => b.BoardPos.y);
        
        foreach (var row in rowMap)
        {
            // 동일한 행 들을 열(x) 을 기준으로 오름차순 정렬
            List <Block> sorted = row.OrderBy(b => b.BoardPos.x).ToList();
            
            int count = 1;

            for (int i = 1; i < sorted.Count; i++)
            {
                // 현재 열(x) == 이전 열(x) 값 + 1 인지 확인
                if (sorted[i].BoardPos.x == sorted[i - 1].BoardPos.x + 1)
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

        // 세로 줄 : 열(x를 기준으로 묶기) - 딕셔너리
        var colMap = group.GroupBy(b => b.BoardPos.x);
        foreach (var col in colMap)
        {
            List <Block> sorted = col.OrderBy(b => b.BoardPos.y).ToList();
            int count = 1;

            for (int i = 1; i < sorted.Count; i++)
            {
                if (sorted[i].BoardPos.y == sorted[i - 1].BoardPos.y + 1)
                {
                    count++;
                    if (count >= 3)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            // 역순으로 추가
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

        // HashSet -> List 변환 후 반환
        return resultSet.ToList();
    }
}
