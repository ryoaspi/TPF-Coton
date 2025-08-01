using System;
using System.Collections.Generic;
using PlasticGui.WorkspaceWindow.Items;
using UnityEngine;

namespace Player.Runtime
{
    [Serializable]
    public class Inventory
    {
        public int m_Maxslot = 20;
        public List<ItemData> m_items = new();

        public Inventory(int maxSlots = 20)
        {
            m_Maxslot = maxSlots;
            m_items = new List<ItemData>(new ItemData[m_Maxslot]);
        }

        public bool AddItem(ItemData item)
        {
            // stack si même item trouvé
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i] != null && m_items[i]?.m_ID == item.m_ID)
                {
                    m_items[i]!.m_quantity += item.m_quantity;
                    return true;
                }
            }
            
            //sinon, ajoute dans premier slot vide
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i] == null)
                {
                    m_items[i] = item;
                    return true;
                }
            }
            
            Debug.Log("Inventaire plein !");
            return false;
        }

        public void RemoveItem(int index)
        {
            if (index >= 0 && index < m_items.Count) m_items[index] = null;
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
