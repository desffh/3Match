using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보드를 구성하는 셀 & 블럭 
public class Board : MonoBehaviour
{
    int row; // 행
    int col; // 열

    // 2차원 배열 생성
    private Cell[,] cells;
    private Block[,] tiles;

    public int Row => row;
    public int Col => col;

    public Cell[,] Cells => cells;
    public Block[,] Tiles => tiles;


    // 2차원 배열 크기 설정 [row, col] 
    public void Init(int nRow, int nCol)
    {
        row = nRow;
        col = nCol;

        cells = new Cell[row, col];
        tiles = new Block[row, col];
    }
}
