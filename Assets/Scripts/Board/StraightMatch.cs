using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// summary
// ���ӵ� ��ġ ����Ʈ ��ȯ
// summary

public class StraightMatch
{
    private Board _board;

    // ������ ����
    public StraightMatch(Board board)
    {
        _board = board;
    }

    // ���� �Լ��� ���� �Ű������� �޸��� �� ���� �� ����

    public List<Block> ConnectStraightMatch(List<Block> group)
    {
        HashSet<Block> resultSet = new HashSet<Block>();

        // ���� �� : �� (y�� �������� �׷�ȭ)
        var rowMap = group.GroupBy(b => b.BoardPos.y);
        
        foreach (var row in rowMap)
        {
            // ������ �� ���� ��(x) �� �������� �������� ����
            List <Block> sorted = row.OrderBy(b => b.BoardPos.x).ToList();
            
            int count = 1;

            for (int i = 1; i < sorted.Count; i++)
            {
                // ���� ��(x) == ���� ��(x) �� + 1 ���� Ȯ��
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

        // ���� �� : ��(x�� �������� ����) - ��ųʸ�
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
                            // �������� �߰�
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

        // HashSet -> List ��ȯ �� ��ȯ
        return resultSet.ToList();
    }
}
