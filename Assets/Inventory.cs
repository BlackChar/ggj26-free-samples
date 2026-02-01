using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Mop,
    Shades,
    Hat,
    Perfume
}

[System.Serializable]
public struct ItemSlot
{
    public ItemType item;
    public bool IsEmpty => item == ItemType.None;
}

public class Inventory : MonoBehaviour
{
    [Tooltip("Fixed inventory slots")]
    public int capacity = 6;

    [SerializeField]
    private ItemType[] slots;

    [Tooltip("Fixed item list of obtainable items (can be set in inspector)")]
    public ItemType[] obtainableItems = new ItemType[] { ItemType.Mop, ItemType.Shades, ItemType.Hat, ItemType.Perfume};

    private HashSet<ItemType> obtainableSet;

    private void Awake()
    {
        if (slots == null || slots.Length != capacity)
            slots = new ItemType[capacity];

        // Empty slot init
        for (int i = 0; i < slots.Length; i++)
            slots[i] = ItemType.None;

        obtainableSet = new HashSet<ItemType>(obtainableItems);
    }

    
    public bool IsObtainable(ItemType item) =>
        item != ItemType.None && obtainableSet.Contains(item);

    // Non-stackable: every slot can have 1 piece max
    public bool TryAdd(ItemType item)
    {
        if (!IsObtainable(item)) return false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == ItemType.None)
            {
                slots[i] = item;
                return true;
            }
        }

        // no free slot
        return false;
    }

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= slots.Length) return false;
        if (slots[index] == ItemType.None) return false;

        slots[index] = ItemType.None;
        return true;
    }

    public ItemType GetSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return ItemType.None;
        return slots[index];
    }

    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i] = ItemType.None;
    }

    // how many of a specific item are in the inventory
    public int CountItem(ItemType item)
    {
        int count = 0;
        foreach (var s in slots)
            if (s == item) count++;
        return count;
    }
}