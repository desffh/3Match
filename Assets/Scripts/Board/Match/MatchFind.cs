using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;

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
    /// ��ġ Ȯ�� & �� ����
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
            // ��ġ�Ǵ� �� ���ٸ� �ǵ����� �ִϸ��̼� ȣ�� �� ����
            yield break;
        }
    }

    /// <summary>
    /// ��ġ ���� - 1. BFS 2. IMatchRule �������� 3. ����
    /// </summary>
    
    public bool CheckAllMatches()
    {
        Block[,] blocks = board.Blocks;
        int row = board.Row;
        int col = board.Col;
        bool[,] visited = new bool[row, col];

        // ��� �� ��ȸ �� �׷��� ���� ����Ʈ
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

                // �� ������ 3 �̻��̶�� �߰�
                if (group.Count >= 3)
                {
                    allGroups.Add(group);
                }
            }
        }

        // --- ��� �׷��� ������� ��ġ �Ǵ� ���� ---

        // ��ġ ����Ʈ, Ÿ���� ���� totalMatches Ʃ�� ����Ʈ
        List<(List<Block> match, MatchType type)> totalMatches = new();

        foreach (List <Block> group in allGroups)
        {
            // ExtractAllMatches�� ��ġ ����Ʈ���� ��ȯ��
            List<(List<Block>, MatchType)> matches = matchManager.ExtractAllMatches(group);

            // ��� ��ġ ���
            totalMatches.AddRange(matches);
        }

        // --- ��� ��ġ�� ������� ���� ���� ---

        foreach ((List<Block> matched, MatchType type) in totalMatches)
        {
            matchMerger.Merge(matched, type);
        }

        return totalMatches.Count > 0;
    }

}
