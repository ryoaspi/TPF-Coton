using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheFundation.Runtime
{
    // Stock une position Vector3 dans un fact
    [Serializable]
    public class SerialisableVector3
    {
        public float m_x, m_y, m_z;

        public SerialisableVector3(Vector3 v)
        {
            m_x = v.x;
            m_y = v.y;
            m_z = v.z;
        }
        public Vector3 ToVector3() => new Vector3(m_x,m_y,m_z);
    }

    // Représente un objet d'inventaire avec un ID et une quantité
    [Serializable]
    public class InventoryItem
    {
        public string m_id;
        public int m_quantity;
    }
    
    // Représente la progression d'une quête (étape actuelle, complétée ou non)
    [Serializable]
    public class QuestProgress
    {
        public string m_questId;
        public int m_isCompleted;
        public int m_currentStep;
    }
    
    // Permet de sérialiser un Dictionary (JsonUtility ne les supporte pas directement)
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> _keys = new ();
        [SerializeField] private List<TValue> _values = new ();
        
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
            foreach (var kv in this )
            {
                _keys.Add(kv.Key);
                _values.Add(kv.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < Math.Min(_keys.Count, _values.Count); i++)
            {
                this[_keys[i]] = _values[i];
            }
        }
    }
}
