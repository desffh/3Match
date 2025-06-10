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
    private const int matchLength = 5;              // �� ��ġ ����
    public MatchType matchType => MatchType.LShape; // ��ġ Ÿ�� (���� ����� Ư�� Ÿ��)
    
    private int Priority = 6;                       // �켱 ����
    public int priority => Priority;


    public LShapeMatchRule(int priority)
    {
        Priority = priority;
    }

    /// <summary>
    /// L�� ��ġ�� �� ����Ʈ�� ��ȯ
    /// </summary>
    /// <param name="group">BFS�� ã�Ƴ� ������ ���� ����Ʈ </param>
    /// <returns>L�� ��ġ�� �� �ִٸ� ��ġ �� ����Ʈ�� ��ȯ</returns>
    
    public List <Block> ExtractMatchBlocks(List <Block> group)
    {
        if (group.Count < matchLength)
        {
            return null;
        }

        // ������ ��ǥ���� ����
        HashSet<Vector2Int> positions = group.Select(b => b.BoardPos).ToHashSet();
        
        // ��ǥ ����� ���� ������ �� ���
        Dictionary<Vector2Int, Block> posToBlock = group.ToDictionary(b => b.BoardPos);

        // L��ġ�� ����Ʈ ��ȯ
        if (LShapeCenterMatch(positions, posToBlock, out List <Block> result))
        {
            return result;
        }

        return null;
    }

    /// <summary>
    /// L�� �߽ɰ� �ֺ� 2������ ����� ����� ã��
    /// </summary>
    
    private bool LShapeCenterMatch(
        HashSet<Vector2Int> positions,
        Dictionary<Vector2Int, Block> posToBlock,
        out List<Block> matchedBlocks)
    {
        // �߽���ǥ�� ����
        foreach (Vector2Int center in positions)
        {
            List<Vector2Int> connectBlock = new();

            // ��ǥ�� ���� �����Ѵٸ� connectBlock ����Ʈ�� ��ǥ �߰�
            if (positions.Contains(center + Vector2Int.up)) connectBlock.Add(Vector2Int.up);
            if (positions.Contains(center + Vector2Int.down)) connectBlock.Add(Vector2Int.down);
            if (positions.Contains(center + Vector2Int.left)) connectBlock.Add(Vector2Int.left);
            if (positions.Contains(center + Vector2Int.right)) connectBlock.Add(Vector2Int.right);

            // �߽ɰ� ����� ��ǥ�� 2�����
            if (connectBlock.Count == 2)
            {
                // �� ������ ���� + �������� Ȯ��
                // Any : ����Ʈ�� ������ �����ϴ� ��Ұ� �ִ� ��
                bool hasHorizontal = connectBlock.Any(dir => dir == Vector2Int.left || dir == Vector2Int.right);
                bool hasVertical = connectBlock.Any(dir => dir == Vector2Int.up || dir == Vector2Int.down);

                // �� ������ ��� �����ؾ� �ϳ��� �ִ� ��
                if (hasHorizontal && hasVertical)
                {
                    // �߽� + 4�� ����� ���� ����Ʈ (�� 5������ Ȯ��)
                    List <Block> result = new();

                    // 1. �߽� �߰�
                    result.Add(posToBlock[center]);

                    // 2. ����� �� 4�� �߰�
                    foreach (Vector2Int dir in connectBlock)
                    {
                        // L�� ���� ���� 2ĭ�� �پ� �����Ƿ� 1ĭ, 2ĭ �̵�
                        for (int i = 1; i <= 2; i++)
                        {
                            Vector2Int next = center + dir * i;
                            
                            // ��ǥ��(Key)�� �� ã��(Value)
                            if (posToBlock.TryGetValue(next, out Block b))
                            {
                                result.Add(b);
                            }
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
