using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// summary
/// Board 이벤트 구독자 -> 매치 실행
/// </summary>

public class MatchFind : MonoBehaviour
{
    private Board board;

    private FindConnectMatch findConnectMatch;

    private StraightMatch straightMatch;

    private MatchManager matchManager;

    private MatchMerger matchMerger;

    private void Awake()
    {
        board = GetComponent<Board>();

        findConnectMatch = new FindConnectMatch(board);
        straightMatch = new StraightMatch(board);

        matchMerger = new MatchMerger(board);
        matchManager = new MatchManager(matchMerger);

    }

    private void OnEnable()
    {
        Board.OnmatchFind += HandleOnMatchFind;
        Board.FirstOnmatchFind += FirstHandleOnMatchFind;
    }

    private void OnDisable()
    {
        Board.OnmatchFind -= HandleOnMatchFind;
        Board.FirstOnmatchFind -= FirstHandleOnMatchFind;

    }

    private void HandleOnMatchFind()
    {
        StartCoroutine(MatchLoop());
    }
    private void FirstHandleOnMatchFind()
    {
        StartCoroutine(SpawnAndMatchLoop());
    }
    private IEnumerator SpawnAndMatchLoop()
    {
        bool matched;

        do
        {
            yield return board.StartCoroutine(board.FillUntilStable());
            yield return new WaitForSeconds(0.2f);

            matched = CheckAllMatches(); // 직접 호출 (이벤트 아님)
            yield return new WaitForSeconds(0.3f);


        } while (matched);
    }


    private IEnumerator MatchLoop()
    {
        bool matched;

        do
        {
            matched = CheckAllMatches(); // 직접 호출 (이벤트 아님)
            yield return new WaitForSeconds(0.3f);

            yield return board.StartCoroutine(board.FillUntilStable());
            yield return new WaitForSeconds(0.2f);

        } while (matched);
    }

    public bool CheckAllMatches()
    {
        Block[,] blocks = board.Blocks;
        int row = board.Row;
        int col = board.Col;
        bool[,] visited = new bool[row, col];

        bool check;

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                // 이미 방문했다면 건너뛰기 
                if (blocks[y, x] == null || visited[y, x])
                    continue;

                // 인접한 모든 매치 찾기 (동일한 숫자끼리의 덩어리)
                List<Block> group = findConnectMatch.FindConnectedMatch(blocks, x, y, visited);

                if (group.Count <= 1)
                {
                    // 되돌리기 애니메이션
                }

                // 연속된 매치 찾기
                List<Block> validGroup = straightMatch.ConnectStraightMatch(group);

                // 연속된 매치 갯수가 3개 이상이라면 
                if (validGroup.Count >= 3)
                {
                    // T : T자 병합 실행
                    //일반: 일반 병합 실행
                    if(matchManager.ProcessMatch(validGroup))
                    {
                        check = true;
                        return check;
                    }
                    
                }
            }
        }

        return false;
    }
}
