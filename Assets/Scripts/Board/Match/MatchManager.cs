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
            new LineMatchRule(5, 7),  // 5 매치
            new TShapeMatchRule(6),   // T 매치
            new LShapeMatchRule(5),   // L 매치
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

    public List<(List<Block>, MatchType)> ExtractAllMatches(List<Block> group)
    {
        // 매치 리스트를 담을 results 리스트
        List<(List<Block>, MatchType)> results = new();

        // 사용된 블럭의 좌표값들 
        HashSet<Vector2Int> used = new();

        foreach (IMatchRule rule in matchRules.OrderByDescending(r => r.priority))
        {
            // 그룹안의 모든 매치가 이루어질 때 까지 수행
            while (true)
            {
                // 이미 사용된 블록 좌표라면 제외 
                List<Block> filtered = group.Where(b => !used.Contains(b.BoardPos)).ToList();
                
                // 사용되지 않은 블럭들의 수가 최소(3) 미만이라면 break
                if (filtered.Count < 3)
                {
                    break;
                }

                List<Block> match = rule.ExtractMatchBlocks(filtered);

                if (match != null)
                {
                    results.Add((match, rule.matchType));

                    // 매치된 블럭들의 좌표 저장
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
