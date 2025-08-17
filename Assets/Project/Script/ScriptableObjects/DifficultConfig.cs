using UnityEngine;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "DifficultConfig", menuName = "Gameplay/Difficult/DifficultConfig")]
    public class DifficultConfig : ScriptableObject
    {
        [SerializeField] private DifficultyLevel _difficultyLevel;
        [SerializeField] private int _colorQuantity;

        public DifficultyLevel DifficultyLevel => _difficultyLevel;
        public int ColorQuantity => _colorQuantity;
    }
}
