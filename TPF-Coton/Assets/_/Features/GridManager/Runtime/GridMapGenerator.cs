using System;
using UnityEngine;

namespace GridManager.Runtime
{
    public class GridMapGenerator : MonoBehaviour
    {
        #region Api Unity

        private void Start()
        {
            if (_generateOnStart) GenerateGrid();
        }

        #endregion
        
        
        #region Utils

        public GameObject GetCell(int x, int y, int z)
        {
            return _gridCells[x, y, z];
        }
        
        #endregion
        
        
        #region Main Method
        
        [ContextMenu("Generate Grid")]
        private void GenerateGrid()
        {
            // Vérification des dimensions
            if (_width <= 0 || _height <= 0 || _gridHeightLevels <= 0)
            {
                Debug.LogError("Grid dimensions must be greater than 0.");
                return;
            }
            
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
            
            if (_tilePrefab == null)
            {
                Debug.LogWarning($"Tile prefab not set for grid : {gameObject.name}");
                return;
            }
            
            _gridCells = new GameObject[_width, _gridHeightLevels, _height];
            
            for (int y = 0; y < _gridHeightLevels; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    for (int z = 0; z < _height; z++)
                    {
                        Vector3 pos = new Vector3(x * _spacing, y * _spacing, z* _spacing);
                        GameObject tile = Instantiate(_tilePrefab, pos, Quaternion.identity, transform);
                        tile.name = $"Tile_{y},{x},{z}";
                        _gridCells[x, y, z] = tile;
                    }
                }
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [Header("Grid Setting")]
        [SerializeField] private int _width = 10;
        [SerializeField] private int _height = 10;
        [SerializeField] private int _gridHeightLevels = 3;
        [SerializeField] private float _spacing = 1f;

        [Header("Tile Prefab")]
        [SerializeField] private GameObject _tilePrefab;
        private GameObject[,,] _gridCells;
        
        [Header("Generate Grid Start")]
        [SerializeField] private bool _generateOnStart = true;
        
        #endregion
    }
}
