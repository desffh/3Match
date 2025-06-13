using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ���õ� �Ű������� �ѱ� �� ��� (���� ������)
///
/// BlockSpawner.cs
/// BlocksBelow.cs
/// </summary>

public class BoardContext
{
    public Block[,] Blocks;                            // ������ �Ҵ�� 2���� �迭 
    public Func<int, int, Vector3> GetWorldPosition;   // ��ǥ���� ���� ��ġ�� �Լ�

    // ���������� ���� �Ҵ�Ǵ� �࿭
    public int row;
    public int col;
}
