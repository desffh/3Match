using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SpecialBlockType
{
    Normal,
    RowBomb, // �� ��ġ
    ColBomb, // �� ��ġ

}

// summary
// Ư�� �� Ÿ��
// summary

[Serializable]
public class BlockTypeData
{
    public SpecialBlockType specialType;
    public Sprite Sprite;
    
    // ���� �ɷ�
}

// ��� Ÿ�� �����ʹ� ���߿� ��ųʸ��� �����صΰ� Get(type ���Ÿ��)�� ���� ���� ������ ����