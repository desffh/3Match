using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 어떤 룰이 적용되는 지 판단 (IMatchRule)
/// </summary>

public class MatchManager
{
    private List<IMatchRule> matchRules;

    private MatchMerger matchMerger;

    public MatchManager(MatchMerger matchMerger)
    {
        matchRules = new List<IMatchRule>
        {
            // 우선순위 순
            new TShapeMatchRule(),   // T 매치
            new LShapeMatchRule(),   // L 매치
            new LineMatchRule(5, 5), // 5 매치
            new LineMatchRule(4, 4), // 4 매치
            new LineMatchRule(3, 3), // 3 매치
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
                    // 병합할 블럭 리스트를 인자로 넘김
                    return matchMerger.Merge(matched, rule.matchType);
                }
                return false; // 하나만 처리 (우선순위 적용)
            }
        }
        // 매치 없음
        return false;
    }
}
