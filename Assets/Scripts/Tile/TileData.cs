using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six
}

[CreateAssetMenu(menuName = "TileData")]
public class TileData : ScriptableObject
{
    [SerializeField] private TileType tileType;
    [SerializeField] private int num;
    [SerializeField] private Sprite sprite;

    public TileType TileType => tileType;
    public int Num => num;
    public Sprite Sprite => sprite;
}
