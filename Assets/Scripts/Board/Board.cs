using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���带 �����ϴ� �� & �� 
public class Board : MonoBehaviour
{
    int row; // ��
    int col; // ��

    // 2���� �迭 ����
    private Cell[,] cells;
    private Block[,] tiles;

    public int Row => row;
    public int Col => col;

    public Cell[,] Cells => cells;
    public Block[,] Tiles => tiles;


    // 2���� �迭 ũ�� ���� [row, col] 
    public void Init(int nRow, int nCol)
    {
        row = nRow;
        col = nCol;

        cells = new Cell[row, col];
        tiles = new Block[row, col];
    }
}
