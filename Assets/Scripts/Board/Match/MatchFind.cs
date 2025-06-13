using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;

/// summary
/// Board 이벤트 구독자 -> 매치 실행
/// </summary>

public class MatchFind : MonoBehaviour
{
    private Board board;

    // 동일한 숫자 덩어리 반환
    private FindConnectMatch findConnectMatch;

    // 매치 판단
    private MatchManager matchManager;
    // 병합 실행
    private MatchMerger matchMerger;


    private void Awake()
    {
        board = GetComponent<Board>();

        // 객체 생성 & 생성자 주입
        findConnectMatch = new FindConnectMatch(board);
        matchMerger = new MatchMerger(board);
        matchManager = new MatchManager(matchMerger);

    }

    private void OnEnable()
    {
        Board.OnmatchFind += HandleOnMatchFind;
    }

    private void OnDisable()
    {
        Board.OnmatchFind -= HandleOnMatchFind;

    }

    private void HandleOnMatchFind()
    {
        StartCoroutine(MatchLoop());
    }


    /// <summary>
    /// 매치 확인 & 블럭 생성
    /// </summary>
    
    private IEnumerator MatchLoop()
    {
        bool matched;

        do
        {
            matched = CheckAllMatches();
            
            if(matched)
            {
                yield return new WaitForSeconds(0.2f);
            }

            yield return board.StartCoroutine(board.FillBlocks());
            yield return new WaitForSeconds(0.1f);

        } while (matched);

        if(!matched)
        {
            // 매치되는 게 없다면 되돌리기 애니메이션 호출 후 종료
            yield break;
        }
    }

    /// <summary>
    /// 매치 시작 - 1. BFS 2. IMatchRule 전략패턴 3. 병합
    /// </summary>
    
    public bool CheckAllMatches()
    {
        Block[,] blocks = board.Blocks;
        int row = board.Row;
        int col = board.Col;
        bool[,] visited = new bool[row, col];

        // 모든 블럭 순회 후 그룹을 담을 리스트
        List<List<Block>> allGroups = new();

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                if (blocks[y, x] == null || visited[y, x])
                {
                    continue;
                }

                List<Block> group = findConnectMatch.FindConnectedMatch(blocks, x, y, visited);

                // 블럭 개수가 3 이상이라면 추가
                if (group.Count >= 3)
                {
                    allGroups.Add(group);
                }
            }
        }

        // --- 모든 그룹을 대상으로 매치 판단 수행 ---

        // 매치 리스트, 타입을 담을 totalMatches 튜플 리스트
        List<(List<Block> match, MatchType type)> totalMatches = new();

        foreach (List <Block> group in allGroups)
        {
            // ExtractAllMatches는 매치 리스트들을 반환함
            List<(List<Block>, MatchType)> matches = matchManager.ExtractAllMatches(group);

            // 모든 매치 담기
            totalMatches.AddRange(matches);
        }

        // --- 모든 매치를 대상으로 병합 수행 ---

        foreach ((List<Block> matched, MatchType type) in totalMatches)
        {
            matchMerger.Merge(matched, type);
        }

        return totalMatches.Count > 0;
    }

}
