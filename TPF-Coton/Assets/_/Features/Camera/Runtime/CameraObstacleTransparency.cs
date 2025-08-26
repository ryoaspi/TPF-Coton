
using UnityEngine;
using System.Collections.Generic;

namespace Camera.Runtime
{
    /// <summary>
    /// Makes obstacles between the camera and a target player transparent.
    /// This script manages a smooth fade effect by tracking all affected
    /// obstacles in a single dictionary.
    /// It uses MaterialPropertyBlocks for performance and to avoid
    /// modifying shared materials.
    /// </summary>
    public class CameraObstacleTransparency : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The tag of the player object.")]
        [SerializeField] private string playerTag = "Player";

        [Tooltip("The layer(s) that should be treated as obstacles.")]
        [SerializeField] private LayerMask obstacleMask;

        [Tooltip("The shader property name for the color/alpha.")]
        [SerializeField] private string transparencyProperty = "_Base_Color";

        [Tooltip("The target alpha for transparent obstacles.")]
        [SerializeField] private float transparentAlpha = 0.3f;

        [Tooltip("The speed at which the fade effect occurs.")]
        [SerializeField] private float fadeSpeed = 5f;

        // The transform of the player object.
        private Transform _player;

        // A class to hold the state of an affected obstacle.
        private class ObstacleState
        {
            public Color originalColor;
            public MaterialPropertyBlock mpb;
        }

        // A single dictionary to track all obstacles that have been affected.
        // The value is an ObstacleState object.
        private readonly Dictionary<Renderer, ObstacleState> _affectedObstacles = new Dictionary<Renderer, ObstacleState>();

        /// <summary>
        /// Finds the player object on start.
        /// </summary>
        void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null)
                _player = playerObj.transform;
            else
                Debug.LogError("CameraObstacleTransparency: No player found with the tag '" + playerTag + "'");
        }

        /// <summary>
        /// Main logic to check for obstacles and manage transparency.
        /// </summary>
        void Update()
        {
            if (_player == null) return;

            Vector3 camPos = transform.position;
            Vector3 dir = _player.position - camPos;
            float dist = dir.magnitude;

            // Raycast towards the player to find all obstacles in the way.
            RaycastHit[] hits = Physics.RaycastAll(camPos, dir.normalized, dist, obstacleMask);

            // Use a hash set for efficient lookups.
            HashSet<Renderer> currentHitRenderers = new HashSet<Renderer>();
            foreach (RaycastHit hit in hits)
            {
                Renderer r = hit.collider.GetComponent<Renderer>();
                if (r != null)
                {
                    currentHitRenderers.Add(r);
                }
            }
            
            // Add new obstacles to the affected list.
            foreach (Renderer r in currentHitRenderers)
            {
                if (!_affectedObstacles.ContainsKey(r))
                {
                    // Create a new state object.
                    ObstacleState state = new ObstacleState();
                    
                    // Get the initial color from the renderer's shared material.
                    state.originalColor = r.sharedMaterial.GetColor(transparencyProperty);
                    
                    // Create a new property block for this renderer.
                    state.mpb = new MaterialPropertyBlock();
                    // Initialize the property block with the original color.
                    state.mpb.SetColor(transparencyProperty, state.originalColor);
                    
                    // Add the obstacle to the dictionary.
                    _affectedObstacles.Add(r, state);
                }
            }
            
            // -------------------------------------------------------------------------
            // Fading Logic
            // -------------------------------------------------------------------------

            // Handle fading in and out for all affected obstacles.
            var keysToProcess = new List<Renderer>(_affectedObstacles.Keys);
            foreach (var renderer in keysToProcess)
            {
                ObstacleState state = _affectedObstacles[renderer];
                
                Color c = state.mpb.GetColor(transparencyProperty);
                Color originalColor = state.originalColor;

                if (currentHitRenderers.Contains(renderer))
                {
                    // Object is an obstacle, fade it in (to transparent).
                    c.a = Mathf.Lerp(c.a, transparentAlpha, Time.deltaTime * fadeSpeed);
                }
                else
                {
                    // Object is no longer an obstacle, fade it out (to opaque).
                    c.a = Mathf.Lerp(c.a, originalColor.a, Time.deltaTime * fadeSpeed);
                }

                state.mpb.SetColor(transparencyProperty, c);
                renderer.SetPropertyBlock(state.mpb);

                // If the object is fully opaque and no longer an obstacle, remove it.
                if (!currentHitRenderers.Contains(renderer) && Mathf.Abs(c.a - originalColor.a) < 0.01f)
                {
                    renderer.SetPropertyBlock(null);
                    _affectedObstacles.Remove(renderer);
                }
            }
        }

        /// <summary>
        /// Cleans up state when the script is disabled or destroyed.
        /// </summary>
        private void OnDisable()
        {
            // Ensure all objects are reset to their original state.
            ResetAllObstacles();
        }

        /// <summary>
        /// Resets all obstacles back to their original color/alpha.
        /// </summary>
        private void ResetAllObstacles()
        {
            foreach (var renderer in _affectedObstacles.Keys)
            {
                if (renderer != null)
                {
                    renderer.SetPropertyBlock(null);
                }
            }
            _affectedObstacles.Clear();
        }
    }
}

