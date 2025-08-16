using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gazeus.DesafioMatch3
{
    public class SwipeInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<Vector2Int> OnSwipeComplete;

        [SerializeField] private float _minSwipeDistance = 50f;

        private Vector2Int _direction;
        private Vector2 _startPos;

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPos = eventData.position;
            _direction = Vector2Int.zero;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Vector2 delta = eventData.position - _startPos;
            if (delta.magnitude < _minSwipeDistance)
                return;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                _direction = delta.x > 0 ? Vector2Int.right : Vector2Int.left;
            else
                _direction = delta.y > 0 ? Vector2Int.down : Vector2Int.up;

            OnSwipeComplete?.Invoke(_direction);
        }
    }

}

