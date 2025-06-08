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

    public MatchManager(MatchMerger matchMerger)
    {
        matchRules = new List<IMatchRule>
        {
            // �켱���� ��
            new TShapeMatchRule(),   // T ��ġ
            new LShapeMatchRule(),   // L ��ġ
            new LineMatchRule(5, 5), // 5 ��ġ
            new LineMatchRule(4, 4), // 4 ��ġ
            new LineMatchRule(3, 3), // 3 ��ġ
        };

        this.matchMerger = matchMerger;
    }

    public bool ProcessMatch(List<Block> group)
    {
        foreach (IMatchRule rule in matchRules.OrderByDescending(r => r.priority))
        {
            if (rule.isMatch(group))
            {
                List <Block> matched = rule.ExtractMatchBlocks(group);

                if (matched != null && matched.Count > 0)
                {
                    // ������ �� ����Ʈ�� ���ڷ� �ѱ�
                    return matchMerger.Merge(matched, rule.matchType);
                }
                return false; // �ϳ��� ó�� (�켱���� ����)
            }
        }
        // ��ġ ����
        return false;
    }
}
