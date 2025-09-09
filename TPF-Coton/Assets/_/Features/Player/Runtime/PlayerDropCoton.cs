using System;
using Object.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Runtime
{
    public class PlayerDropCoton : MonoBehaviour
    {
        #region Publics

        public event Action<int> OnCotonLost;

        #endregion
        
        #region Utils
        
        public void DropCotonDamage(int damageToApply)
        {
            OnCotonLost?.Invoke(damageToApply);
            
            
            for (int i = 0; i < damageToApply; i++)
            {
                GameObject newCoton = Instantiate(_coton, transform.position, Quaternion.identity);

                Vector2 offset = Random.insideUnitCircle;
                offset.y = Mathf.Abs(offset.y);
                
                Vector3 startPos = transform.position;
                Vector3 randomOffset = new Vector3(offset.x, 0, offset.y) * _distance;
                Vector3 targetPos = startPos + randomOffset;

                // 🔹 Vérifie avec un raycast si un mur bloque la trajectoire
                if (Physics.Raycast(startPos, randomOffset.normalized, out RaycastHit hit, _distance))
                {
                    // Ajuste la target juste avant le mur
                    targetPos = hit.point - randomOffset.normalized * 0.3f;
                }
                
                var cotonComp = newCoton.GetComponent<Coton>();
                var lerpComp = newCoton.GetComponent<ParabolLerp>();
                
                // Désactive la physique pendant le lerp
                cotonComp.SetPhysicsActive(false);
                
                // Lance le lerp
                lerpComp.Lerp(startPos, targetPos, _arcHeight, _arcDuration);
                
                // Quand le Lerp est terminé, réactive la physique pour la chute naturelle
                lerpComp.OnLerpComplete += () => cotonComp.SetPhysicsActive(true);
            }
        }
        
        
        #endregion
        
        #region Private And Protected
        
        [SerializeField] private GameObject _coton;
        [SerializeField] private float _distance = 1.5f;
        [SerializeField] private float _arcHeight = 2f;
        [SerializeField] private float _arcDuration = 1f;
        
        #endregion
    }
}