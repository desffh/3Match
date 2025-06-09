using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// summary
/// Board �̺�Ʈ ������ -> ��ġ ����
/// </summary>

public class MatchFind : MonoBehaviour
{
    private Board board;

    // ������ ���� ��� ��ȯ
    private FindConnectMatch findConnectMatch;

    // ��ġ �Ǵ�
    private MatchManager matchManager;
    // ���� ����
    private MatchMerger matchMerger;


    private void Awake()
    {
        board = GetComponent<Board>();

        // ��ü ���� & ������ ����
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
    /// ù ���� �� �� ���� & ��ġ Ȯ��
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
    /// ���� �� ��ġ Ȯ�� & �� ����
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
            // ��ġ�Ǵ� �� ���ٸ� �ǵ����� �ִϸ��̼� ȣ�� �� ����
            yield break;
        }
    }

    // ��ġ �̺�Ʈ
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
                // �̹� �湮�ߴٸ� �ǳʶٱ� 
                if (blocks[y, x] == null || visited[y, x])
                    continue;

                // ������ ��� ��ġ ã�� (������ ���ڳ����� ���)
                List<Block> group = findConnectMatch.FindConnectedMatch(blocks, x, y, visited);

                // ��ġ ������ 3�� �̻��̶�� 
                if (group.Count >= 3)
                {
                    // T : T�� ���� ����
                    //�Ϲ�: �Ϲ� ���� ����
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
