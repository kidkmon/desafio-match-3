using TMPro;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class HUDManager : Singleton<HUDManager>
    {
        [Header("UI Elements")]
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _levelText;
        [SerializeField] TextMeshProUGUI _lifeText;

        void OnEnable()
        {
            ScoreSystem.Instance.OnScoreUpdated.AddListener(UpdateScoreUI);
            GameManager.Instance.OnLevelUpdated.AddListener(UpdateLevelUI);
            GameManager.Instance.OnLifeUpdated.AddListener(UpdateLifeUI);
            ScoreSystem.Instance.Initialize();
        }

        void OnDisable()
        {
            ScoreSystem.Instance.OnScoreUpdated.RemoveListener(UpdateScoreUI);
            GameManager.Instance.OnLevelUpdated.RemoveListener(UpdateLevelUI);
            GameManager.Instance.OnLifeUpdated.RemoveListener(UpdateLifeUI);
        }

        void UpdateScoreUI(int score) => _scoreText.text = score.ToString();

        public void UpdateLevelUI(int level)
        {
            _levelText.text = $"Level {level}";
        }

        public void UpdateLifeUI(int value)
        {
            _lifeText.text = value.ToString();
        }

    }
}
