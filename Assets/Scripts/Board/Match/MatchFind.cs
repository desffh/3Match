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
        Board.OnmatchFind += CheckAllMatches;
    }

    private void OnDisable()
    {
        Board.OnmatchFind -= CheckAllMatches;
    }

    public void CheckAllMatches()
    {
        Block[,] blocks = board.Blocks;
        int row = board.Row;
        int col = board.Col;
        bool[,] visited = new bool[row, col];

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                // 이미 방문했다면 건너뛰기 
                if (blocks[y, x] == null || visited[y, x])
                    continue;

                // 인접한 모든 매치 찾기 (동일한 숫자끼리의 덩어리)
                List<Block> group = findConnectMatch.FindConnectedMatch(blocks, x, y, visited);

                if(group.Count <= 1)
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
                    matchManager.ProcessMatch(validGroup);
                }
            }
        }
    }
}
