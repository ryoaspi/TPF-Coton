using System;
using UnityEngine;
using Random = System.Random;

namespace Environment.Runtime
{
    public class DistanceBasedTessellation : MonoBehaviour
    {
        [Header("Tessellatioin Settings")] 
        [Tooltip("Distance maximale pour la transition de tessellation.")]
        public float m_maxDistance = 30f;
        
        [Tooltip("Facteur de tessellation quand la caméra est proche.")]
        public float m_maxTessellation = 6;
        
        [Tooltip("Facteur de tessellation quand la caméra est loin.")]
        public float m_minTessellation = 1f;

        private Renderer _rend;
        private MaterialPropertyBlock _propBlock;
        private Transform _cameraTransform;
        
        // Optionnel : étaler les updates sur plusieurs frames pour résuire le coût CPU
        private int _frameOffset;

        private void Awake()
        {
            _rend =  GetComponent<Renderer>();
            _propBlock = new MaterialPropertyBlock();
            
            // Utilise la caméra principale comme référence
            _cameraTransform =  Camera.main.transform;
            
            //Génère un offset de frame aléatoire pour étaler les updates
            _frameOffset = (int)(transform.position.sqrMagnitude % 5); //stable random.
        }

        private void Update()
        {
            // Optimisation : ne met à jour que 1 fois toutes les 5 frames.
            if (Time.frameCount % 5 != _frameOffset) return;
            
            if (_cameraTransform is null || _rend is null) return;
            
            float distance = Vector3.Distance(transform.position, _cameraTransform.position);
            float t = Mathf.Clamp01(distance / m_maxDistance);
            float tessFactor = Mathf.Lerp(m_maxTessellation, m_minTessellation, t);
            
            _rend.SetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_TessellationFactor", tessFactor);
            _rend.SetPropertyBlock(_propBlock);
        }
    }
}
