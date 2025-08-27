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
                
                Vector3 targetPos = transform.position + new Vector3(offset.x,0,offset.y) * _distance;
                
                var contonComp = newCoton.GetComponent<Coton>();
                var lerpComp = newCoton.GetComponent<ParabolLerp>();
                
                // Désactive la physique pendant le lerp
                contonComp.SetPhysicsActive(false);
                
                // Lance le lerp
                lerpComp.Lerp(transform.position, targetPos, _arcHeight, _arcDuration);;
                
                // Quand le Lerp est terminé, réactive la physique pour la chute naturel.
                lerpComp.OnLerpComplete += () => contonComp.SetPhysicsActive(true);
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
