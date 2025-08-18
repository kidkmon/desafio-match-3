using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views
{
    [RequireComponent(typeof(SwipeInput))]
    public class TileSpotView : MonoBehaviour
    {
        public event Action<Vector2Int, Vector2Int> Swiped;

        [SerializeField] private Image _tileImage;

        private SwipeInput _swipeInput;

        private int _x;
        private int _y;

        #region Unity
        private void Awake()
        {
            _swipeInput = GetComponent<SwipeInput>();
            _swipeInput.OnSwipeComplete += OnTileSwipe;
        }
        #endregion

        public Tween AnimatedSetTile(GameObject tile)
        {
            Image currentImage = _tileImage;
            _tileImage.transform.SetParent(tile.transform.parent);
            _tileImage.transform.DOMove(tile.transform.position, 0.3f);
            _tileImage = tile.GetComponent<Image>();

            tile.GetComponentInParent<TileSpotView>().SetTileImage(currentImage);
            tile.transform.SetParent(transform);
            tile.transform.DOKill();
    
            return tile.transform.DOMove(transform.position, 0.3f);
        }

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void SetColorTile(Color color)
        {
            _tileImage.color = color;
        }

        public void SetTileImage(Image tileImage)
        {
            _tileImage = tileImage;
        }

        public void ResetTilePosition()
        {
            _tileImage.rectTransform.offsetMin = new Vector2(_tileImage.rectTransform.offsetMin.x, 4);
            _tileImage.rectTransform.offsetMax = new Vector2(_tileImage.rectTransform.offsetMax.x, -4);
        }
        public GameObject Tile => _tileImage.gameObject;

        private void OnTileSwipe(Vector2Int direction)
        {
            Vector2Int from = new(_x, _y);
            Vector2Int to = from + direction;

            if (to.x >= 0 && to.y >= 0 && to.x < EnvironmentConfigs.Instance.GameConfig.BoardWidth && to.y < EnvironmentConfigs.Instance.GameConfig.BoardHeight)
            {
                Swiped?.Invoke(from, to);
            }
        }
    }
}
