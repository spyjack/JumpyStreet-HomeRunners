using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RowRarity
{
    [Header("Lane Generation Rarities")]
    [SerializeField]
    public float grassRarity;
    [SerializeField]
    public float streetRarity;
    [SerializeField]
    public float roadRarity;
    [SerializeField]
    public float riverRarity;
    [SerializeField]
    public float railroadRarity;
    [SerializeField]
    public float forestRarity;

    public RowType GetRow(float _rarity)
    {
        if(_rarity <= forestRarity)
        {
            return RowType.forest;
        }else if (_rarity > forestRarity && _rarity <= streetRarity)
        {
            return RowType.street;
        }else if (_rarity > streetRarity && _rarity <= roadRarity)
        {
            return RowType.road;
        }
        else if (_rarity > roadRarity && _rarity <= riverRarity)
        {
            return RowType.river;
        }
        else if (_rarity > riverRarity && _rarity <= railroadRarity)
        {
            return RowType.railroad;
        }

        return RowType.grass;
    }
}

public enum RowType
{
    street,
    road,
    river,
    railroad,
    grass,
    forest
}
