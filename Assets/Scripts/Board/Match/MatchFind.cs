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

    /// <summary>
    /// 첫 시작 시 블럭 생성 & 매치 확인
    /// </summary>
    /// <returns></returns>

    private IEnumerator SpawnAndMatchLoop()
    {
        bool matched;

        do
        {
            yield return board.StartCoroutine(board.FillUntilStable());
            yield return new WaitForSeconds(0.2f);

            matched = CheckAllMatches();
            yield return new WaitForSeconds(0.3f);
        } 
        while (matched);
    }


    /// <summary>
    /// 스왑 후 매치 확인 & 블럭 생성
    /// </summary>
    
    private IEnumerator MatchLoop()
    {
        bool matched;

        do
        {
            matched = CheckAllMatches();
            yield return new WaitForSeconds(0.3f);

            yield return board.StartCoroutine(board.FillUntilStable());
            yield return new WaitForSeconds(0.2f);

        } while (matched);

        if(!matched)
        {
            // 매치되는 게 없다면 되돌리기 애니메이션 호출 후 종료
            yield break;
        }
    }

    // 매치 이벤트
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

                // 매치 갯수가 3개 이상이라면 
                if (group.Count >= 3)
                {
                    // T : T자 병합 실행
                    //일반: 일반 병합 실행
                    if (matchManager.ProcessMatch(group))
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
