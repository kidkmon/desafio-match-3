using UnityEngine;
using UnityEngine.Events;

namespace Gazeus.DesafioMatch3
{
    public class ScoreSystem : Singleton<ScoreSystem>
    {
        [HideInInspector] public UnityEvent<int> OnScoreUpdated;

        private int _score;

        public void Initialize()
        {
            SetScore(0);
        }

        void SetScore(int value)
        {
            _score = value;
            OnScoreUpdated?.Invoke(_score);
        }

        #region Public Methods

        public int Score => _score;
        public void AddScore(int value) => SetScore(_score + value);

        public bool TryDeductScore(int deductValue)
        {
            if (_score - deductValue < 0) return false;
            SetScore(_score - deductValue);
            return true;
        }

        #endregion
    }
}
