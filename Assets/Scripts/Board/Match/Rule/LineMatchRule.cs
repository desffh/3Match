using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// summary
/// ���� ���� ���� ��ġ Ȯ��
/// </summary>

public class LineMatchRule : IMatchRule
{
    private int matchLength;      // ���� ��ġ ����
    private MatchType MatchType;  // ��ġ Ÿ�� (���� ����� Ư�� Ÿ��)
    private int Priority;         // �켱 ����
    
    public MatchType matchType => MatchType;
    public int priority => Priority;

    public LineMatchRule(int matchLength, int priority)
    {
        this.matchLength = matchLength;
        Priority = priority;
    }

    /// <summary>
    /// ���� ��ġ�� �� ����Ʈ�� ��ȯ
    /// </summary>
    /// <param name="group">BFS�� ã�Ƴ� ������ ���� ����Ʈ </param>
    /// <returns>��ġ�� �� �ִٸ� ��ġ �� ����Ʈ�� ��ȯ</returns>

    public List<Block> ExtractMatchBlocks(List<Block> group)
    {
        // ��ġ�� �� �ִٸ� true�� ��ȯ�Ͽ� match ����Ʈ return 
        if (FirstLineMatch(group, out List <Block> match))
        {
            return match;
        }

        return null;
    }

    private bool FirstLineMatch(List<Block> group, out List<Block> match)
    {
        match = null;

        // ����(��) �˻� -> y�� �������� �׷�ȭ. (������ y�� ���� �׷�ȭ)
        var horizontal = group.GroupBy(b => b.BoardPos.y);

        foreach (var line in horizontal)
        {
            // �� �׷��� x�� �������� ������������ ���� (x ��)
            List <Block> sorted = line.OrderBy(b => b.BoardPos.x).ToList();


            // sorted.Count               : ���ĵ� ��� ����
            // matchLength                : �������� ��ġ�ž� �ϴ� �ּ� ����
            // sorted.Count - matchLength : ��ȿ�� �������� �ִ밪(�ε��� ���� ���� ����)

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

                if (isMatch) // ���� ��ġ�� �� �ִٸ�
                {
                    MatchType = GetMatchType(matchLength, true); // ��ġ Ÿ�� ���� (true = ����Row)
                    match = sorted.GetRange(i, matchLength);     // ��ġ ���� ����Ʈ�� ���� �� ��ȯ
                    return true;
                }
            }
        }

        // ����(��) �˻� -> x�� �������� �׷�ȭ. (������ x�� ���� �׷�ȭ)
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
                    MatchType = GetMatchType(matchLength, false); // ��ġ Ÿ�� ���� (false = ����Col)
                    match = sorted.GetRange(i, matchLength);
                    return true;
                }
            }
        }
        
        // ��ġ�� �� ���ٸ� false 
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
        
        // �������� Ÿ���� �ִٸ� ��ȯ
        if (Enum.TryParse(enumName, out MatchType type))
        {
            return type;
        }

        return MatchType.None;
    }
    
}
