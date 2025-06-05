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
    private int _matchLength;   // �� �� ���� (3, 4, 5)
    private MatchType _matchType;        // enum���� �����
    private int _priority;               // 5��ġ > 4��ġ > 3��ġ

    public MatchType matchType => _matchType;
    public int priority => _priority;

    // ������ ����
    public LineMatchRule(int matchLength, int priority)
    {
        _matchLength = matchLength;
        _priority = priority;
    }

    /// <summary>
    /// 1. ���� ���� ���� ��ġ Ȯ��
    /// 2. enum ��ġ Ÿ�� ����
    /// 3. true ��ȯ
    /// </summary>

    public bool isMatch(List<Block> group)
    {
        if (group == null || group.Count != _matchLength)
            return false;

        // ��ǥ �������� ����
        var ordered = group.OrderBy(b => b.BoardPos.x).ToList();

        // ���� ��ġ Ȯ�� (��� ����� y���� ���ƾ� ���� ��, x���� �������̾�� ��)
        bool isHorizontal = group.All(b => b.BoardPos.y == group[0].BoardPos.y) &&
                            IsConsecutive(ordered.Select(b => b.BoardPos.x).ToList());

        if (isHorizontal)
        {
            _matchType = GetMatchType(_matchLength, isRow: true);
            return true;
        }

        // ���� ��ġ Ȯ�� (��� ����� x���� ���ƾ� ���� ��, y���� �������̾�� ��)
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
    /// ���ڿ� -> enum���� ��ȯ
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
    /// x y ��ǥ������ �������� ���ĵ� ����Ʈ�� ���ӵ� �������� Ȯ��
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
    /// �ܺο��� ȣ���ؼ� ���
    /// -> IsMatch�� true�� ��� ��ȯ
    /// </summary>

    public List<Block> ExtractMatchBlocks(List<Block> group)
    {
        return group;
    }

}
