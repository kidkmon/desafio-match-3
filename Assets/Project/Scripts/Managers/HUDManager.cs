using TMPro;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class HUDManager : Singleton<HUDManager>
    {
        [Header("UI Elements")]
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _levelText;

        void OnEnable()
        {
            ScoreSystem.Instance.OnScoreUpdated.AddListener(UpdateScoreUI);
            ScoreSystem.Instance.Initialize();
        }

        void OnDisable()
        {
            ScoreSystem.Instance.OnScoreUpdated.RemoveListener(UpdateScoreUI);
        }

        void UpdateScoreUI(int score) => _scoreText.text = score.ToString();
        
        public void UpdateLevelUI(int level)
        {
            _levelText.text = $"Level {level}";
        }

    }
}
