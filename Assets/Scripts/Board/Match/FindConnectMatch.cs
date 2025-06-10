using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BFS(�ʺ� �켱 Ž��)�� ����� ������ ���� ��� �׷� ��ȯ
/// ( == ����� ���� ���)
/// </summary>

public class FindConnectMatch
{
    private Board board;

    // ������ ����
    public FindConnectMatch(Board board)
    {
        this.board = board;
    }

    /// <summary>
    /// BFS�� ����� �� Ȯ�� (�ݺ��� �ȿ��� ���� - ��� �� ��ȸ(�ߺ��� �ǳʶٱ�))
    /// 
    /// 
    /// </summary>

    public List<Block> FindConnectedMatch(Block[,] blocks, int startX, int startY, bool[,] visited)
    {
        int targetNum = blocks[startY, startX].Num;                 // ����� ���� 
        List <Block> matchList = new List<Block>();                    // ������ ���� ����� ���� List
        Queue <Vector2Int> queue = new Queue <Vector2Int>();          // ��ȸ �� ���� ���� Queue
        queue.Enqueue(new Vector2Int(startX, startY));              // Queue�� ���ڷ� ���� ��ǥ�� �߰�
        visited[startY, startX] = true;                             // ���ڷ� ���� ��ǥ�� �湮üũ

        // [���� ��ǥ �迭 (x , y)]
        // Vector2Int.up    : (0, 1)
        // Vector2Int.down  : (0,-1)
        // Vector2Int.left  : (-1,0)
        // Vector2Int.right : (1, 0)
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (queue.Count > 0)
        {
            // ť���� ���� ��ǥ���� ���� ����Ʈ�� �߰�
            Vector2Int current = queue.Dequeue();
            Block currentBlock = blocks[current.y, current.x];
            matchList.Add(currentBlock);

            foreach (Vector2Int dir in directions)
            {
                // �� ���� ���⿡ ���� ���ο� ��ǥ
                int newX = current.x + dir.x;
                int newY = current.y + dir.y;

                // �׸��� ���� �� & �湮���� ���� ��ǥ���
                if (IsInGrids(newX, newY) && !visited[newY, newX])
                {
                    // ������ ���ο� ���� ������ ���ڶ�� ť�� �߰� & �湮 üũ 
                    Block newBlock = blocks[newY, newX];

                    if (newBlock != null && newBlock.Num == targetNum)
                    {
                        queue.Enqueue(new Vector2Int(newX, newY));
                        visited[newY, newX] = true;
                    }
                }
            }
        }
        return matchList;
    }

    // �׸��� ���� �� ���� Ȯ��
    private bool IsInGrids(int x, int y)
    {
        return x >= 0 && x < board.Col && y >= 0 && y < board.Row;
    }
}
