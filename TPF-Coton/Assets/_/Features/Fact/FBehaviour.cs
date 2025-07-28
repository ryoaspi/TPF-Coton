using UnityEngine;

namespace TheFundation.Runtime
{
    public class FBehaviour : MonoBehaviour
    {

        public static FactDictionary FactPersistence
        {
            get => GameManager.m_gameFacts;
        }

        // FACTS
        protected bool HasFact<T>(string key, out T value)
        {
            return GameManager.m_gameFacts.FactExist(key, out value);
        }

        protected T GetFact<T>(string key)
        {
            return GameManager.m_gameFacts.GetFact<T>(key);
        }

        protected void SetFact<T>(string key, T value,
            FactDictionary.FactPersistence persistence = FactDictionary.FactPersistence.Normal)
        {
            if (GameManager.m_gameFacts == null)
            {
                Debug.Log("Game facts not initialized");
                return;
            }
            
            GameManager.m_gameFacts.SetFact(key, value, persistence);
        }

        protected void RemoveFact(string key)
        {
            GameManager.m_gameFacts.RemoveFact(key);
        }

        protected bool TryGetFact<T>(string key, out T value)
        {
            return GameManager.m_gameFacts.TryGetFact(key, out value);
        }
        
        
        // SAVE SYSTEM
        protected void SaveToSlot(int slot)
        {
            GameManager.SaveGameToSlot(slot);
        }

        protected void LoadFromSlot(int slot)
        {
            GameManager.LoadGameFromSlot(slot);
        }

        protected void DeleteSlot(int slot)
        {
            GameManager.DeleteSaveSlot(slot);
        }

        protected bool SlotExist(int slot)
        {
            return GameManager.HasSaveInSlot(slot);
        }
        
        // Localization
        protected void SetLanguage(string language)
        {
            LocalizationManager.m_Instance.LoadLanguage(language);
        }

        protected string GetCurrentLanguage()
        {
            return LocalizationManager.m_Instance.CurrentLanguage;
        }

        protected string GetLocalizedText(string key)
        {
            return LocalizationManager.m_Instance.GetText(key);
        }
    }
}
