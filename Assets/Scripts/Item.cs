#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System;
using UnityEngine;

public enum ItemType
{
    Ressource1
}

[Serializable]
public struct Item
{
    [SerializeField]
    private ItemType _type;

    [SerializeField]
    private int _quantity;

    public ItemType Type => _type;

    public int Quantity => _quantity;

    public Item(ItemType type, int quantity) : this()
    {
        _type = type;
        _quantity = quantity;
    }

    public Item Pick(ref int quantity)
    {
        if (_quantity < quantity)
            quantity = _quantity;
        return new Item(_type, _quantity - quantity);
    }

    public Item Drop(int quantity)
    {
        return new Item(_type, _quantity + quantity);
    }
}
