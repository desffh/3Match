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

    // 생성자
    public MatchManager(MatchMerger matchMerger)
    {
        matchRules = new List<IMatchRule>
        {
            // 우선순위 순 (7 6 5 4 3)
            new TShapeMatchRule(7),   // T 매치
            new LShapeMatchRule(6),   // L 매치
            new LineMatchRule(5, 5),  // 5 매치
            new LineMatchRule(4, 4),  // 4 매치
            new LineMatchRule(3, 3),  // 3 매치
        };

        this.matchMerger = matchMerger;
    }

    /// <summary>
    /// 우선순위 순으로 IMatchRule을 하나씩 호출
    /// 
    /// isMatch == true : 매치가 되었다면
    /// 매치된 블럭 리스트를 가져온 후 병합 호출
    /// </summary>

    public bool ProcessMatch(List<Block> group)
    {
        // 우선순위를 내림차순으로 정렬 (7 6 5 4 3)
        foreach (IMatchRule rule in matchRules.OrderByDescending(r => r.priority))
        {
            // 매치된 블럭 반환 -> 매치된 게 없으면 matched == null
            List <Block> matched = rule.ExtractMatchBlocks(group);

            if (matched != null)
            {
                // 병합할 블럭 리스트를 인자로 넘김
                return matchMerger.Merge(matched, rule.matchType);
            } 
        }
        // 매치 없음 -> 블럭이 3개면서 L자 모양
        return false;
    }
}
