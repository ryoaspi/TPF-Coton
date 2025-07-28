using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Runtime
{
    [Serializable]
    public class Inventory 
    {
        public List<ItemData> m_items = new();
        public Dictionary<EquipSlot, ItemData> m_equippedItems = new();

        public void AddItem(ItemData item)
        {
            var existing = m_items.Find(x => x.m_ID == item.m_ID);
            if (existing != null) existing.m_quantity += item.m_quantity;
            
            else m_items.Add(item);
        }

        public void EquipItem(ItemData item, EquipSlot slot)
        {
            m_equippedItems[slot] = item;
        }

        public void UnequipItem(EquipSlot slot)
        {
            m_equippedItems.Remove(slot);
        }

        public enum EquipSlot
        {
            Head,
            Body,
            Weapon,
            Legs,
            Shield
        }
    }
}
