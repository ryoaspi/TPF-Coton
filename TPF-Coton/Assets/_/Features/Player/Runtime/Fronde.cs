// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.Serialization;
//
// namespace Player.Runtime
// {
//     public class Fronde : MonoBehaviour
//     {
//         #region Public
//         [Header("Références")]
//         public Transform m_firePoint;
//         [HideInInspector] public FrondePool m_projectilePool;
//         [FormerlySerializedAs("_hpLoss")] public int m_hpLoss=1;
//         [FormerlySerializedAs("_coolDownCharge")] [HideInInspector]public bool m_coolDownCharge;
//         
//         [Header("Indicateur de visée")]
//         public GameObject m_aimVisualPrefab;
//         public float m_rayLength = 5f;
//         #endregion
//
//         #region UnityAPI
//         private void Awake()
//         {
//             _playerInput = GetComponent<PlayerInput>();
//             _playerMovement = GetComponent<PlayerMovement>();
//             _playerDamage = GetComponent<PlayerDamage>();
//             _shield = GetComponent<Shield>();
//             _playerStats = GetComponent<PlayerStats>();
//             _playerBuff= GetComponent<PlayerBuff>();
//
//             if (m_aimVisualPrefab != null)
//             {
//                 _aimVisual = Instantiate(m_aimVisualPrefab, transform);
//                 _aimVisual.SetActive(false);
//                 _line = _aimVisual.GetComponent<LineRenderer>();
//             }
//         }
//
//         private void Start()
//         {
//             m_projectilePool = FrondePool.Instance;
//             m_coolDownCharge=false;
//         }
//
//         private void Update()
//         {
//             if (m_isCharging)
//             {
//                 _chargeTimer += Time.deltaTime;
//                 UpdateAimVisual();
//             }
//
//             if (m_coolDownCharge)
//             {
//                 _timeCharge+=Time.deltaTime;
//                 if (_timeCharge >= _maxCoolDown)
//                 {
//                     m_coolDownCharge = false;
//                     _timeCharge = 0;
//                 }
//             }
//         }
//         #endregion
//
//         #region Utils
//        
//
//         private void UpdateAimVisual()
//         {
//             if (_aimVisual != null && _line != null)
//             {
//                 _aimVisual.SetActive(true);
//                 Vector3 startPos = m_firePoint.position;
//                 Vector3 endPos = m_firePoint.position + m_firePoint.forward * m_rayLength;
//
//                 _line.SetPosition(0, startPos);
//                 _line.SetPosition(1, endPos);
//             }
//         }
//
//         private void HideAimVisual()
//         {
//             if (_aimVisual != null)
//                 _aimVisual.SetActive(false);
//         }
//         #endregion
//
//         #region New input system
//         private void OnEnable()
//         {
//             var actions = _playerInput.actions;
//             actions["Shoot"].started += Shoot;
//             actions["Shoot"].canceled += Shoot;
//         }
//
//         private void OnDisable()
//         {
//             var actions = _playerInput.actions;
//             actions["Shoot"].started -= Shoot;
//             actions["Shoot"].canceled -= Shoot;
//         }
//
//         public void Shoot(InputAction.CallbackContext context)
//         {
//             if (m_coolDownCharge || _playerDamage.m_isAttacking || _shield.m_isShielding || _playerStats.m_currentHealth <=m_hpLoss)
//                 return;
//
//             if (context.started)
//             {
//                 
//                 _chargeTimer = 0f;
//                 m_isCharging = true;
//                 if (_aimVisual != null) _aimVisual.SetActive(true);
//                 _playerMovement.m_speed -= _playerMovement.m_speed;
//             }
//             else if (context.canceled && m_isCharging)
//             {
//                 _playerStats.FrondeSelfDamage(m_hpLoss);
//                 m_isCharging = false;
//                 _playerMovement.m_speed = _playerMovement.m_speedSave;
//                 HideAimVisual();
//
//                 
//                 _bullet = FrondePool.Instance.GetFromPool();
//                 if (_bullet == null) return;
//
//                 
//                 _bullet.transform.position = m_firePoint.position;
//                 _bullet.transform.rotation = m_firePoint.rotation;
//                 _bullet.transform.parent = null;
//                 
//                 
//                 
//                 // Reset cooldown
//                 m_coolDownCharge = true;
//                 _timeCharge = 0f;
//             }
//             
//             
//         }
//         #endregion
//
//         #region Private
//         private PlayerInput _playerInput;
//         private float _chargeTimer;
//         [FormerlySerializedAs("_isCharging")] public bool m_isCharging;
//         private GameObject _bullet;
//
//         [Header("Fronde Variables")]
//         [SerializeField] private float _maxCoolDown=3;
//         
//         [SerializeField] private int _speedLoss=3;
//         
//         
//         
//         private float _timeCharge;
//         
//         private GameObject _aimVisual;
//         private LineRenderer _line;
//         
//         private PlayerDamage _playerDamage;
//         private PlayerMovement _playerMovement;
//         private Shield _shield;
//         private PlayerStats _playerStats;
//         private PlayerBuff _playerBuff;
//         #endregion
//     }
// }
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player.Runtime
{
    public class Fronde : MonoBehaviour
    {
        #region Public
        [Header("Références")]
        public Transform m_firePoint;
        [HideInInspector] public FrondePool m_projectilePool;
        [FormerlySerializedAs("_hpLoss")] public int m_hpLoss = 1;
        [FormerlySerializedAs("_coolDownCharge")] [HideInInspector] public bool m_coolDownCharge;

        [Header("Indicateur de visée")]
        public GameObject m_aimVisualPrefab;
        public float m_rayLength = 5f;
        #endregion

        #region UnityAPI
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerDamage = GetComponent<PlayerDamage>();
            _shield = GetComponent<Shield>();
            _playerStats = GetComponent<PlayerStats>();
            _playerBuff = GetComponent<PlayerBuff>();

            if (m_aimVisualPrefab != null)
            {
                _aimVisual = Instantiate(m_aimVisualPrefab, transform);
                _aimVisual.SetActive(false);
                _line = _aimVisual.GetComponent<LineRenderer>();
            }
        }

        private void Start()
        {
            m_projectilePool = FrondePool.Instance;
            m_coolDownCharge = false;
        }

        private void Update()
        {
            if (m_isCharging)
            {
                _chargeTimer += Time.deltaTime;
                UpdateAimVisual();
            }

            if (m_coolDownCharge)
            {
                _timeCharge += Time.deltaTime;
                if (_timeCharge >= _maxCoolDown)
                {
                    m_coolDownCharge = false;
                    _timeCharge = 0;
                }
            }
        }
        #endregion

        #region Utils
        private void UpdateAimVisual()
        {
            if (_aimVisual != null && _line != null)
            {
                _aimVisual.SetActive(true);
                Vector3 startPos = m_firePoint.position;
                Vector3 endPos = m_firePoint.position + m_firePoint.forward * m_rayLength;

                _line.SetPosition(0, startPos);
                _line.SetPosition(1, endPos);
            }
        }

        private void HideAimVisual()
        {
            if (_aimVisual != null)
                _aimVisual.SetActive(false);
        }
        #endregion

        #region New Input System
        private void OnEnable()
        {
            if (_playerInput == null || _playerInput.actions == null) return;
            var actions = _playerInput.actions;
            if (actions["Shoot"] != null)
            {
                actions["Shoot"].started += Shoot;
                actions["Shoot"].canceled += Shoot;
            }
        }

        private void OnDisable()
        {
            if (_playerInput == null || _playerInput.actions == null) return;
            var actions = _playerInput.actions;
            if (actions["Shoot"] != null)
            {
                actions["Shoot"].started -= Shoot;
                actions["Shoot"].canceled -= Shoot;
            }
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (_playerDamage == null || _shield == null || _playerStats == null || _playerMovement == null)
                return;

            if (m_coolDownCharge || _playerDamage.m_isAttacking || _shield.m_isShielding || _playerStats.m_currentHealth <= m_hpLoss)
                return;

            if (context.started)
            {
                _chargeTimer = 0f;
                m_isCharging = true;
                if (_aimVisual != null) _aimVisual.SetActive(true);
                _playerMovement.m_speed = 0f;
            }
            else if (context.canceled && m_isCharging)
            {
                _playerStats.FrondeSelfDamage(m_hpLoss);
                m_isCharging = false;
                _playerMovement.m_speed = _playerMovement.m_speedSave;
                HideAimVisual();

                _bullet = FrondePool.Instance.GetFromPool();
                if (_bullet == null) return;

                _bullet.transform.position = m_firePoint.position;
                _bullet.transform.rotation = m_firePoint.rotation;
                _bullet.transform.parent = null;

                m_coolDownCharge = true;
                _timeCharge = 0f;
            }
        }
        #endregion

        #region Private
        private PlayerInput _playerInput;
        private float _chargeTimer;
        [FormerlySerializedAs("_isCharging")] public bool m_isCharging;
        private GameObject _bullet;

        [Header("Fronde Variables")]
        [SerializeField] private float _maxCoolDown = 3;

        private float _timeCharge;

        private GameObject _aimVisual;
        private LineRenderer _line;

        private PlayerDamage _playerDamage;
        private PlayerMovement _playerMovement;
        private Shield _shield;
        private PlayerStats _playerStats;
        private PlayerBuff _playerBuff;
        #endregion
    }
}
