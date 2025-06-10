using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BFS(너비 우선 탐색)을 사용한 인접한 숫자 블록 그룹 반환
/// ( == 연결된 숫자 덩어리)
/// </summary>

public class FindConnectMatch
{
    private Board board;

    // 생성자 주입
    public FindConnectMatch(Board board)
    {
        this.board = board;
    }

    /// <summary>
    /// BFS를 사용한 블럭 확인 (반복문 안에서 실행 - 모든 블럭 순회(중복은 건너뛰기))
    /// 
    /// 
    /// </summary>

    public List<Block> FindConnectedMatch(Block[,] blocks, int startX, int startY, bool[,] visited)
    {
        int targetNum = blocks[startY, startX].Num;                 // 블록의 숫자 
        List <Block> matchList = new List<Block>();                    // 인접한 숫자 블록을 담을 List
        Queue <Vector2Int> queue = new Queue <Vector2Int>();          // 순회 할 블럭을 담을 Queue
        queue.Enqueue(new Vector2Int(startX, startY));              // Queue에 인자로 들어온 좌표값 추가
        visited[startY, startX] = true;                             // 인자로 들어온 좌표값 방문체크

        // [방향 좌표 배열 (x , y)]
        // Vector2Int.up    : (0, 1)
        // Vector2Int.down  : (0,-1)
        // Vector2Int.left  : (-1,0)
        // Vector2Int.right : (1, 0)
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (queue.Count > 0)
        {
            // 큐에서 꺼낸 좌표값의 블럭을 리스트에 추가
            Vector2Int current = queue.Dequeue();
            Block currentBlock = blocks[current.y, current.x];
            matchList.Add(currentBlock);

            foreach (Vector2Int dir in directions)
            {
                // 블럭 주위 방향에 따른 새로운 좌표
                int newX = current.x + dir.x;
                int newY = current.y + dir.y;

                // 그리드 범위 내 & 방문되지 않은 좌표라면
                if (IsInGrids(newX, newY) && !visited[newY, newX])
                {
                    // 인접한 새로운 블럭이 동일한 숫자라면 큐에 추가 & 방문 체크 
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

    // 그리드 범위 내 인지 확인
    private bool IsInGrids(int x, int y)
    {
        return x >= 0 && x < board.Col && y >= 0 && y < board.Row;
    }
}
