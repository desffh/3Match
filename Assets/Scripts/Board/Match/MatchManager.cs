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
            new TShapeMatchRule(7),   // T ��ġ
            new LShapeMatchRule(6),   // L ��ġ
            new LineMatchRule(5, 5),  // 5 ��ġ
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

    public bool ProcessMatch(List<Block> group)
    {
        // �켱������ ������������ ���� (7 6 5 4 3)
        foreach (IMatchRule rule in matchRules.OrderByDescending(r => r.priority))
        {
            // ��ġ�� �� ��ȯ -> ��ġ�� �� ������ matched == null
            List <Block> matched = rule.ExtractMatchBlocks(group);

            if (matched != null)
            {
                // ������ �� ����Ʈ�� ���ڷ� �ѱ�
                return matchMerger.Merge(matched, rule.matchType);
            } 
        }
        // ��ġ ���� -> ���� 3���鼭 L�� ���
        return false;
    }
}
