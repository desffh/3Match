using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� Ŭ�� �Է¹ޱ�
/// </summary>

public class BlockInput : MonoBehaviour
{
    [Header("Ŭ���� �� ���")]
    [SerializeField] private Block firstBlock;
    [SerializeField] private Block secondBlock;

    private Board board;

    // Swap ȣ�� �̺�Ʈ -> Board.cs�� TrySwap�� ������
    public static event Action<Block, Block> OnSwapRequest;


    private void Awake()
    {
        board = GetComponentInParent<Board>();
    }

    void Update()
    {
        // ù��°�� ���� ��
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Input.mousePosition;
            firstBlock = GetBlockUnderTouch(touchPos);
        }

        // �ι�°�� ���� ��
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchPos = Input.mousePosition;
            secondBlock = GetBlockUnderTouch(touchPos);
            
            if (firstBlock != null && secondBlock != null && firstBlock != secondBlock)
            {
                // �� ��� ���̰� ������ ��쿡 ���� ��û
                if (IsAdjacent(firstBlock.BoardPos, secondBlock.BoardPos))
                {
                    // ��ϵ� ���� �̺�Ʈ ����
                    OnSwapRequest?.Invoke(firstBlock, secondBlock);
                }
            }

            firstBlock = null;
            secondBlock = null;
        }
    }

    /// <summary>
    /// ���� ������Ʈ ��ȯ
    /// </summary>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    private Block GetBlockUnderTouch(Vector2 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);

        return hit.collider?.GetComponent<Block>();
    }


    /// <summary>
    /// ���õ� �� ���� ������ �� Ȯ��
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    
    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x); // �� x���� ���̸� ���밪���� ��ȯ 
        int dy = Mathf.Abs(a.y - b.y); // �� y���� ���̸� ���밪���� ��ȯ

        // ���̰� (0, 1) (1, 0) �̸� true ��ȯ
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

}
