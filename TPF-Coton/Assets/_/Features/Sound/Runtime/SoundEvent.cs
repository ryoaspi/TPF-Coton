using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEvent : MonoBehaviour
    {
        [System.Serializable]
        public class NamedSound
        {
            public string m_name;
            public List<AudioClip> m_audioClips=new List<AudioClip>();
            [Range(0f,1f)]
            public float m_volume = 1f;
        }

        [Header("Liste des sons")] 
        public List<NamedSound> m_sounds;

        private Dictionary<string, NamedSound> _soundDict;
        private AudioSource _audioSource;
        private Action _onCompleteCallback;
        private int _lastClipIndex = -1;

        void Awake()
        {
            _audioSource =  GetComponent<AudioSource>();
            _soundDict = new Dictionary<string, NamedSound>();

            foreach (var sound in m_sounds)
            {
                if (!_soundDict.ContainsKey(sound.m_name))
                    _soundDict.Add(sound.m_name, sound);
                else 
                    Debug.LogWarning($"le son '{sound.m_name}' est dupliqué sur {gameObject.name}");
            }
        }

        public void PlaySoundEvent(string soundName)
        {
            if (_soundDict.TryGetValue(soundName, out var sound))
            {
                if (sound.m_audioClips.Count == 0)
                {
                    Debug.LogWarning($"Aucun clip défini pour '{soundName}' sur {gameObject.name}");
                    return;
                }
                
                // Choisir un clip aléatoire
                var randomIndex = Random.Range(0, sound.m_audioClips.Count);
                var selectedClip = sound.m_audioClips[randomIndex];
                
                _audioSource.PlayOneShot(selectedClip,sound.m_volume);
                
            }

            else
            {
                Debug.LogWarning($"Son '{soundName}' non trouvé sur  {gameObject.name}");
            }
        }
        
        public void PlaySoundEventScript(string soundName, Action onComplete = null)
        {
            if (_soundDict.TryGetValue(soundName, out var sound))
            {
                if (sound.m_audioClips.Count == 0)
                {
                    Debug.LogWarning($"Aucun clip défini pour '{soundName}' sur {gameObject.name}");
                    return;
                }
                
                // Choisir un clip aléatoire
                var randomIndex = GetNonRepeatingRandomIndex(sound.m_audioClips.Count);
                var selectedClip = sound.m_audioClips[randomIndex];
                
                _audioSource.PlayOneShot(selectedClip,sound.m_volume);

                if (onComplete != null)
                {
                    _onCompleteCallback = onComplete;
                    Invoke(nameof(InvokeOnComplete),selectedClip.length);
                }
            }

            else
            {
                Debug.LogWarning($"Son '{soundName}' non trouvé sur  {gameObject.name}");
            }
        }

        private void InvokeOnComplete()
        {
            _onCompleteCallback?.Invoke();
            CancelInvoke(nameof(InvokeOnComplete));
            _onCompleteCallback = null;
        }

        private int GetNonRepeatingRandomIndex(int count)
        {
            if (count <= 1) return 0;

            int newIndex;
            do
            {
                newIndex = Random.Range(0, count);
            } while  (newIndex == _lastClipIndex);
            
            _lastClipIndex = newIndex;
            return newIndex;
        }
    }
}
