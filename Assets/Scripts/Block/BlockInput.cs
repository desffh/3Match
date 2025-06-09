using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 블록 클릭 입력받기
/// </summary>

public class BlockInput : MonoBehaviour
{
    public static event Action<Block, Block> OnSwapRequest;

    [SerializeField] private Block firstBlock;
    [SerializeField] private Block secondBlock;

    [SerializeField] private Board board;

    private void Awake()
    {
        board = GetComponentInParent<Board>();
    }


    void Update()
    {
        // 첫번째로 누른 블럭
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Input.mousePosition;
            firstBlock = GetBlockUnderTouch(touchPos);
        }

        // 두번째로 누른 블럭
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchPos = Input.mousePosition;
            secondBlock = GetBlockUnderTouch(touchPos);
            
            if (firstBlock != null && secondBlock != null && firstBlock != secondBlock)
            {
                // 두 블록 사이가 인접한 경우에 스왑 요청
                if (IsAdjacent(firstBlock.BoardPos, secondBlock.BoardPos))
                {
                    // 등록된 스왑 이벤트 실행
                    OnSwapRequest?.Invoke(firstBlock, secondBlock);
                }
            }

            firstBlock = null;
            secondBlock = null;
        }
    }

    /// <summary>
    /// 누른 블럭의 컴포넌트 반환 
    /// </summary>

    private Block GetBlockUnderTouch(Vector2 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);

        return hit.collider?.GetComponent<Block>();
    }


    /// <summary>
    /// 선택된 두 블럭이 인접한 지 확인
    /// </summary>
    
    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x); // 두 x값의 차이를 절대값으로 반환 
        int dy = Mathf.Abs(a.y - b.y); // 두 y값의 차이를 절대값으로 반환

        // 차이가 (0, 1) (1, 0) 이면 true 반환
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

}
