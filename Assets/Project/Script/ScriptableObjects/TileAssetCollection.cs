using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "TileAssetCollection", menuName = "Gameplay/Tile/TileAssetCollection")]
    public class TileAssetCollection : ScriptableObject
    {
        [SerializeField] private TileAssetConfig[] _normalTileAssetConfigs;
        [SerializeField] private TileAssetConfig[] _specialTileAssetConfigs;

        private TileAssetConfig[] _availableTileAssets;
        public TileAssetConfig[] AvailableTileAssets => _availableTileAssets;

        public void InitializeRandomTiles(int size)
        {
            if (size > _normalTileAssetConfigs.Length)
            {
                Debug.LogWarning($"Requested size {size} exceeds available normal tile assets count. Using all available assets.");
                size = _normalTileAssetConfigs.Length;
            }

            _availableTileAssets = new TileAssetConfig[size];
            List<int> usedIndexes = new();

            for (int i = 0; i < size; i++)
            {
                int randomIndex = Random.Range(0, _normalTileAssetConfigs.Length);
                if (usedIndexes.Contains(randomIndex))
                {
                    i--;
                    continue;
                }

                _availableTileAssets[i] = _normalTileAssetConfigs[randomIndex];
                usedIndexes.Add(randomIndex);
            }
        }

        public TileAssetConfig GetAnySpecialTileAsset()
        {
            if (_specialTileAssetConfigs.Length == 0)
            {
                Debug.LogWarning("No special tile assets available.");
                return null;
            }

            int randomIndex = Random.Range(0, _specialTileAssetConfigs.Length);
            return _specialTileAssetConfigs[randomIndex];
        }
    }
}
