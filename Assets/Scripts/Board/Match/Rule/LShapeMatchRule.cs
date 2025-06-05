using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

/// <summary>
/// L�� ��� ��ġ �Ǻ� �� ���� ��� ����
/// </summary>
public class LShapeMatchRule : IMatchRule
{
    private const int requireBlockCount = 5;
    private MatchType _matchType = MatchType.LShape;
    private int _priority = 6;

    public MatchType matchType => _matchType;
    public int priority => _priority;

    /// <summary>
    /// L�� ��ġ ���� �Ǵ�
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
    /// ���տ� ����� L�� ��� 5�� ��ȯ
    /// -> �ܺο��� ȣ���ؼ� ���
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
    /// L�� �߽ɰ� �ֺ� 2������ ����� ����� ã��
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

            if (connectedDirs.Count == 2)
            {
                // �� ������ ���� + �������� Ȯ��
                bool hasHorizontal = connectedDirs.Any(dir => dir == Vector2Int.left || dir == Vector2Int.right);
                bool hasVertical = connectedDirs.Any(dir => dir == Vector2Int.up || dir == Vector2Int.down);

                if (hasHorizontal && hasVertical)
                {
                    // �߽� + 4�� ��� ���� (�� 5������ Ȯ��)
                    List<Block> result = new();
                    result.Add(posToBlock[center]);

                    foreach (var dir in connectedDirs)
                    {
                        // L�� ���� ���� 2ĭ�� �پ� �����Ƿ� 1ĭ, 2ĭ �̵�
                        for (int i = 1; i <= 2; i++)
                        {
                            Vector2Int next = center + dir * i;
                            if (posToBlock.TryGetValue(next, out var b))
                                result.Add(b);
                        }
                    }

                    if (result.Count == 5)
                    {
                        matchedBlocks = result;
                        return true;
                    }
                }
            }
        }
        matchedBlocks = null;
        return false;
    }
}
