using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// T자 모양 매치 판별 및 병합 대상 추출
/// </summary>
/// 
public class TShapeMatchRule : IMatchRule
{      
    private const int matchLength = 5;              // 블럭 매치 개수
    public MatchType matchType => MatchType.TShape; // 매치 타입 (블럭에 저장될 특수 타입)
    
    private int Priority;                           // 우선 순위
    public int priority => Priority;

    public TShapeMatchRule(int priority)
    {
        Priority = priority;
    }

    /// <summary>
    /// T자 매치된 블럭 리스트를 반환
    /// </summary>
    /// <param name="group">BFS로 찾아낸 인접한 숫자 리스트 </param>
    /// <returns>T자 매치된 게 있다면 매치 블럭 리스트를 반환</returns>

    public List <Block> ExtractMatchBlocks(List <Block> group)
    {
        if (group.Count < matchLength)
        {
            return null;
        }

        // 블럭들의 좌표값만 저장
        HashSet <Vector2Int> positions = group.Select(b => b.BoardPos).ToHashSet();
        
        // 좌표 결과로 블럭을 가져올 때 사용
        Dictionary<Vector2Int, Block> posToBlock = group.ToDictionary(b => b.BoardPos);

        // T매치된 리스트 반환
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
                // 중심을 기준으로 T자 좌표 생성
                Vector2Int line1 = center + line[0];
                Vector2Int line2 = center + line[1];
                Vector2Int arm1 = center + arms[0];
                Vector2Int arm2 = center + arms[1];

                // 모든 방향에 블럭이 있는지 확인 (생성한 좌표들에 블럭들이 있는지)
                if (positions.Contains(line1) && positions.Contains(line2) &&
                    positions.Contains(arm1) && positions.Contains(arm2))
                {
                    // 중심 포함, 연결된 블럭 리스트에 저장 
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

