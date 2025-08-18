using TMPro;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class HUDManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] TextMeshProUGUI _scoreText;

        void OnEnable()
        {
            ScoreSystem.Instance.OnScoreUpdated.AddListener(UpdateScoreUI);
        }

        void OnDisable()
        {
            ScoreSystem.Instance.OnScoreUpdated.RemoveListener(UpdateScoreUI);
        }

        void UpdateScoreUI(int score) => _scoreText.text = score.ToString();

    }
}
