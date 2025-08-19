using TMPro;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class HUDManager : Singleton<HUDManager>
    {
        [Header("UI Elements")]
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _levelText;
        [SerializeField] TextMeshProUGUI _heartText;

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

        public void UpdateHeartUI(int value)
        {
            _heartText.text = value.ToString();
        }

    }
}
