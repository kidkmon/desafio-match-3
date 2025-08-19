using Gazeus.DesafioMatch3.Controllers;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameController _gameController;

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
        }

        #endregion
    }
}
