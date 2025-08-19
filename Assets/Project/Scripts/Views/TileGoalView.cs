using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3
{
    public class TileGoalView : MonoBehaviour
    {
        public event Action OnGoalCompleted;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _tileText;
        [SerializeField] private Image _tileImage;

        private int _quantity;
        public bool GoalCompleted { get; private set; }

        public void Setup(int quantity, Color tileColor)
        {
            GoalCompleted = false;
            _quantity = quantity;
            _tileImage.color = tileColor;
            UpdateTileUI(_quantity);
        }

        public void DecreaseTileGoal()
        {
            if (GoalCompleted) return;

            if (_quantity - 1 > 0)
            {
                _quantity -= 1;
            }
            else
            {
                CompleteTileGoal();
            }

            UpdateTileUI(_quantity);
        }

        private void UpdateTileUI(int value)
        {
            _tileText.text = value.ToString();
        }

        private void CompleteTileGoal()
        {
            _quantity = 0;
            GoalCompleted = true;
            _tileImage.color = new Color(_tileImage.color.r, _tileImage.color.g, _tileImage.color.b, 0.5f);
            _tileText.color = new Color(1f, 1f, 1f, 0.5f);
            OnGoalCompleted?.Invoke();
        }
    }
}
