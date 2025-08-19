using Gazeus.DesafioMatch3.Controllers;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Controllers")]
        [SerializeField] private GameController _gameController;
        [SerializeField] private LevelController _levelController;

        public DifficultyLevel Difficulty { get; private set; }
        public int Level { get; private set; }

        private void Start()
        {
            Level = 1;
            AudioManager.Instance.PlayIntroSound();
        }

        private void SetGameLevel(DifficultyLevel difficulty, int level = 1)
        {
            Difficulty = difficulty;
            Level = level;
            LogSystem.Instance.LogStartLevel(difficulty.ToString(), level);
        }

        #region Public Methods

        public void StartGame(DifficultyLevel difficulty, int level)
        {
            SetGameLevel(difficulty, level);
            _gameController.StartLevel(difficulty, level);

            LevelConfig levelConfig = EnvironmentConfigs.Instance.LevelConfigCollection.GetLevel(level);
            _levelController.SetupLevel(levelConfig);

            HUDManager.Instance.UpdateLevelUI(level);
            HUDManager.Instance.UpdateHeartUI(EnvironmentConfigs.Instance.GameConfig.InitialLife);
        }

        public void UpdateLevelGoal(int id)
        {
            _levelController.UpdateLevelGoal(id);
        }

        public void UpdateLeftMoves()
        {
            _levelController.UpdateLeftMoves();
        }

        #endregion
    }
}
