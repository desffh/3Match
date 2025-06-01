using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] int num;

    public TileType tileType { get; private set; }

    [SerializeField] Image tileImage;


    // Ÿ�� ������ ���� -> �� ���� ���� �� ȣ��
    public void Init(TileData tiledata)
    {
        this.num = tiledata.Num;

        this.tileType = tiledata.TileType;

        this.tileImage.sprite = tiledata.Sprite;
    }

}
