using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3
{
    public class TileGoalView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _tileText;
        [SerializeField] private Image _tileImage;

        private int _quantity;
        private bool _goalComplete;

        public void Setup(int quantity, Color tileColor)
        {
            _goalComplete = false;
            _quantity = quantity;
            _tileImage.color = tileColor;
            UpdateTileUI(_quantity);
        }

        public void DecreaseTileGoal()
        {
            if (_goalComplete) return;

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
            _goalComplete = true;
            _tileImage.color = new Color(_tileImage.color.r, _tileImage.color.g, _tileImage.color.b, 0.5f);
            _tileText.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }
}
