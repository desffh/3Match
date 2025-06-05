using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BFS(너비 우선 탐색)을 사용한 인접한 숫자 블록 그룹 반환
/// ( == 연결된 숫자 덩어리)
/// </summary>

public class FindConnectMatch
{
    private Board _board;

    // 생성자 주입
    public FindConnectMatch(Board board)
    {
        _board = board;
    }

    // BFS를 사용한 블럭 확인
    public List<Block> FindConnectedMatch(Block[,] blocks, int startX, int startY, bool[,] visited)
    {
        int targetNum = blocks[startY, startX].Num;
        List<Block> matched = new List<Block>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startY, startX] = true;

        // 블럭마다 상하좌우 블럭을 큐에 추가
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

                // 그리드 범위 내 & 방문되지 않음
                if (IsInBounds(newX, newY) && !visited[newY, newX])
                {
                    Block neighbor = blocks[newY, newX];
                    if (neighbor != null && neighbor.Num == targetNum)
                    {
                        // 인접한 블록 큐에 추가 & 방문 체크 
                        queue.Enqueue(new Vector2Int(newX, newY));
                        visited[newY, newX] = true;
                    }
                }
            }
        }
        return matched;
    }

    // 그리드 범위 내 인지 확인
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < _board.Col && y >= 0 && y < _board.Row;
    }
}
