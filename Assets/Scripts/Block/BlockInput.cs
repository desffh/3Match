using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� Ŭ�� �Է¹ޱ�
/// </summary>

public class BlockInput : MonoBehaviour
{
    public static event Action<Block, Block> OnSwapRequest;

    [SerializeField] private Block firstBlock;
    [SerializeField] private Block secondBlock;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Input.mousePosition;
            firstBlock = GetBlockUnderTouch(touchPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchPos = Input.mousePosition;
            secondBlock = GetBlockUnderTouch(touchPos);
            
            if (firstBlock != null && secondBlock != null && firstBlock != secondBlock)
            {
                // �� ��� ���̰� ������ ��쿡�� ���� ��û
                if (IsAdjacent(firstBlock.BoardPos, secondBlock.BoardPos))
                {
                    // ��ϵ� �̺�Ʈ ����
                    OnSwapRequest?.Invoke(firstBlock, secondBlock);
                }
            }

            firstBlock = null;
            secondBlock = null;
        }
    }
    private Block GetBlockUnderTouch(Vector2 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);

        return hit.collider?.GetComponent<Block>();
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

}
