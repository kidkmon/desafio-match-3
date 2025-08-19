using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "LevelConfigCollection", menuName = "Gameplay/Level/LevelConfigCollection")]
    public class LevelConfigCollection : ScriptableObject
    {
        [SerializeField] private LevelConfig[] _levelConfigs;

        Dictionary<int, LevelConfig> _levelConfigsDict;

        public void Initialize()
        {
            _levelConfigsDict = new Dictionary<int, LevelConfig>();
            foreach (LevelConfig config in _levelConfigs)
            {
                if (!_levelConfigsDict.ContainsKey(config.Id))
                {
                    _levelConfigsDict.Add(config.Id, config);
                }
            }
        }

        public LevelConfig GetLevel(int level)
        {
            if (_levelConfigsDict.TryGetValue(level, out LevelConfig config))
            {
                return config;
            }

            Debug.LogWarning($"No configuration found for level: {level}");
            return null;
        }
    }
}
