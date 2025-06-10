using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

/// <summary>
/// L자 모양 매치 판별 및 병합 대상 추출
/// </summary>
public class LShapeMatchRule : IMatchRule
{
    private const int matchLength = 5;              // 블럭 매치 개수
    public MatchType matchType => MatchType.LShape; // 매치 타입 (블럭에 저장될 특수 타입)
    
    private int Priority = 6;                       // 우선 순위
    public int priority => Priority;


    public LShapeMatchRule(int priority)
    {
        Priority = priority;
    }

    /// <summary>
    /// L자 매치된 블럭 리스트를 반환
    /// </summary>
    /// <param name="group">BFS로 찾아낸 인접한 숫자 리스트 </param>
    /// <returns>L자 매치된 게 있다면 매치 블럭 리스트를 반환</returns>
    
    public List <Block> ExtractMatchBlocks(List <Block> group)
    {
        if (group.Count < matchLength)
        {
            return null;
        }

        // 블럭들의 좌표값만 저장
        HashSet<Vector2Int> positions = group.Select(b => b.BoardPos).ToHashSet();
        
        // 좌표 결과로 블럭을 가져올 때 사용
        Dictionary<Vector2Int, Block> posToBlock = group.ToDictionary(b => b.BoardPos);

        // L매치된 리스트 반환
        if (LShapeCenterMatch(positions, posToBlock, out List <Block> result))
        {
            return result;
        }

        return null;
    }

    /// <summary>
    /// L자 중심과 주변 2방향이 연결된 블록을 찾음
    /// </summary>
    
    private bool LShapeCenterMatch(
        HashSet<Vector2Int> positions,
        Dictionary<Vector2Int, Block> posToBlock,
        out List<Block> matchedBlocks)
    {
        // 중심좌표를 기준
        foreach (Vector2Int center in positions)
        {
            List<Vector2Int> connectBlock = new();

            // 좌표에 블럭이 존재한다면 connectBlock 리스트에 좌표 추가
            if (positions.Contains(center + Vector2Int.up)) connectBlock.Add(Vector2Int.up);
            if (positions.Contains(center + Vector2Int.down)) connectBlock.Add(Vector2Int.down);
            if (positions.Contains(center + Vector2Int.left)) connectBlock.Add(Vector2Int.left);
            if (positions.Contains(center + Vector2Int.right)) connectBlock.Add(Vector2Int.right);

            // 중심과 연결된 좌표가 2개라면
            if (connectBlock.Count == 2)
            {
                // 두 방향이 수직 + 수평인지 확인
                // Any : 리스트에 조건을 만족하는 요소가 있는 지
                bool hasHorizontal = connectBlock.Any(dir => dir == Vector2Int.left || dir == Vector2Int.right);
                bool hasVertical = connectBlock.Any(dir => dir == Vector2Int.up || dir == Vector2Int.down);

                // 두 변수가 모두 만족해야 하나씩 있는 것
                if (hasHorizontal && hasVertical)
                {
                    // 중심 + 4개 블록을 담을 리스트 (총 5개인지 확인)
                    List <Block> result = new();

                    // 1. 중심 추가
                    result.Add(posToBlock[center]);

                    // 2. 연결된 블럭 4개 추가
                    foreach (Vector2Int dir in connectBlock)
                    {
                        // L자 선을 따라 2칸씩 붙어 있으므로 1칸, 2칸 이동
                        for (int i = 1; i <= 2; i++)
                        {
                            Vector2Int next = center + dir * i;
                            
                            // 좌표값(Key)로 블럭 찾기(Value)
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
