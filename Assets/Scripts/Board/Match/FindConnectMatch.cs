using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BFS(�ʺ� �켱 Ž��)�� ����� ������ ���� ��� �׷� ��ȯ
/// ( == ����� ���� ���)
/// </summary>

public class FindConnectMatch
{
    private Board _board;

    // ������ ����
    public FindConnectMatch(Board board)
    {
        _board = board;
    }

    // BFS�� ����� �� Ȯ��
    public List<Block> FindConnectedMatch(Block[,] blocks, int startX, int startY, bool[,] visited)
    {
        int targetNum = blocks[startY, startX].Num;
        List<Block> matched = new List<Block>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startY, startX] = true;

        // ������ �����¿� ���� ť�� �߰�
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            Block currentBlock = blocks[current.y, current.x];
            matched.Add(currentBlock);

            foreach (Vector2Int dir in directions)
            {
                int newX = current.x + dir.x;
                int newY = current.y + dir.y;

                // �׸��� ���� �� & �湮���� ����
                if (IsInBounds(newX, newY) && !visited[newY, newX])
                {
                    Block neighbor = blocks[newY, newX];
                    if (neighbor != null && neighbor.Num == targetNum)
                    {
                        // ������ ��� ť�� �߰� & �湮 üũ 
                        queue.Enqueue(new Vector2Int(newX, newY));
                        visited[newY, newX] = true;
                    }
                }
            }
        }
        return matched;
    }

    // �׸��� ���� �� ���� Ȯ��
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < _board.Col && y >= 0 && y < _board.Row;
    }
}
