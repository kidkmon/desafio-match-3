using TMPro;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class LevelController : MonoBehaviour
    {
        [Header("Level Moves")]
        [SerializeField] private TextMeshProUGUI _leftMovesText;

        [Header("Level Goals")]
        [SerializeField] private Transform _goalsContainer;
        [SerializeField] private GameObject _tileGoalPrefab;

        private Dictionary<int, TileGoalView> _tileGoalsDict;
        private int _leftMoves;

        void Start()
        {
            _tileGoalsDict = new Dictionary<int, TileGoalView>();
        }

        public void SetupLevel(LevelConfig level)
        {
            _leftMoves = level.MaxMoves;
            _leftMovesText.text = $"Left Moves: {_leftMoves}";

            foreach (LevelGoal goal in level.Goals)
            {
                TileGoalView goalView = Instantiate(_tileGoalPrefab, _goalsContainer).GetComponent<TileGoalView>();
                Color color = EnvironmentConfigs.Instance.TileAssetCollection.AvailableTileAssets[goal.TileId].Color;
                goalView.Setup(goal.Quantity, color);
                _tileGoalsDict.Add(goal.TileId, goalView);
                goalView.OnGoalCompleted += CheckLevelStatus;
            }
        }

        public void ClearLevel()
        {
            foreach (TileGoalView goalView in _tileGoalsDict.Values)
            {
                goalView.OnGoalCompleted -= CheckLevelStatus;
                Destroy(goalView.gameObject);
            }

            _tileGoalsDict.Clear();
        }

        public void UpdateLevelGoal(int id)
        {
            if (_tileGoalsDict.ContainsKey(id))
            {
                _tileGoalsDict[id].DecreaseTileGoal();
            }
        }

        public bool CanUpdateLeftMoves()
        {
            if (_leftMoves - 1 > 0)
            {
                _leftMoves -= 1;
                UpdateLeftMoves(_leftMoves);
                return true;
            }

            UpdateLeftMoves(0);
            return false;
        }

        public void IncreaseLeftMoves()
        {
            _leftMoves += EnvironmentConfigs.Instance.GameConfig.MovesToIncrease;
            UpdateLeftMoves(_leftMoves);
        }

        private void CheckLevelStatus()
        {
            if (HasCompletedLevel())
            {
                GameManager.Instance.ShowWinPopup();
            }
        }

        private bool HasCompletedLevel()
        {
            foreach (TileGoalView goalView in _tileGoalsDict.Values)
            {
                if (!goalView.GoalCompleted) return false;
            }
            return true;
        }

        private void UpdateLeftMoves(int leftMoves)
        {
            _leftMoves = leftMoves;
            _leftMovesText.text = $"Left Moves: {_leftMoves}";
        }
    }
}
