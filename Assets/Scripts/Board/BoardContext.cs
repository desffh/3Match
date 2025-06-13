using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보드와 관련된 매개변수로 넘길 때 사용 (공유 데이터)
///
/// BlockSpawner.cs
/// BlocksBelow.cs
/// </summary>

public class BoardContext
{
    public Block[,] Blocks;                            // 블럭들이 할당된 2차원 배열 
    public Func<int, int, Vector3> GetWorldPosition;   // 좌표값에 따른 위치값 함수

    // 스테이지에 따라 할당되는 행열
    public int row;
    public int col;
}
