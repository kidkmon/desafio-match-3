using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class GameManager : Singleton<GameManager>
    {
        public DifficultyLevel Level { get; private set; }

        void Start()
        {
            ScoreSystem.Instance.Initialize();
        }

        #region Public Methods

        public void SetDifficultyLevel(DifficultyLevel level)
        {
            Level = level;
        }
        
        #endregion
    }
}
