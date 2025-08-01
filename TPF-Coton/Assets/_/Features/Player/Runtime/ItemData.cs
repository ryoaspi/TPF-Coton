using System;
using UnityEngine;

namespace Player.Runtime
{
    [Serializable]
    public class ItemData
    {
        public string m_ID;
        public string m_name;
        public string m_description;
        public int m_quantity;
        public Sprite m_icon;
    }
}
