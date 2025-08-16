using System;
using DG.Tweening;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Views
{
    [RequireComponent(typeof(SwipeInput))]
    public class TileSpotView : MonoBehaviour
    {
        public event Action<Vector2Int, Vector2Int> Swiped;

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
            tile.transform.SetParent(transform);
            tile.transform.DOKill();

            return tile.transform.DOMove(transform.position, 0.3f);
        }

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void SetTile(GameObject tile)
        {
            tile.transform.SetParent(transform, false);
            tile.transform.position = transform.position;
        }

        private void OnTileSwipe(Vector2Int direction)
        {
            Vector2Int from = new(_x, _y);
            Vector2Int to = from + direction;

            if (to.x >= 0 && to.y >= 0)
            {
                Swiped?.Invoke(from, to);
            }
        }
    }
}
