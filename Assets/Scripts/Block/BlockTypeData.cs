using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SpecialBlockType
{
    Normal,
    RowBomb, // 행 매치
    ColBomb, // 열 매치

}

// summary
// 특수 블럭 타입
// summary

[Serializable]
public class BlockTypeData
{
    public SpecialBlockType specialType;
    public Sprite Sprite;
    
    // 고유 능력
}

// 블록 타입 데이터는 나중에 딕셔너리로 저장해두고 Get(type 블록타입)을 통해 꺼내 쓰도록 수정