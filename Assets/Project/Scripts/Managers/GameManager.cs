using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class GameManager : Singleton<GameManager>
    {
        public DifficultyLevel Difficulty { get; private set; }
        public int Level { get; private set; }

        void Start()
        {
            ScoreSystem.Instance.Initialize();
        }

        #region Public Methods

        public void SetGameLevel(DifficultyLevel difficulty, int level = 1)
        {
            Difficulty = difficulty;
            Level = level;

            LogSystem.Instance.LogStartLevel(level);
        }

        #endregion
    }
}
