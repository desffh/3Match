using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// T�� ��� ��ġ �Ǻ� �� ���� ��� ����
/// </summary>
/// 
public class TShapeMatchRule : IMatchRule
{      
    private const int matchLength = 5;              // �� ��ġ ����
    public MatchType matchType => MatchType.TShape; // ��ġ Ÿ�� (���� ����� Ư�� Ÿ��)
    
    private int Priority;                           // �켱 ����
    public int priority => Priority;

    public TShapeMatchRule(int priority)
    {
        Priority = priority;
    }

    /// <summary>
    /// T�� ��ġ�� �� ����Ʈ�� ��ȯ
    /// </summary>
    /// <param name="group">BFS�� ã�Ƴ� ������ ���� ����Ʈ </param>
    /// <returns>T�� ��ġ�� �� �ִٸ� ��ġ �� ����Ʈ�� ��ȯ</returns>

    public List <Block> ExtractMatchBlocks(List <Block> group)
    {
        if (group.Count < matchLength)
        {
            return null;
        }

        // ������ ��ǥ���� ����
        HashSet <Vector2Int> positions = group.Select(b => b.BoardPos).ToHashSet();
        
        // ��ǥ ����� ���� ������ �� ���
        Dictionary<Vector2Int, Block> posToBlock = group.ToDictionary(b => b.BoardPos);

        // T��ġ�� ����Ʈ ��ȯ
        if (TShapeCenterMatch(positions, posToBlock, out List <Block> result))
        {
            return result;
        }
        
        return null;
    }

    private static readonly (Vector2Int[] line, Vector2Int[] arms)[] TPatterns =
    {
    (new[] { Vector2Int.up, Vector2Int.up * 2 }, new[] { Vector2Int.left, Vector2Int.right }),
    (new[] { Vector2Int.down, Vector2Int.down * 2 }, new[] { Vector2Int.left, Vector2Int.right }),
    (new[] { Vector2Int.left, Vector2Int.left * 2 }, new[] { Vector2Int.up, Vector2Int.down }),
    (new[] { Vector2Int.right, Vector2Int.right * 2 }, new[] { Vector2Int.up, Vector2Int.down }),
};

    private bool TShapeCenterMatch
    (
        HashSet<Vector2Int> positions,
        Dictionary<Vector2Int, Block> posToBlock,
        out List<Block> matchedBlocks
    )
    {
        foreach (Vector2Int center in positions)
        {
            foreach (var (line, arms) in TPatterns)
            {
                // �߽��� �������� T�� ��ǥ ����
                Vector2Int line1 = center + line[0];
                Vector2Int line2 = center + line[1];
                Vector2Int arm1 = center + arms[0];
                Vector2Int arm2 = center + arms[1];

                // ��� ���⿡ ���� �ִ��� Ȯ�� (������ ��ǥ�鿡 ������ �ִ���)
                if (positions.Contains(line1) && positions.Contains(line2) &&
                    positions.Contains(arm1) && positions.Contains(arm2))
                {
                    // �߽� ����, ����� �� ����Ʈ�� ���� 
                    matchedBlocks = new()
                    {
                        posToBlock[center],
                        posToBlock[line1],
                        posToBlock[line2],
                        posToBlock[arm1],
                        posToBlock[arm2]
                    };
                    return true;
                }
            }
        }

        matchedBlocks = null;
        return false;
    }


}

