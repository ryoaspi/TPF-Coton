using UnityEngine;

namespace Player.Runtime
{
    public class MagnetismeCoton : MonoBehaviour
    {
        private void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionDistance,_layerCoton);
            
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.activeInHierarchy)
                {
                    Transform cotonTransform = collider.transform;
                    
                    //déplacement vers le joueur
                    cotonTransform.position = Vector3.MoveTowards(cotonTransform.position, transform.position, _attractionSpeed * Time.deltaTime);
                    
                }
            }
        }

        [SerializeField] private LayerMask _layerCoton;
        [SerializeField] private float _detectionDistance = 5f;
        [SerializeField] private float _attractionSpeed = 5f;
    }
}
