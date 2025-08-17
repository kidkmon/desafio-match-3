using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "DifficultConfigCollection", menuName = "Gameplay/Difficult/DifficultConfigCollection")]
    public class DifficultConfigCollection : ScriptableObject
    {
        [SerializeField] private DifficultConfig[] _difficultConfigs;

        public DifficultConfig[] DifficultConfigs => _difficultConfigs;

        Dictionary<DifficultyLevel, DifficultConfig> _difficultLevelConfig;

        public void Initialize()
        {
            _difficultLevelConfig = new Dictionary<DifficultyLevel, DifficultConfig>();
            foreach (DifficultConfig config in _difficultConfigs)
            {
                if (!_difficultLevelConfig.ContainsKey(config.DifficultyLevel))
                {
                    _difficultLevelConfig.Add(config.DifficultyLevel, config);
                }
            }
        }

        public DifficultConfig GetConfigByDifficulty(DifficultyLevel difficulty)
        {
            if (_difficultLevelConfig.TryGetValue(difficulty, out DifficultConfig config))
            {
                return config;
            }

            Debug.LogWarning($"No configuration found for difficulty: {difficulty}");
            return null;
        }
    }
}