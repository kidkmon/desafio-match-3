using Gazeus.DesafioMatch3.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Gazeus.DesafioMatch3
{
    public class GameManager : Singleton<GameManager>
    {
        [HideInInspector] public UnityEvent<int> OnLevelUpdated;
        [HideInInspector] public UnityEvent<int> OnLifeUpdated;

        [Header("Controllers")]
        [SerializeField] private GameController _gameController;
        [SerializeField] private LevelController _levelController;

        public DifficultyLevel Difficulty { get; private set; }
        public int Level { get; private set; }
        public int Life { get; private set; }

        private void Start()
        {
            Level = 1;
        }

        private void SetGameLevel(DifficultyLevel difficulty, int level)
        {
            Difficulty = difficulty;
            Level = level;
            OnLevelUpdated?.Invoke(Level);
            LogSystem.Instance.LogStartLevel(difficulty.ToString(), level);
        }

        private void SetupNewGame(DifficultyLevel difficulty, int level)
        {
            _gameController.StartLevel(difficulty);

            LevelConfig levelConfig = EnvironmentConfigs.Instance.LevelConfigCollection.GetLevel(level);
            _levelController.ClearLevel();
            _levelController.SetupLevel(levelConfig);
        }

        #region Public Methods

        public void StartNewGame(DifficultyLevel difficulty, int level)
        {
            SetGameLevel(difficulty, level);
            Life = EnvironmentConfigs.Instance.GameConfig.InitialLife;

            SetupNewGame(difficulty, level);

            HUDManager.Instance.UpdateLevelUI(level);
            HUDManager.Instance.UpdateLifeUI(Life);
        }

        public bool CanDecreaseLife()
        {
            if (Life - 1 > 0) return true;
            return false;
        }

        public void DecreaseLife()
        {
            if (CanDecreaseLife())
            {
                Life -= 1;
                OnLifeUpdated?.Invoke(Life);
            }
        }

        public void ShowWinPopup()
        {
            AudioManager.Instance.PlayLevelWinSound();
            _gameController.ShowWinPopup(() =>
            {
                StartNewGame(Difficulty, Level + 1);
            });

        }

        public void ShowLostPopup()
        {
            AudioManager.Instance.PlayLevelLostSound();
            _gameController.ShowLostPopup();
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
