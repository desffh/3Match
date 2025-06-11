using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// � ���� ����Ǵ� �� �Ǵ� (IMatchRule)
/// </summary>

public class MatchManager
{
    private List<IMatchRule> matchRules;

    private MatchMerger matchMerger;

    // ������
    public MatchManager(MatchMerger matchMerger)
    {
        matchRules = new List<IMatchRule>
        {
            // �켱���� �� (7 6 5 4 3)
            new LineMatchRule(5, 7),  // 5 ��ġ
            new TShapeMatchRule(6),   // T ��ġ
            new LShapeMatchRule(5),   // L ��ġ
            new LineMatchRule(4, 4),  // 4 ��ġ
            new LineMatchRule(3, 3),  // 3 ��ġ
        };

        this.matchMerger = matchMerger;
    }

    /// <summary>
    /// �켱���� ������ IMatchRule�� �ϳ��� ȣ��
    /// 
    /// isMatch == true : ��ġ�� �Ǿ��ٸ�
    /// ��ġ�� �� ����Ʈ�� ������ �� ���� ȣ��
    /// </summary>

    public List<(List<Block>, MatchType)> ExtractAllMatches(List<Block> group)
    {
        // ��ġ ����Ʈ�� ���� results ����Ʈ
        List<(List<Block>, MatchType)> results = new();

        // ���� ���� ��ǥ���� 
        HashSet<Vector2Int> used = new();

        foreach (IMatchRule rule in matchRules.OrderByDescending(r => r.priority))
        {
            // �׷���� ��� ��ġ�� �̷���� �� ���� ����
            while (true)
            {
                // �̹� ���� ��� ��ǥ��� ���� 
                List<Block> filtered = group.Where(b => !used.Contains(b.BoardPos)).ToList();
                
                // ������ ���� ������ ���� �ּ�(3) �̸��̶�� break
                if (filtered.Count < 3)
                {
                    break;
                }

                List<Block> match = rule.ExtractMatchBlocks(filtered);

                if (match != null)
                {
                    results.Add((match, rule.matchType));

                    // ��ġ�� ������ ��ǥ ����
                    foreach (Block b in match)
                    {
                        used.Add(b.BoardPos);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        return results;
    }

}
