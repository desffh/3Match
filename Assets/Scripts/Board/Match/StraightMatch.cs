using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// ���ӵ� ��ġ ����Ʈ ��ȯ
/// </summary>

public class StraightMatch
{
    private Board _board;

    // ������ ����
    public StraightMatch(Board board)
    {
        _board = board;
    }

    // ���� �Լ��� ���� �Ű������� �޸��� �� ���� �� ����

    /// <summary>
    /// map : x�� y�� �������� ������ �ͳ��� �׷�ȭ 
    ///
    /// foreach(line) : ��Ƶ� �׷�ȭ �� ���� �׷쿡 ����
    /// </summary>


    public List<Block> ConnectStraightMatch(List<Block> group)
    {
        HashSet<Block> resultSet = new HashSet<Block>();

        // ���� ��Ī
        MatchStraightLines(group, b => b.BoardPos.y, b => b.BoardPos.x, pos => pos.x, resultSet);

        // ���� ��Ī
        MatchStraightLines(group, b => b.BoardPos.x, b => b.BoardPos.y, pos => pos.y, resultSet);

        return resultSet.ToList(); // ���ӵ� ��ġ ��� ����Ʈ ��ȯ
    }


    private void MatchStraightLines
    (
        // �Ű�����
        List<Block> group,
        Func<Block, int> groupBySelector, // �׷� ���� ����
        Func<Block, int> orderBySelector, // �������� ���� ����
        Func<Vector2Int, int> compareSelector, // ��ǥ ��
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
