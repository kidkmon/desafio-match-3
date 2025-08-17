using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class EnvironmentConfigs : Singleton<EnvironmentConfigs>
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private TileAssetCollection _tileAssetCollection;
        [SerializeField] private DifficultConfigCollection _difficultConfigCollection;

        public GameConfig GameConfig => _gameConfig;
        public TileAssetCollection TileAssetCollection => _tileAssetCollection;
        public DifficultConfigCollection DifficultConfigCollection => _difficultConfigCollection;

        public DifficultyLevel Level { get; private set; }

        private void Awake()
        {
            _difficultConfigCollection.Initialize();
        }
        
        public void SetDifficultyLevel(DifficultyLevel level)
        {
            Level = level;
        }
    }
}
